using BlazorApp.Client.Interfaces;
using BlazorApp.Client.Models;
using BlazorApp.Client.Settings;
using BlazorApp.Helpers;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BlazorApp.Services
{
    internal class FileUploadService : IFileUploadService
    {
        protected readonly FileHelper _fileHelper;
        protected readonly IOptionsMonitor<FileUploadSettings> _options;
        protected readonly ILogger<FileUploadService> _logger;

        public List<IBrowserFile> SelectedFiles { get; private set; } = [];
        public List<string> ErrorFiles { get; private set; } = [];
        public string ErrorMessage { get; private set; } = string.Empty;
        public FileUploadSettings Settings => _options.CurrentValue;

        public FileUploadService(FileHelper fileHelper, IOptionsMonitor<FileUploadSettings> options, ILogger<FileUploadService> logger)
        {
            _fileHelper = fileHelper;
            _options = options;
            _logger = logger;
        }

        public async Task OnInitializedAsync(CancellationToken cancellationToken = default)
        {

        }

        public void LoadFiles(in InputFileChangeEventArgs e)
        {
            try
            {
                SelectedFiles.Clear();
                ErrorFiles.Clear();

                if (Settings.EnableMaxAllowedFiles ? e.FileCount <= Settings.MaxAllowedFiles : true)
                    SelectedFiles.AddRange(e.GetMultipleFiles(Settings.EnableMaxAllowedFiles ? Settings.MaxAllowedFiles : e.FileCount)
                        .Where(x => Settings.EnableMaxAllowedFiles ? x.Size <= Settings.MaxFileSize : true && string.IsNullOrEmpty(x.ContentType) == false));
                else
                    ErrorMessage = $"Ошибка получения файлов, выбрано больше чем разрешено";
            }
            catch (Exception ex)
            {
                SelectedFiles.Clear();
                ErrorFiles.Clear();
                ErrorMessage = $"Произошла ошибка, попробуйте ещё раз";
            }
        }
        public async Task SendFilesAsync(CancellationToken cancellationToken = default)
        {
            if (SelectedFiles.Count <= 0) return;

            Result<List<FileUploadResult>> result = await _fileHelper.SaveFiles(SelectedFiles);

            ErrorFiles.Clear();
            ErrorFiles.AddRange(result.Value?.Where(x => x.IsSave == false).Select(x => x.Name) ?? Array.Empty<string>());

            SelectedFiles.Clear();
        }

        public void RemoveItem(IBrowserFile file)
        {
            SelectedFiles.Remove(file);
        }
        public void ClearSelected()
        {
            SelectedFiles.Clear();
            ErrorFiles.Clear();
            ErrorMessage = string.Empty;
        }

        // Флаг для отслеживания состояния
        private bool _isDisposed;
        public async ValueTask DisposeAsync()
        {
            if (_isDisposed) return;

            _isDisposed = true;

            SelectedFiles.Clear();
            ErrorFiles.Clear();
            ErrorMessage = null;
        }
    }
}
