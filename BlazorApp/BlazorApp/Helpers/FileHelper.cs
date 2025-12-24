using BlazorApp.Client.Models;
using BlazorApp.Client.Settings;
using FileExstend;
using FileExstend.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.IO;
using System.Runtime;

namespace BlazorApp.Helpers
{
    public class FileHelper
    {
        private static string GenerateRandomFileNameWithOriginalExtension(in IBrowserFile file)
        {
            string extension = FileExstensionHelper.GetExtension(file.Name);
            return Guid.NewGuid().ToString("N") + extension;
        }
        private static string GenerateRandomFileNameWithOriginalExtension(in IFormFile file)
        {
            string extension = FileExstensionHelper.GetExtension(file.FileName);
            return Guid.NewGuid().ToString("N") + extension;
        }

        protected readonly IWebHostEnvironment _webHostEnvironment;
        protected readonly IOptionsMonitor<FileUploadSettings> _options;
        protected FileUploadSettings _settings => _options.CurrentValue;
        protected readonly ILogger<FileHelper> _logger;
        protected readonly string _folder;
        public FileHelper(IWebHostEnvironment webHostEnvironment, IOptionsMonitor<FileUploadSettings> options, ILogger<FileHelper> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _options = options;
            _logger = logger;
            _folder = Path.Combine(_webHostEnvironment.WebRootPath, "unsafe_uploads");
        }

        public IEnumerable<FileItem> GetFiles()
        {
            return Directory.GetFiles(_folder)
                .Select(x => new FileInfo(x))
                .Select(x => new FileItem()
                {
                    Name = x.Name,
                    Path = $"unsafe_uploads/{x.Name}",
                    Length = x.Length,
                    CreationTime = x.CreationTime,
                });
        }

        public async Task<Result<string>> SaveFile(string path, IBrowserFile file)
        {
            Result<string> result;
            try
            {
                if (FileExstensionHelper.GetExtensionType(file.Name).Type == FileType.UNKNOWN)
                    throw new InvalidOperationException("Неизвестный тип файла");

                string trustedFileName = GenerateRandomFileNameWithOriginalExtension(in file);

                string pathFile = Path.Combine(path, trustedFileName);

                await using FileStream fs = new(pathFile, FileMode.Create);
                await file.OpenReadStream(_settings.MaxFileSize).CopyToAsync(fs);

                _logger.LogInformation($"Небезопасное имя файла: {file.Name} Файл сохранен: {trustedFileName}");
                result = new Result<string>()
                {
                    Success = true,
                    Value = trustedFileName,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Файл: {file.Name} Ошибка: {ex.Message}");
                result = new Result<string>()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
            return result;
        }
        public async Task<Result<string>> SaveFile(string path, IFormFile file)
        {
            Result<string> result;
            try
            {
                if (FileExstensionHelper.GetExtensionType(file.Name).Type == FileType.UNKNOWN)
                    throw new InvalidOperationException("Неизвестный тип файла");

                string trustedFileName = GenerateRandomFileNameWithOriginalExtension(in file);

                string pathFile = Path.Combine(path, trustedFileName);

                await using FileStream fs = new(pathFile, FileMode.Create);
                await file.OpenReadStream().CopyToAsync(fs);

                _logger.LogInformation($"Небезопасное имя файла: {file.FileName} Файл сохранен: {trustedFileName}");
                result = new Result<string>()
                {
                    Success = true,
                    Value = trustedFileName,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Файл: {file.Name} Ошибка: {ex.Message}");
                result = new Result<string>()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
            return result;
        }

        public async Task<Result<List<FileUploadResult>>> SaveFiles(IEnumerable<IBrowserFile> files)
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
                var saveResultFile = await SaveFile(_folder, file);
                result.Value.Add(new FileUploadResult()
                {
                    IsSave = saveResultFile.Success,
                    Name = saveResultFile.Value ?? file.Name
                });
            }
            return result;
        }
        public async Task<Result<List<FileUploadResult>>> SaveFiles(IEnumerable<IFormFile> files)
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
                var saveResultFile = await SaveFile(_folder, file);
                result.Value.Add(new FileUploadResult()
                {
                    IsSave = saveResultFile.Success,
                    Name = saveResultFile.Value ?? file.Name
                });
            }
            return result;
        }
        public async Task<Result<List<FileUploadResult>>> SaveFiles(IFormFileCollection files)
        {
            Result<List<FileUploadResult>> result = new Result<List<FileUploadResult>>()
            {
                Success = true,
                Value = new List<FileUploadResult>()
            };

            if (_settings.EnableMaxAllowedFiles)
                files = (IFormFileCollection)files.Take(_settings.MaxAllowedFiles);
            if (_settings.EnableMaxFileSize)
                files = (IFormFileCollection)files.Where(x => x.Length <= _settings.MaxFileSize);

            foreach (IFormFile file in files)
            {
                var saveResultFile = await SaveFile(_folder, file);
                result.Value.Add(new FileUploadResult()
                {
                    IsSave = saveResultFile.Success,
                    Name = saveResultFile.Value ?? file.Name
                });
            }
            return result;
        }

        public async Task<Result<byte[]>> DownloadAsync(string filePath)
        {
            Result<byte[]> result = new Result<byte[]>();

            string path = Path.Combine(_webHostEnvironment.WebRootPath, filePath);

            if (File.Exists(path) == false)
            {
                result.Success = false;
                result.ErrorMessage = "Не найден файл";
                return result;
            }
            else
            {
                result.Success = true;
                result.Value = await File.ReadAllBytesAsync(path);
                return result;
            }
        }
        public Result Delete(string filePath)
        {
            Result result = new Result<byte[]>();

            string path = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
            
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
