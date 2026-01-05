using BlazorApp.Client.Interfaces;
using BlazorApp.Client.Models;
using BlazorApp.Client.Settings;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Routing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp.Client.Services
{
    internal class FileUploadService : IFileUploadService
    {
        protected readonly HttpClient _client;

        public List<IBrowserFile> SelectedFiles { get; private set; } = [];
        public List<string> ErrorFiles { get; private set; } = [];
        public string ErrorMessage { get; private set; } = string.Empty;
        public FileUploadSettings Settings { get; private set; } = FileUploadSettings.Empty;

        public FileUploadService(HttpClient client)
        {
            _client = client;
        }

        public async Task OnInitializedAsync(CancellationToken cancellationToken = default)
        {
            Settings = await GetSettings(cancellationToken) ?? FileUploadSettings.Empty;
        }
        protected async Task<FileUploadSettings?> GetSettings(CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _client.GetAsync(Files.API.GetSettings, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                FileUploadSettings? result = await JsonSerializer.DeserializeAsync<FileUploadSettings>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                }, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                ErrorMessage = "Отмена операции получения настроек";
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Произошла ошибка получения настроек";
                return null;
            }
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
        private async Task<IEnumerable<FileUploadResult>?> SendMessage(CancellationToken cancellationToken = default)
        {
            try
            {
                bool upload = false;

                using MultipartFormDataContent content = new MultipartFormDataContent();
                foreach (IBrowserFile file in SelectedFiles)
                {
                    try
                    {
                        StreamContent fileContent = new StreamContent(file.OpenReadStream(file.Size, cancellationToken));

                        cancellationToken.ThrowIfCancellationRequested();

                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

                        content.Add(
                            content: fileContent,
                            name: "\"files\"",
                            fileName: file.Name);

                        upload = true;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                cancellationToken.ThrowIfCancellationRequested();
                if (upload == false)
                {
                    ErrorMessage = "Произошла ошибка добавления файлов";
                    return null;
                }

                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Files.API.Upload);
                request.SetBrowserRequestStreamingEnabled(true);
                request.Content = content;

                using HttpResponseMessage response = await _client.SendAsync(request, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if (response.IsSuccessStatusCode == false)
                {
                    ErrorMessage = "Произошла ошибка сохранения файлов";
                    return null;
                }

                using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await JsonSerializer.DeserializeAsync<IEnumerable<FileUploadResult>>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                }, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                ErrorMessage = "Операция отменена";
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Произошла ошибка отправки файлов";
                return null;
            }
        }
        public async Task SendFilesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (SelectedFiles.Count <= 0) return;

                IEnumerable<FileUploadResult>? result = await SendMessage(cancellationToken);

                SelectedFiles.Clear();

                ErrorFiles.Clear();
                ErrorFiles.AddRange(result?.Where(x => x.IsSave == false).Select(x => x.Name) ?? Array.Empty<string>());
            }
            catch (Exception ex)
            {
                ErrorMessage = "Произошла ошибка: " + ex.Message;
            }
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

