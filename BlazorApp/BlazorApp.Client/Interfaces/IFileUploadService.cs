using BlazorApp.Client.Settings;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorApp.Client.Interfaces
{
    public interface IFileUploadService : IAsyncDisposable
    {
        List<IBrowserFile> SelectedFiles { get; }
        List<string> ErrorFiles { get; }
        string ErrorMessage { get; }
        FileUploadSettings Settings { get; }

        void ClearSelected();
        void LoadFiles(in InputFileChangeEventArgs e);
        Task OnInitializedAsync(CancellationToken cancellationToken = default);
        void RemoveItem(IBrowserFile file);
        Task SendFilesAsync(CancellationToken cancellationToken = default);
    }
}
