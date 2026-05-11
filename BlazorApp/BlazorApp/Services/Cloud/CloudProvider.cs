using BlazorApp.Client.Models;
using BlazorApp.Client.Models.Cloud;
using BlazorApp.Client.Pages;
using BlazorApp.Client.Settings;
using BlazorApp.Models.Clouds;
using DataContext.Context;
using DataContext.Models;
using DataContext.Models.Cloud;
using FileExstend;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
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
        protected readonly IOptionsMonitor<CloudSettings> _optionsMonitorCloudSettings;
        public const string MainFolder = "Cloudes";

        public CloudProvider(
            IWebHostEnvironment webHostEnvironment,
            IDbContextFactory<DBContext> dbContextFactory,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<FileUploadSettings> optionsMonitorFileUploadSettings,
            IOptionsMonitor<CloudSettings> optionsMonitorCloudSettings)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContextFactory = dbContextFactory;
            _userManager = userManager;
            _optionsMonitorFileUploadSettings = optionsMonitorFileUploadSettings;
            _optionsMonitorCloudSettings = optionsMonitorCloudSettings;
        }

        private static void ExistsOrCreateDirectory(string path)
        {
            if (Directory.Exists(path) == false) Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Получить список дисков пользователя
        /// </summary>
        public async Task<Result<IEnumerable<CloudModel>>> GetMainPageAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                string pathMainFolder = Path.Combine(_webHostEnvironment.WebRootPath, MainFolder);
                ExistsOrCreateDirectory(pathMainFolder);

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                List<CloudModel> allClouds = await context.PersonalClouds
                    .Where(x => x.UserId == appUser.Id)
                    .Select(x => new CloudModel
                    {
                        Id = x.Id,
                        IsPersonal = true,
                        SystemName = x.SystemName,
                        DisplayName = x.DisplayName,
                        Description = x.Description,
                        Size = context.CloudFileData
                            .Where(t => t.CloudItem.PersonalCloudId == x.Id)
                            .Sum(t => t.Size),
                        UserEmail = x.User.Email ?? string.Empty,
                    })
                    .Concat(context.GlobalClouds
                        .Where(x => x.UserId == appUser.Id)
                        .Select(x => new CloudModel
                        {
                            Id = x.Id,
                            IsPersonal = false,
                            SystemName = x.SystemName,
                            DisplayName = x.DisplayName,
                            Description = x.Description,
                            Size = context.CloudFileData
                                .Where(t => t.CloudItem.GlobalCloudId == x.Id)
                                .Sum(t => t.Size),
                            UserEmail = x.User.Email ?? string.Empty,
                        }))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return Result<IEnumerable<CloudModel>>.OnSuccess(allClouds);
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
        /// <summary>
        /// Получить список дисков других пользователей
        /// </summary>
        public async Task<Result<IEnumerable<CloudModel>>> GetGlobalPageAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                string pathMainFolder = Path.Combine(_webHostEnvironment.WebRootPath, MainFolder);
                ExistsOrCreateDirectory(pathMainFolder);

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                List<CloudModel> allClouds = await context.GlobalClouds
                    .AsNoTracking()
                    .Where(x => x.UserId != appUser.Id)
                    .Select(x => new CloudModel
                    {
                        Id = x.Id,
                        IsPersonal = false,
                        SystemName = x.SystemName,
                        DisplayName = x.DisplayName,
                        Description = x.Description,
                        Size = context.CloudFileData
                            .Where(t => t.CloudItem.GlobalCloudId == x.Id)
                            .Sum(t => t.Size),
                        UserEmail = x.User.Email ?? string.Empty,
                    })
                    .ToListAsync(cancellationToken);

                return Result<IEnumerable<CloudModel>>.OnSuccess(allClouds);
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
        /// <summary>
        /// Получить информацию о диске пользователя
        /// </summary>
        public async Task<Result<CloudModel>> GetUserCloudAsync(Guid cloudId, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                CloudModel? cloud =

                    await context.PersonalClouds
                    .AsNoTracking()
                    .Where(x => x.UserId == appUser.Id && x.Id == cloudId)
                    .Select(x => new CloudModel()
                    {
                        Id = x.Id,
                        IsPersonal = true,
                        SystemName = x.SystemName,
                        DisplayName = x.DisplayName,
                        Description = x.Description,
                        Size = context.CloudFileData
                            .Where(t => t.CloudItem.PersonalCloudId == x.Id)
                            .Sum(t => t.Size),
                        UserEmail = x.User.Email ?? string.Empty,
                    })
                    .FirstOrDefaultAsync()

                    ??

                    await context.GlobalClouds
                    .AsNoTracking()
                    .Where(x => x.Id == cloudId)
                    .Select(x => new CloudModel()
                    {
                        Id = x.Id,
                        IsPersonal = true,
                        SystemName = x.SystemName,
                        DisplayName = x.DisplayName,
                        Description = x.Description,
                        Size = context.CloudFileData
                            .Where(t => t.CloudItem.GlobalCloudId == x.Id)
                            .Sum(t => t.Size),
                        UserEmail = x.User.Email ?? string.Empty,
                    })
                    .FirstOrDefaultAsync() ?? throw new Exception("Cloud not found");

                return Result<CloudModel>.OnSuccess(cloud);
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result<CloudModel>.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result<CloudModel>.OnError(ex);
            }
        }
        /// <summary>
        /// Создать диск у пользователя
        /// </summary>
        public async Task<Result> CreateCloudAsync(string displayName, string description, bool isPersonal, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                CloudSettings cloudSettings = GetCloudSettings().Value!;

                if (isPersonal)
                {
                    if (cloudSettings.EnableMaxPersonalClouds)
                    {
                        int currentCountCloudPerson = await context.PersonalClouds
                            .Where(x => x.UserId == appUser.Id)
                            .CountAsync();

                        if (currentCountCloudPerson + 1 > cloudSettings.MaxPersonalClouds)
                            return Result.OnError("Превышено кол-во дисков");
                    }

                    await context.PersonalClouds.AddAsync(new PersonalCloud()
                    {
                        SystemName = Guid.NewGuid().ToString("N"),
                        DisplayName = displayName,
                        Description = description,
                        UserId = appUser.Id,
                    });
                    await context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    if (cloudSettings.EnableMaxGlobalClouds)
                    {
                        int currentCountCloudPerson = await context.GlobalClouds
                            .Where(x => x.UserId == appUser.Id)
                            .CountAsync();

                        if (currentCountCloudPerson + 1 > cloudSettings.MaxGlobalClouds)
                            return Result.OnError("Превышено кол-во общедоступных дисков");
                    }

                    await context.GlobalClouds.AddAsync(new GlobalCloud()
                    {
                        SystemName = Guid.NewGuid().ToString("N"),
                        DisplayName = displayName,
                        Description = description,
                        UserId = appUser.Id,
                    });
                    await context.SaveChangesAsync(cancellationToken);
                }
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
        /// <summary>
        /// Обновить диск у пользователя
        /// </summary>
        public async Task<Result> UpdateCloudAsync(Guid id, string displayName, string description, bool isPersonal, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                if (isPersonal)
                {
                    PersonalCloud cloud = await context.PersonalClouds.FirstOrDefaultAsync(c => c.Id == id && c.UserId == appUser.Id)
                        ?? throw new Exception("Cloud not found");

                    cloud.DisplayName = displayName;
                    cloud.Description = description;

                    await context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    GlobalCloud cloud = await context.GlobalClouds.FirstOrDefaultAsync(c => c.Id == id && c.UserId == appUser.Id)
                        ?? throw new Exception("Cloud not found");

                    cloud.DisplayName = displayName;
                    cloud.Description = description;

                    await context.SaveChangesAsync(cancellationToken);
                }

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
        /// <summary>
        /// Удалить диск у пользователя
        /// </summary>
        public async Task<Result> DeleteCloudAsync(Guid id, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                PersonalCloud? cloud = await context.PersonalClouds.FirstOrDefaultAsync(c => c.Id == id && c.UserId == appUser.Id);
                if (cloud != null)
                {
                    context.PersonalClouds.Remove(cloud);

                    await context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    GlobalCloud? gcloud = await context.GlobalClouds.FirstOrDefaultAsync(c => c.Id == id && c.UserId == appUser.Id);

                    if (gcloud != null)
                    {
                        context.GlobalClouds.Remove(gcloud);

                        await context.SaveChangesAsync(cancellationToken);
                    }
                    else
                        throw new Exception("Cloud not found");
                }

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
        public Result<CloudSettings> GetCloudSettings()
        {
            return Result<CloudSettings>.OnSuccess(_optionsMonitorCloudSettings.CurrentValue ?? CloudSettings.Empty);
        }


        public async Task<Result<IEnumerable<CloudItemModel>>> GetCloudItemAsync(Guid cloudId, Guid? parrentId, bool isPersonal, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            if (isPersonal)
                return await GetPersonalCloudItemAsync(cloudId, parrentId, user, cancellationToken);
            else
                return await GetGlobalCloudItemAsync(cloudId, parrentId, user, cancellationToken);
        }
        private async Task<Result<IEnumerable<CloudItemModel>>> GetPersonalCloudItemAsync(Guid cloudId, Guid? parrentId, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

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

                var sizeMap = await context.CloudFileData
                    .Where(x => cloudItems.Select(s => s.Id).Any(a => a == x.CloudItem.Id))
                    .Select(x => new
                    {
                        x.CloudItem.Id,
                        x.Size,
                    })
                    .ToListAsync();

                return Result<IEnumerable<CloudItemModel>>.OnSuccess(cloudItems.Select(x => new CloudItemModel()
                {
                    Id = x.Id,
                    Type = (CloudItemModel.ItemType)x.Type,
                    SystemName = x.SystemName,
                    DisplayName = x.DisplayName,
                    Description = x.Description,
                    Size = sizeMap.FirstOrDefault(s => s.Id == x.Id)?.Size,
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
        private async Task<Result<IEnumerable<CloudItemModel>>> GetGlobalCloudItemAsync(Guid cloudId, Guid? parrentId, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                List<CloudItem> cloudItems = await context.CloudItems
                    .AsNoTracking()
                    .Where(x => x.GlobalCloudId == cloudId && x.ParrentId == parrentId)
                    .Include(x => x.GlobalCloud)
                    .Include(x => x.User)
                    .ToListAsync();

                // будем разворачивать недостающие файлы
                IEnumerable<CloudItem> files = cloudItems.Where(x => x.Type == CloudItem.ItemType.File) ?? Array.Empty<CloudItem>();
                if (files.Count() > 0)
                {
                    foreach (CloudItem file in files)
                    {
                        string targetFolderParrent = Path.Combine(_webHostEnvironment.WebRootPath, MainFolder, file.GlobalCloud!.UserId, parrentId?.ToString() ?? string.Empty);
                        ExistsOrCreateDirectory(targetFolderParrent);

                        string targetFilePath = Path.Combine(targetFolderParrent, file.SystemName);
                        if (File.Exists(targetFilePath) == false)
                        {
                            CloudFileData? dbPastFile = await context.CloudFileData
                                .AsNoTracking()
                                .Where(x => x.Id == file.FileDataId)
                                .FirstOrDefaultAsync();
                            if (dbPastFile != null)
                            {
                                await File.WriteAllBytesAsync(targetFilePath, dbPastFile.Data);
                            }
                        }
                    }
                }

                var sizeMap = await context.CloudFileData
                    .Where(x => cloudItems.Select(s => s.Id).Any(a => a == x.CloudItem.Id))
                    .Select(x => new
                    {
                        x.CloudItem.Id,
                        x.Size,
                    })
                    .ToListAsync();

                return Result<IEnumerable<CloudItemModel>>.OnSuccess(cloudItems.Select(x => new CloudItemModel()
                {
                    Id = x.Id,
                    UserEmail = x.User.Email ?? string.Empty,
                    Type = (CloudItemModel.ItemType)x.Type,
                    SystemName = x.SystemName,
                    DisplayName = x.DisplayName,
                    Description = x.Description,
                    Size = sizeMap.FirstOrDefault(s => s.Id == x.Id)?.Size,
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
        public async Task<Result> CreateFolderAsync(Guid cloudId, Guid? parrentId, string displayName, string description, bool isPersonal, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                if (isPersonal)
                {
                    await context.CloudItems.AddAsync(new CloudItem()
                    {
                        Type = CloudItem.ItemType.Folder,
                        Description = description,
                        DisplayName = displayName,
                        SystemName = Guid.NewGuid().ToString("N"),
                        ParrentId = parrentId,
                        PersonalCloudId = cloudId,
                        UserId = appUser.Id,
                    });
                }
                else
                {
                    await context.CloudItems.AddAsync(new CloudItem()
                    {
                        Type = CloudItem.ItemType.Folder,
                        Description = description,
                        DisplayName = displayName,
                        SystemName = Guid.NewGuid().ToString("N"),
                        ParrentId = parrentId,
                        GlobalCloudId = cloudId,
                        UserId = appUser.Id,
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
        public async Task<Result> UpdateItemAsync(Guid cloudId, Guid id, Guid? parrentId, string displayName, string description, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                CloudItem item = await context.CloudItems
                    .Where(x => x.Id == id &&
                        x.PersonalCloudId == cloudId &&
                        x.UserId == appUser.Id)
                    .FirstOrDefaultAsync()

                    ??

                    await context.CloudItems
                    .Where(x => x.Id == id &&
                        x.GlobalCloud.Id == cloudId &&
                        (x.GlobalCloud.UserId == appUser.Id || x.UserId == appUser.Id))
                    .FirstOrDefaultAsync()

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

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                CloudItem item = await context.CloudItems
                    .Where(x => x.Id == id &&
                        x.PersonalCloudId == cloudId &&
                        x.UserId == appUser.Id)
                    .FirstOrDefaultAsync()

                    ??

                    await context.CloudItems
                    .Where(x => x.Id == id &&
                        x.GlobalCloud.Id == cloudId &&
                        (x.GlobalCloud.UserId == appUser.Id || x.UserId == appUser.Id))
                    .FirstOrDefaultAsync()

                    ?? throw new Exception("Item not found");

                if (item.FileDataId != null)
                {
                    string targetFile = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        MainFolder,
                        item.GlobalCloud?.UserId ?? appUser.Id,
                        item.ParrentId?.ToString() ?? string.Empty,
                        item.SystemName);
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
        public async Task<Result<IEnumerable<UploadFileResult>>> CreateCloudFilesAsync(Guid cloudId, Guid? parrentId, List<IBrowserFile> files, bool isPersonal, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            if (isPersonal)
                return await CreatePersonalCloudFilesAsync(cloudId, parrentId, files, user, cancellationToken);
            else
                return await CreateGlobalCloudFilesAsync(cloudId, parrentId, files, user, cancellationToken);
        }
        private async Task<Result<IEnumerable<UploadFileResult>>> CreatePersonalCloudFilesAsync(Guid cloudId, Guid? parrentId, List<IBrowserFile> files, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                string targetFolderParrent = Path.Combine(_webHostEnvironment.WebRootPath, MainFolder, appUser.Id, parrentId?.ToString() ?? string.Empty);
                ExistsOrCreateDirectory(targetFolderParrent);

                FileUploadSettings settings = GetFileUploadSettings().Value!;
                CloudSettings cloudSettings = GetCloudSettings().Value!;

                List<UploadFileResult> results = new List<UploadFileResult>();

                // отсекаем что не подходит по настройкам

                IEnumerable<IBrowserFile> errorFiles = files.Where(x => settings.EnableMaxFileSize ? x.Size >= settings.MaxFileSize : false);
                results.AddRange(errorFiles.Select(x => new UploadFileResult()
                {
                    Success = false,
                    FileName = x.Name,
                    Message = "Превышен размер загружаемого файла"
                }));
                foreach (var errorFile in errorFiles)
                    files.Remove(errorFile);

                errorFiles = files.Skip(settings.EnableMaxAllowedFiles ? settings.MaxAllowedFiles : files.Count());
                results.AddRange(errorFiles.Select(x => new UploadFileResult()
                {
                    Success = false,
                    FileName = x.Name,
                    Message = "Превышено кол-во загружаемых файлов"
                }));
                foreach (var errorFile in errorFiles)
                    files.Remove(errorFile);

                if (files.Count() <= 0)
                    return Result<IEnumerable<UploadFileResult>>.OnSuccess(results);

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                long cloudSize = cloudSettings.EnableMaxPersonalCloudSize ? await context.CloudFileData
                    .AsNoTracking()
                    .Where(x => x.CloudItem.PersonalCloudId == cloudId)
                    .SumAsync(x => x.Size) : 0;

                foreach (IBrowserFile file in files)
                {
                    if (cloudSettings.EnableMaxPersonalCloudSize)
                    {
                        if (cloudSize + file.Size <= cloudSettings.MaxPersonalCloudSize)
                        {
                            cloudSize += file.Size;
                        }
                        else
                        {
                            results.Add(new UploadFileResult()
                            {
                                Success = false,
                                FileName = file.Name,
                                Message = "Превышен допустимый размер диска"
                            });
                            continue;
                        }
                    }

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
                        UserId = appUser.Id,
                        SystemName = trustedFileName,
                        DisplayName = file.Name,//trustedFileName,
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

                    results.Add(new UploadFileResult()
                    {
                        Success = true,
                        FileName = file.Name,
                        Message = "Успешно загружен"
                    });
                }

                await context.SaveChangesAsync(cancellationToken);

                return Result<IEnumerable<UploadFileResult>>.OnSuccess(results);
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result<IEnumerable<UploadFileResult>>.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<UploadFileResult>>.OnError(ex);
            }
        }
        private async Task<Result<IEnumerable<UploadFileResult>>> CreateGlobalCloudFilesAsync(Guid cloudId, Guid? parrentId, List<IBrowserFile> files, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                    ?? throw new Exception("User not found");

                FileUploadSettings settings = GetFileUploadSettings().Value!;
                CloudSettings cloudSettings = GetCloudSettings().Value!;

                List<UploadFileResult> results = new List<UploadFileResult>();

                // отсекаем что не подходит по настройкам

                IEnumerable<IBrowserFile> errorFiles = files.Where(x => settings.EnableMaxFileSize ? x.Size >= settings.MaxFileSize : false);
                results.AddRange(errorFiles.Select(x => new UploadFileResult()
                {
                    Success = false,
                    FileName = x.Name,
                    Message = "Превышен размер загружаемого файла"
                }));
                foreach (var errorFile in errorFiles)
                    files.Remove(errorFile);

                errorFiles = files.Skip(settings.EnableMaxAllowedFiles ? settings.MaxAllowedFiles : files.Count());
                results.AddRange(errorFiles.Select(x => new UploadFileResult()
                {
                    Success = false,
                    FileName = x.Name,
                    Message = "Превышено кол-во загружаемых файлов"
                }));
                foreach (var errorFile in errorFiles)
                    files.Remove(errorFile);

                if (files.Count() <= 0)
                    return Result<IEnumerable<UploadFileResult>>.OnSuccess(results);

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                long cloudSize = cloudSettings.EnableMaxGlobalCloudSize ? await context.CloudFileData
                    .AsNoTracking()
                    .Where(x => x.CloudItem.GlobalCloudId == cloudId)
                    .SumAsync(x => x.Size) : 0;

                GlobalCloud globalCloud = await context.GlobalClouds
                    .AsNoTracking()
                    .Where(x => x.Id == cloudId)
                    .FirstOrDefaultAsync() ?? throw new Exception("Cloud not found");

                string targetFolderParrent = Path.Combine(_webHostEnvironment.WebRootPath, MainFolder, globalCloud.UserId, parrentId?.ToString() ?? string.Empty);
                ExistsOrCreateDirectory(targetFolderParrent);

                foreach (IBrowserFile file in files)
                {
                    if (cloudSettings.EnableMaxGlobalCloudSize)
                    {
                        if (cloudSize + file.Size <= cloudSettings.MaxGlobalCloudSize)
                        {
                            cloudSize += file.Size;
                        }
                        else
                        {
                            results.Add(new UploadFileResult()
                            {
                                Success = false,
                                FileName = file.Name,
                                Message = "Превышен допустимый размер диска"
                            });
                            continue;
                        }
                    }

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
                        DisplayName = file.Name,//trustedFileName,
                        GlobalCloudId = cloudId,
                        Description = file.Name,
                        UserId = appUser.Id,
                        FileData = new CloudFileData()
                        {
                            Data = fileData,
                            Size = fileData.Length,
                            Md5Hash = md5Hash,
                            Sha256Hash = sha256Hash
                        }
                    });

                    results.Add(new UploadFileResult()
                    {
                        Success = true,
                        FileName = file.Name,
                        Message = "Успешно загружен"
                    });
                }

                await context.SaveChangesAsync(cancellationToken);

                return Result<IEnumerable<UploadFileResult>>.OnSuccess(results);
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result<IEnumerable<UploadFileResult>>.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<UploadFileResult>>.OnError(ex);
            }
        }
        public async Task<Result<byte[]>> DownloadCloudFilesAsync(Guid cloudId, Guid? parrentId, string systemName, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                        ?? throw new Exception("User not found");

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                string userCloudId = await context.PersonalClouds
                    .Where(x => x.Id == cloudId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync()

                    ??

                    await context.GlobalClouds
                    .Where(x => x.Id == cloudId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync()

                    ?? throw new Exception("Cloud not found");

                string targetFolderParrent = Path.Combine(_webHostEnvironment.WebRootPath, MainFolder, userCloudId, parrentId?.ToString() ?? string.Empty);
                ExistsOrCreateDirectory(targetFolderParrent);

                string filePath = Path.Combine(targetFolderParrent, systemName);

                if (File.Exists(filePath) == false)
                {
                    CloudFileData data = await context.CloudFileData
                        .AsNoTracking()
                        .Where(x =>
                            x.CloudItem.SystemName == systemName &&
                            x.CloudItem.ParrentId == parrentId &&
                            (x.CloudItem.PersonalCloudId == cloudId || x.CloudItem.GlobalCloudId == cloudId))
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
        public async Task<Result<FileStreamContent>> ViewCloudFilesAsync(Guid cloudId, Guid? parrentId, string systemName, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(user)
                        ?? throw new Exception("User not found");

                await using DBContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                string userCloudId = await context.PersonalClouds
                    .Where(x => x.Id == cloudId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync()

                    ??

                    await context.GlobalClouds
                    .Where(x => x.Id == cloudId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync()

                    ?? throw new Exception("Cloud not found");

                string targetFolderParrent = Path.Combine(_webHostEnvironment.WebRootPath, MainFolder, userCloudId, parrentId?.ToString() ?? string.Empty);
                ExistsOrCreateDirectory(targetFolderParrent);

                string filePath = Path.Combine(targetFolderParrent, systemName);

                if (File.Exists(filePath) == false)
                {
                    CloudFileData data = await context.CloudFileData
                        .AsNoTracking()
                        .Where(x =>
                            x.CloudItem.SystemName == systemName &&
                            x.CloudItem.ParrentId == parrentId &&
                            (x.CloudItem.PersonalCloudId == cloudId || x.CloudItem.GlobalCloudId == cloudId))
                        .FirstOrDefaultAsync() ?? throw new Exception("Не найден файл");
                    await using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await fs.WriteAsync(data.Data, 0, data.Data.Length);
                    }
                }

                // Определяем MIME тип автоматически
                if (new FileExtensionContentTypeProvider().TryGetContentType(filePath, out string? contentType) == false)
                    contentType = "application/octet-stream";

                // Открываем поток файла для эффективной передачи
                FileStream fileStream = File.OpenRead(filePath);

                return Result<FileStreamContent>.OnSuccess(new FileStreamContent()
                {
                    FileStream = fileStream,
                    ContentType = contentType
                });
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return Result<FileStreamContent>.OnCanceled(ex);
            }
            catch (Exception ex)
            {
                return Result<FileStreamContent>.OnError(ex);
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
