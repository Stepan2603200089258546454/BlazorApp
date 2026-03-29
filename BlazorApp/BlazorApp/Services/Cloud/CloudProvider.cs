using BlazorApp.Client.Models;
using BlazorApp.Client.Models.Cloud;
using BlazorApp.Client.Pages;
using BlazorApp.Client.Settings;
using DataContext.Context;
using DataContext.Models;
using DataContext.Models.Cloud;
using FileExstend;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorApp.Services.Cloud
{
    public class CloudProvider
    {
        protected readonly IDbContextFactory<DBContext> _dbContextFactory;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IWebHostEnvironment _webHostEnvironment;
        protected readonly IOptionsMonitor<FileUploadSettings> _optionsMonitorFileUploadSettings;
        protected const string MainFolder = "Cloudes";

        public CloudProvider(
            IWebHostEnvironment webHostEnvironment,
            IDbContextFactory<DBContext> dbContextFactory,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<FileUploadSettings> optionsMonitorFileUploadSettings)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContextFactory = dbContextFactory;
            _userManager = userManager;
            _optionsMonitorFileUploadSettings = optionsMonitorFileUploadSettings;
        }

        private static void ExistsOrCreateDirectory(string path)
        {
            if (Directory.Exists(path) == false) Directory.CreateDirectory(path);
        }

        public async Task<Result<IEnumerable<CloudModel>>> GetMainPageAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user) 
                    ?? throw new Exception("User not found");

                string pathMainFolder = Path.Combine(_webHostEnvironment.WebRootPath, MainFolder);

                ExistsOrCreateDirectory(pathMainFolder);

                using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                List<CloudModel> personalClouds = await context.PersonalClouds
                    .AsNoTracking()
                    .Where(x => x.UserId == appUser.Id)
                    .Select(x => new CloudModel()
                    {
                        Id = x.Id,
                        SystemName = x.SystemName,
                        DisplayName = x.DisplayName,
                        Description = x.Description,
                    })
                    .ToListAsync();

                return Result<IEnumerable<CloudModel>>.OnSuccess(personalClouds);
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result<IEnumerable<CloudModel>>.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CloudModel>>.OnError(ex);
            }
        }
        public async Task<Result> CreateCloudAsync(string displayName, string description, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user) 
                    ?? throw new Exception("User not found");

                using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                await context.PersonalClouds.AddAsync(new PersonalCloud()
                {
                    SystemName = Guid.NewGuid().ToString("N"),
                    DisplayName = displayName,
                    Description = description,
                    UserId = appUser.Id,
                });
                await context.SaveChangesAsync(cancellationToken);

                return Result.OnSuccess();
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result.OnError(ex);
            }
        }
        public async Task<Result> UpdateCloudAsync(Guid id, string displayName, string description, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user) 
                    ?? throw new Exception("User not found");

                using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                PersonalCloud cloud = await context.PersonalClouds.FirstOrDefaultAsync(c => c.Id == id && c.UserId == appUser.Id) 
                    ?? throw new Exception("Cloud not found");

                cloud.DisplayName = displayName;
                cloud.Description = description;

                await context.SaveChangesAsync(cancellationToken);

                return Result.OnSuccess();
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result.OnError(ex);
            }
        }
        public async Task<Result> DeleteCloudAsync(Guid id, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user) 
                    ?? throw new Exception("User not found");

                using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                PersonalCloud cloud = await context.PersonalClouds.FirstOrDefaultAsync(c => c.Id == id && c.UserId == appUser.Id) 
                    ?? throw new Exception("Cloud not found");

                context.PersonalClouds.Remove(cloud);

                await context.SaveChangesAsync(cancellationToken);

                return Result.OnSuccess();
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result.OnError(ex);
            }
        }

        public Result<FileUploadSettings> GetFileUploadSettings()
        {
            return Result<FileUploadSettings>.OnSuccess(_optionsMonitorFileUploadSettings.CurrentValue ?? FileUploadSettings.Empty);
        }
        public async Task<Result<IEnumerable<CloudItemModel>>> GetCloudItemAsync(Guid cloudId, Guid? parrentId, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                List<CloudItem> cloudItems = await context.CloudItems
                    .AsNoTracking()
                    .Where(x => x.PersonalCloudId == cloudId && x.ParrentId == parrentId)
                    .ToListAsync();

                // будем разворачивать недостающие файлы
                IEnumerable<CloudItem> files = cloudItems.Where(x => x.Type == CloudItem.ItemType.File) ?? Array.Empty<CloudItem>();
                if (files.Count() > 0)
                {
                    string targetFolderParrent = Path.Combine(_webHostEnvironment.WebRootPath, MainFolder, appUser.Id, parrentId?.ToString() ?? string.Empty);
                    ExistsOrCreateDirectory(targetFolderParrent);

                    IEnumerable<string> discFiles = Directory.GetFiles(targetFolderParrent)
                        .Select(x => FileExstensionHelper.GetFileName(x));

                    IEnumerable<Guid?> filesNotFoundDisc = files
                        .Where(x => discFiles.Contains(x.SystemName) == false && x.FileDataId != null)
                        .Select(x => x.FileDataId);
                    
                    if (filesNotFoundDisc.Count() > 0)
                    {
                        List<CloudFileData> dbPastFile = await context.CloudFileData
                            .AsNoTracking()
                            .Where(x => filesNotFoundDisc.Contains(x.Id))
                            .Include(x => x.CloudItem)
                            .ToListAsync();
                        foreach (CloudFileData dbFile in dbPastFile)
                        {
                            await File.WriteAllBytesAsync(Path.Combine(targetFolderParrent, dbFile.CloudItem.SystemName), dbFile.Data);
                        }
                    }
                }
                return Result<IEnumerable<CloudItemModel>>.OnSuccess(cloudItems.Select(x => new CloudItemModel()
                {
                    Id = x.Id,
                    Type = (CloudItemModel.ItemType)x.Type,
                    SystemName = x.SystemName,
                    DisplayName = x.DisplayName,
                    Description = x.Description,
                }));
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result<IEnumerable<CloudItemModel>>.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CloudItemModel>>.OnError(ex);
            }
        }
        public async Task<Result> CreateFolderAsync(Guid cloudId, Guid? parrentId, string displayName, string description, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                await context.CloudItems.AddAsync(new CloudItem()
                {
                    Type = CloudItem.ItemType.Folder,
                    Description = description,
                    DisplayName = displayName,
                    SystemName = Guid.NewGuid().ToString("N"),
                    ParrentId = parrentId,
                    PersonalCloudId = cloudId,
                });

                await context.SaveChangesAsync(cancellationToken);

                return Result.OnSuccess();
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result.OnError(ex);
            }
        }
        public async Task<Result> UpdateItemAsync(Guid cloudId, Guid id, Guid? parrentId, string displayName, string description, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                var item = await context.CloudItems.FirstOrDefaultAsync(x => x.Id == id && x.PersonalCloudId == cloudId) 
                    ?? throw new Exception("Item not found");

                if (item.Type == CloudItem.ItemType.Folder)
                {
                    item.ParrentId = parrentId;
                    item.DisplayName = displayName;
                    item.Description = description;
                }
                else if (item.Type == CloudItem.ItemType.File)
                {
                    item.ParrentId = parrentId;
                    var exstensionFile = FileExstensionHelper.GetExtension(item.SystemName);
                    var savedDisplayName = FileExstensionHelper.GetFileNameWithoutExtension(displayName);
                    item.DisplayName = string.IsNullOrEmpty(exstensionFile) ? displayName : savedDisplayName + exstensionFile;
                    item.Description = description;
                }

                await context.SaveChangesAsync(cancellationToken);

                return Result.OnSuccess();
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result.OnError(ex);
            }
        }
        public async Task<Result> DeleteCloudItemAsync(Guid cloudId, Guid id, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                var item = await context.CloudItems.FirstOrDefaultAsync(x => x.Id == id && x.PersonalCloudId == cloudId)
                    ?? throw new Exception("Item not found");

                if (item.FileDataId != null)
                {
                    string targetFile = Path.Combine(
                        _webHostEnvironment.WebRootPath, MainFolder, appUser.Id, item.ParrentId?.ToString() ?? string.Empty, item.SystemName);
                    File.Delete(targetFile);

                    context.CloudFileData.RemoveRange(context.CloudFileData.Where(x => x.Id == item.FileDataId));
                }
                context.CloudItems.Remove(item);

                await context.SaveChangesAsync(cancellationToken);

                return Result.OnSuccess();
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result.OnError(ex);
            }
        }
        public async Task<Result> CreateCloudFilesAsync(Guid cloudId, Guid? parrentId, IEnumerable<IBrowserFile> files, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                string targetFolderParrent = Path.Combine(_webHostEnvironment.WebRootPath, MainFolder, appUser.Id, parrentId?.ToString() ?? string.Empty);
                ExistsOrCreateDirectory(targetFolderParrent);

                FileUploadSettings settings = GetFileUploadSettings().Value!;

                // отсекаем что не подходит по настройкам
                files = files
                    .Where(x => settings.EnableMaxFileSize ? x.Size <= settings.MaxFileSize : true)
                    .Take(settings.EnableMaxAllowedFiles ? settings.MaxAllowedFiles : files.Count());

                if (files.Count() <= 0) 
                    return Result.OnError("Нет файлов для сохранения");

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                foreach (IBrowserFile file in files)
                {
                    string strustedFilePath = GenerateFileName(targetFolderParrent, file.Name, out string trustedFileName);

                    byte[] fileData = await GetFileBytes(file);
                    string md5Hash = ComputeMd5Hash(fileData);
                    string sha256Hash = ComputeSha256Hash(fileData);

                    await using (FileStream fs = new FileStream(strustedFilePath, FileMode.Create))
                    {
                        await fs.WriteAsync(fileData, 0, fileData.Length);
                    }

                    await context.CloudItems.AddRangeAsync(new CloudItem()
                    {
                        ParrentId = parrentId,
                        SystemName = trustedFileName,
                        DisplayName = trustedFileName,
                        PersonalCloudId = cloudId,
                        Description = file.Name,
                        FileData = new CloudFileData()
                        {
                            Data = fileData,
                            Size = fileData.Length,
                            Md5Hash = md5Hash,
                            Sha256Hash = sha256Hash
                        }
                    });
                }

                await context.SaveChangesAsync(cancellationToken);

                return Result.OnSuccess();
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result.OnError(ex);
            }
        }
        public async Task<Result<byte[]>> DownloadCloudFilesAsync(Guid cloudId, Guid? parrentId, string systemName, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                        ?? throw new Exception("User not found");

                string targetFolderParrent = Path.Combine(_webHostEnvironment.WebRootPath, MainFolder, appUser.Id, parrentId?.ToString() ?? string.Empty);
                ExistsOrCreateDirectory(targetFolderParrent);

                string filePath = Path.Combine(targetFolderParrent, systemName);

                if (File.Exists(filePath) == false)
                {
                    await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                    CloudFileData data = await context.CloudFileData
                        .AsNoTracking()
                        .Where(x => x.CloudItem.SystemName == systemName && x.CloudItem.ParrentId == parrentId && x.CloudItem.PersonalCloudId == cloudId)
                        .FirstOrDefaultAsync() ?? throw new Exception("Не найден файл");

                    await using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await fs.WriteAsync(data.Data, 0, data.Data.Length);
                    }

                    return Result<byte[]>.OnSuccess(data.Data);
                }
                else
                {
                    return Result<byte[]>.OnSuccess(await File.ReadAllBytesAsync(filePath));
                }
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result<byte[]>.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result<byte[]>.OnError(ex);
            }
        }

        private static async Task<byte[]> GetFileBytes(IBrowserFile file)
        {
            await using (MemoryStream ms = new MemoryStream())
            {
                await file.OpenReadStream(long.MaxValue).CopyToAsync(ms);
                return ms.ToArray();
            }
        }
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
    }
}
