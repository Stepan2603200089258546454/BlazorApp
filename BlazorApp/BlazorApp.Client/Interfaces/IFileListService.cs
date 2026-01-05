using BlazorApp.Client.Models;
using BlazorApp.Client.Models.Request;

namespace BlazorApp.Client.Interfaces
{
    public interface IFileListService
    {
        string ErrorMessage { get; }
        FileGetRequest GetRequest { get; }

        Task<Result?> DeleteFile(string filePath, CancellationToken cancellationToken = default);
        Task<IEnumerable<FileItem>?> GetFiles(CancellationToken cancellationToken = default);

    }
}
