using BlazorApp.Client.Models;

namespace BlazorApp.Client.Interfaces
{
    public interface IFileListService
    {
        string ErrorMessage { get; }

        Task<Result?> DeleteFile(string filePath, CancellationToken cancellationToken = default);
        Task<IEnumerable<FileItem>?> GetFiles(CancellationToken cancellationToken = default);

    }
}
