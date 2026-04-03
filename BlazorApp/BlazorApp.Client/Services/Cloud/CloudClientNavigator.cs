using BlazorApp.Client.Models;
using BlazorApp.Client.Models.Cloud;
using BlazorApp.Client.Settings;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Claims;

namespace BlazorApp.Client.Services.Cloud
{
    public interface ICloudNavigator
    {
        Task<Result> CreateCloudAsync(CreateCloudModel model, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<UploadFileResult>>> CreateCloudFilesAsync(Guid cloudId, Guid? parrentId, IEnumerable<IBrowserFile> files, CancellationToken cancellationToken = default);
        Task<Result> CreateFolderAsync(Guid cloudId, Guid? parrentId, CreateCloudFolderModel model, CancellationToken cancellationToken = default);
        Task<Result> DeleteCloudAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result> DeleteCloudItemAsync(Guid cloudId, Guid id, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<CloudItemModel>>> GetCloudItemAsync(Guid cloudId, Guid? parrentId, CancellationToken cancellationToken = default);
        ValueTask<Result<CloudSettings>> GetCloudSettings(CancellationToken cancellationToken = default);
        ValueTask<Result<FileUploadSettings>> GetFileUploadSettings(CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<CloudModel>>> GetMainPageAsync(CancellationToken cancellationToken = default);
        Task<Result<CloudModel>> GetPersonalCloudAsync(Guid cloudId, CancellationToken cancellationToken = default);
        Task<Result> UpdateCloudAsync(EditCloudModel model, CancellationToken cancellationToken = default);
        Task<Result> UpdateItemAsync(Guid cloudId, Guid? parrentId, EditCloudItemModel model, CancellationToken cancellationToken = default);
    }
    public class CloudClientNavigator : ICloudNavigator
    {
        public Task<Result<IEnumerable<CloudModel>>> GetMainPageAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> CreateCloudAsync(CreateCloudModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteCloudAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateCloudAsync(EditCloudModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<CloudItemModel>>> GetCloudItemAsync(Guid cloudId, Guid? parrentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> CreateFolderAsync(Guid cloudId, Guid? parrentId, CreateCloudFolderModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteCloudItemAsync(Guid cloudId, Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateItemAsync(Guid cloudId, Guid? parrentId, EditCloudItemModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<UploadFileResult>>> CreateCloudFilesAsync(Guid cloudId, Guid? parrentId, IEnumerable<IBrowserFile> files, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result<FileUploadSettings>> GetFileUploadSettings(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result<CloudSettings>> GetCloudSettings(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<CloudModel>> GetPersonalCloudAsync(Guid cloudId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
