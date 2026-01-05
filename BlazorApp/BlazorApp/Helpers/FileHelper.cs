using BlazorApp.Client.Models;
using BlazorApp.Client.Models.Request;
using BlazorApp.Client.Settings;
using DataContext.Context;
using DataContext.Models;
using FileExstend;
using FileExstend.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Runtime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BlazorApp.Helpers
{
    public class FileHelper
    {
        private static string GenerateRandomFileNameWithOriginalExtension(string fileName)
        {
            string extension = FileExstensionHelper.GetExtension(fileName);
            return Guid.NewGuid().ToString("N") + extension;
        }
        private static string GenerateFileName(string path, string fileName, out string trustedFileName)
        {
            int length = 5;
            for (int i = 0; i < length; i++)
            {
                trustedFileName = GenerateRandomFileNameWithOriginalExtension(fileName);
                string pathFile = Path.Combine(path, trustedFileName);
                if (File.Exists(pathFile) == false)
                    return pathFile;
            }
            throw new IOException("Ошибка генерации имени файла, не удалось создать укальное имя");
        }
        private static string ComputeMd5Hash(byte[] data)
        {
            using MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(data);
            return BitConverter.ToString(hashBytes).ToLowerInvariant();
        }

        private static string ComputeSha256Hash(byte[] data)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(data);
            return BitConverter.ToString(hashBytes).ToLowerInvariant();
        }

        protected readonly IWebHostEnvironment _webHostEnvironment;
        protected readonly IOptionsMonitor<FileUploadSettings> _options;
        protected readonly IDbContextFactory<DBContext> _contextFactory;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ILogger<FileHelper> _logger;
        /// <summary>
        /// Куда сохраняются загруженные файлы
        /// </summary>
        protected readonly string _folder;
        protected FileUploadSettings _settings => _options.CurrentValue;
        protected const string UnsafeFolderName = "unsafe_uploads";
        public FileHelper(
            IWebHostEnvironment webHostEnvironment,
            IOptionsMonitor<FileUploadSettings> options,
            IDbContextFactory<DBContext> contextFactory,
            UserManager<ApplicationUser> userManager,
            ILogger<FileHelper> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _options = options;
            _logger = logger;
            _contextFactory = contextFactory;
            _userManager = userManager;
            _folder = Path.Combine(_webHostEnvironment.WebRootPath, UnsafeFolderName);
        }

        public async Task<IEnumerable<FileItem>> GetFiles(FileGetRequest request, ClaimsPrincipal? user)
        {
            var userId = (await _userManager.GetUserAsync(user))?.Id;
            
            string targetFolder = Path.Combine(_folder, userId ?? string.Empty);

            using DBContext context = await _contextFactory.CreateDbContextAsync();

            int page = Math.Max(request.Page, 1);
            int size = Math.Max(request.Size, 1);

            var dbMiniFiles = await context.Files
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.InsertDateUtc)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Size,
                    x.InsertDateUtc
                })
                .ToListAsync();

            if(Directory.Exists(targetFolder) == false)
                Directory.CreateDirectory(targetFolder);

            IEnumerable<string> discFiles = Directory.GetFiles(targetFolder)
                .Select(x => FileExstensionHelper.GetFileName(x));

            var filesNotFoundDisc = dbMiniFiles.Where(x => discFiles.Contains(x.Name) == false).Select(x => x.Id);

            var dbPastFile = await context.Files
                .AsNoTracking()
                .Where(x => filesNotFoundDisc.Contains(x.Id))
                .ToListAsync();
            foreach (var dbFile in dbPastFile)
            {
                await File.WriteAllBytesAsync(Path.Combine(targetFolder, dbFile.Name), dbFile.Data);
            }

            return dbMiniFiles.Select(x => new FileItem()
            {
                Id = x.Id,
                Name = x.Name,
                Path = $"{UnsafeFolderName}/{userId}/{x.Name}",
                Length = x.Size,
                CreationTime = x.InsertDateUtc,
            });
        }
        /// <summary>
        /// Выгрузка данных файла в память
        /// </summary>
        private async Task<byte[]> GetFileBytes(IBrowserFile file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await file.OpenReadStream(long.MaxValue).CopyToAsync(ms);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// Выгрузка данных файла в память
        /// </summary>
        private async Task<byte[]> GetFileBytes(IFormFile file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// Сохранение файла на диск
        /// </summary>
        private async Task<bool> SaveFileFromDisc(byte[] bytes, string pathFile)
        {
            using (FileStream fs = new FileStream(pathFile, FileMode.Create))
            {
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
            return true;
        }
        /// <summary>
        /// Сохранение файла в БД
        /// </summary>
        private async Task<bool> SaveFileFromBD(byte[] bytes, string trustedFileName, ClaimsPrincipal user)
        {
            using DBContext context = await _contextFactory.CreateDbContextAsync();
            await context.Files.AddAsync(new FileEntity()
            {
                Name = trustedFileName,
                Data = bytes,
                InsertDateUtc = DateTime.UtcNow,
                Size = bytes.Length,
                UserId = (await _userManager.GetUserAsync(user))?.Id,
                Md5Hash = ComputeMd5Hash(bytes),
                Sha256Hash = ComputeSha256Hash(bytes),
            });

            return await context.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// Проверка что пользователь не пытается вставить одинаковые файлы
        /// </summary>
        private async Task<bool> IsDublicateFile(byte[] bytes, ClaimsPrincipal user)
        {
            Task<ApplicationUser?> taskFindUser = _userManager.GetUserAsync(user);
            string md5Hash = ComputeMd5Hash(bytes);
            string sha256Hash = ComputeSha256Hash(bytes);
            string? userId = (await taskFindUser)?.Id;

            using (DBContext context = await _contextFactory.CreateDbContextAsync())
            {
                return await context.Files.AnyAsync(x => x.UserId == userId && x.Md5Hash == md5Hash && x.Sha256Hash == sha256Hash);
            }
        }

        private async Task<Result<string>> SaveFile(string path, string fileName, Func<Task<byte[]>> func, ClaimsPrincipal user)
        {
            Result<string> result;
            try
            {
                if (FileExstensionHelper.GetExtensionType(fileName).Type == FileType.UNKNOWN)
                    throw new InvalidOperationException("Неизвестный тип файла");

                var userId = (await _userManager.GetUserAsync(user))?.Id;
                string pathFile = GenerateFileName(Path.Combine(path, userId ?? string.Empty), fileName, out string trustedFileName);
                // создааем буфер тк планируем сохранить файл на диск и в бд
                byte[] bytes = await func.Invoke();

                if (await IsDublicateFile(bytes, user) == true)
                    throw new Exception("Попытка добавить повторяющийся файл");

                // паралельно сохраняем в БД и на диск
                var taskSaveFileFromDisc = SaveFileFromDisc(bytes, pathFile);
                var taskSaveFileFromDB = SaveFileFromBD(bytes, trustedFileName, user);

                await Task.WhenAll(taskSaveFileFromDisc, taskSaveFileFromDB);

                bool saveFileFromDisc = await taskSaveFileFromDisc;
                bool saveFileFromDB = await taskSaveFileFromDB;

                if (saveFileFromDisc == true && saveFileFromDB == false)
                {
                    File.Delete(pathFile);
                    throw new Exception("Не удалось сохранить файл в БД");
                }

                _logger.LogInformation($"Небезопасное имя файла: {fileName} Файл сохранен: {trustedFileName}");
                result = new Result<string>()
                {
                    Success = true,
                    Value = trustedFileName,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Файл: {fileName} Ошибка: {ex.Message}");
                result = new Result<string>()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
            return result;
        }
        private async Task<Result<string>> SaveFile(string path, IBrowserFile file, ClaimsPrincipal user)
        {
            return await SaveFile(path, file.Name, async () => await GetFileBytes(file), user);
        }
        private async Task<Result<string>> SaveFile(string path, IFormFile file, ClaimsPrincipal user)
        {
            return await SaveFile(path, file.FileName, async () => await GetFileBytes(file), user);
        }

        public async Task<Result<List<FileUploadResult>>> SaveFiles(IEnumerable<IBrowserFile> files, ClaimsPrincipal user)
        {
            Result<List<FileUploadResult>> result = new Result<List<FileUploadResult>>()
            {
                Success = true,
                Value = new List<FileUploadResult>()
            };
            if (_settings.EnableMaxAllowedFiles)
                files = files.Take(_settings.MaxAllowedFiles);
            if (_settings.EnableMaxFileSize)
                files = files.Where(x => x.Size <= _settings.MaxFileSize);

            foreach (IBrowserFile file in files)
            {
                Result<string> saveResultFile = await SaveFile(_folder, file, user);
                result.Value.Add(new FileUploadResult()
                {
                    IsSave = saveResultFile.Success,
                    Name = saveResultFile.Value ?? file.Name
                });
            }
            return result;
        }
        public async Task<Result<List<FileUploadResult>>> SaveFiles(IEnumerable<IFormFile> files, ClaimsPrincipal user)
        {
            Result<List<FileUploadResult>> result = new Result<List<FileUploadResult>>()
            {
                Value = new List<FileUploadResult>()
            };

            if (_settings.EnableMaxAllowedFiles)
                files = files.Take(_settings.MaxAllowedFiles);
            if (_settings.EnableMaxFileSize)
                files = files.Where(x => x.Length <= _settings.MaxFileSize);

            foreach (IFormFile file in files)
            {
                Result<string> saveResultFile = await SaveFile(_folder, file, user);
                result.Value.Add(new FileUploadResult()
                {
                    IsSave = saveResultFile.Success,
                    Name = saveResultFile.Value ?? file.Name
                });
            }
            return result;
        }

        public async Task<Result<byte[]>> DownloadAsync(string filePath, ClaimsPrincipal user)
        {
            Result<byte[]> result = new Result<byte[]>();

            string path = Path.Combine(_webHostEnvironment.WebRootPath, filePath);

            if (File.Exists(path) == false)
            {
                using (DBContext context = await _contextFactory.CreateDbContextAsync())
                {
                    var userId = (await _userManager.GetUserAsync(user))?.Id;
                    string fileName = FileExstensionHelper.GetFileName(in path);

                    FileEntity? dbFile = await context.Files
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.UserId == userId && x.Name == fileName);
                    if (dbFile != null)
                    {
                        await File.WriteAllBytesAsync(path, dbFile.Data);
                        result.Success = true;
                        result.Value = dbFile.Data;
                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = "Не найден файл";
                        return result;
                    }
                }

            }
            else
            {
                result.Success = true;
                result.Value = await File.ReadAllBytesAsync(path);
                return result;
            }
        }
        public async Task<Result> Delete(string filePath, ClaimsPrincipal user)
        {
            Result result = new Result();

            string path = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
            var userId = (await _userManager.GetUserAsync(user))?.Id;
            string fileName = FileExstensionHelper.GetFileName(in path);

            using (DBContext context = await _contextFactory.CreateDbContextAsync())
            {
                context.Files.RemoveRange(
                    context.Files.Where(x => x.UserId == userId && x.Name == fileName)
                    );
                await context.SaveChangesAsync();
            }

            if (File.Exists(path) == false)
            {
                result.Success = false;
                result.ErrorMessage = "Не найден файл";
                return result;
            }
            else
            {
                result.Success = true;
                File.Delete(path);
                return result;
            }
        }
    }
}
