using BlazorApp.Client.Models;
using BlazorApp.Client.Models.Cloud;
using BlazorApp.Client.Services.Cloud;
using BlazorApp.Client.Settings;
using DataContext.Context;
using DataContext.Models;
using DataContext.Models.Cloud;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorApp.Services.Cloud
{
    public class CloudServerNavigator : ICloudNavigator
    {
        protected readonly CloudProvider _cloudProvider;
        protected readonly AuthenticationStateProvider _authStateProvider;

        public CloudServerNavigator(
            CloudProvider cloudProvider,
            AuthenticationStateProvider authStateProvider)
        {
            _cloudProvider = cloudProvider;
            _authStateProvider = authStateProvider;
        }

        private async Task<ClaimsPrincipal> GetUserAsync()
        {
            AuthenticationState authState = await _authStateProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal user = authState.User;
            return user;
        }

        public async Task<Result<IEnumerable<CloudModel>>> GetMainPageAsync(CancellationToken cancellationToken = default)
        {
            return await _cloudProvider.GetMainPageAsync(await GetUserAsync(), cancellationToken);
        }
        public async Task<Result<IEnumerable<CloudModel>>> GetGlobalPageAsync(CancellationToken cancellationToken = default)
        {
            return await _cloudProvider.GetGlobalPageAsync(await GetUserAsync(), cancellationToken);
        }
        public async Task<Result<CloudModel>> GetUserCloudAsync(Guid cloudId, CancellationToken cancellationToken = default)
        {
            return await _cloudProvider.GetUserCloudAsync(cloudId, await GetUserAsync(), cancellationToken);
        }
        public async Task<Result> CreateCloudAsync(CreateCloudModel model, CancellationToken cancellationToken = default)
        {
            return await _cloudProvider.CreateCloudAsync(model.DisplayName, model.Description, model.IsPersonal, await GetUserAsync(), cancellationToken);
        }
        public async Task<Result> UpdateCloudAsync(EditCloudModel model, CancellationToken cancellationToken = default)
        {
            return await _cloudProvider.UpdateCloudAsync(model.Id, model.DisplayName, model.Description, model.IsPersonal, await GetUserAsync(), cancellationToken);
        }
        public async Task<Result> DeleteCloudAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _cloudProvider.DeleteCloudAsync(id, await GetUserAsync(), cancellationToken);
        }


        public async ValueTask<Result<FileUploadSettings>> GetFileUploadSettings(CancellationToken cancellationToken = default)
        {
            return _cloudProvider.GetFileUploadSettings();
        }
        public async ValueTask<Result<CloudSettings>> GetCloudSettings(CancellationToken cancellationToken = default)
        {
            return _cloudProvider.GetCloudSettings();
        }
        public async Task<Result<IEnumerable<CloudItemModel>>> GetCloudItemAsync(Guid cloudId, Guid? parrentId, bool isPersonal, CancellationToken cancellationToken = default)
        {
            return await _cloudProvider.GetCloudItemAsync(cloudId, parrentId, isPersonal, await GetUserAsync(), cancellationToken);
        }
        public async Task<Result> CreateFolderAsync(Guid cloudId, Guid? parrentId, CreateCloudFolderModel model, bool isPersonal, CancellationToken cancellationToken = default)
        {
            return await _cloudProvider.CreateFolderAsync(cloudId, parrentId, model.DisplayName, model.Description, isPersonal, await GetUserAsync(), cancellationToken);
        }

        public async Task<Result> UpdateItemAsync(Guid cloudId, Guid? parrentId, EditCloudItemModel model, CancellationToken cancellationToken = default)
        {
            return await _cloudProvider.UpdateItemAsync(cloudId, model.Id, parrentId, model.DisplayName, model.Description, await GetUserAsync(), cancellationToken);
        }
        public async Task<Result> DeleteCloudItemAsync(Guid cloudId, Guid id, CancellationToken cancellationToken = default)
        {
            return await _cloudProvider.DeleteCloudItemAsync(cloudId, id, await GetUserAsync(), cancellationToken);
        }

        public async Task<Result<IEnumerable<UploadFileResult>>> CreateCloudFilesAsync(Guid cloudId, Guid? parrentId, IEnumerable<IBrowserFile> files, bool isPersonal, CancellationToken cancellationToken = default)
        {
            return await _cloudProvider.CreateCloudFilesAsync(cloudId, parrentId, files.ToList(), isPersonal, await GetUserAsync(), cancellationToken);
        }
    }
}
