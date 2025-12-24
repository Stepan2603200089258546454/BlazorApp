using BlazorApp.Client.Interfaces;
using BlazorApp.Client.Models;
using BlazorApp.Helpers;

namespace BlazorApp.Services
{
    internal class FileListService : IFileListService
    {
        protected readonly FileHelper _fileHelper;
        public string ErrorMessage { get; private set; } = string.Empty;

        public FileListService(FileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }


        public async Task<IEnumerable<FileItem>?> GetFiles(CancellationToken cancellationToken = default)
        {
            return _fileHelper.GetFiles();
        }

        public async Task<Result?> DeleteFile(string filePath, CancellationToken cancellationToken = default)
        {
            return _fileHelper.Delete(filePath);
        }
    }
}
