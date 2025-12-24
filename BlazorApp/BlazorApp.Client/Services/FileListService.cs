using BlazorApp.Client.Interfaces;
using BlazorApp.Client.Models;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace BlazorApp.Client.Services
{
    internal class FileListService : IFileListService
    {
        protected readonly HttpClient _client;
        public string ErrorMessage { get; private set; } = string.Empty;

        public FileListService(HttpClient client)
        {
            _client = client;
        }


        public async Task<IEnumerable<FileItem>?> GetFiles(CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _client.GetAsync("/File/GetFiles", cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                IEnumerable<FileItem>? result = await JsonSerializer.DeserializeAsync<IEnumerable<FileItem>>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                }, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                ErrorMessage = "Отмена операции";
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Произошла ошибка";
                return null;
            }
        }

        public async Task<Result?> DeleteFile(string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                // Создаем FormUrlEncodedContent
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["Path"] = filePath
                });
                // Создаем HttpRequestMessage
                var request = new HttpRequestMessage(HttpMethod.Post, "/File/Delete")
                {
                    Content = content
                };
                // Отправляем запрос
                var response = await _client.SendAsync(request);
                cancellationToken.ThrowIfCancellationRequested();
                using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await JsonSerializer.DeserializeAsync<Result>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                }, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result?.Success == false)
                {
                    ErrorMessage = result.ErrorMessage;
                }
                return result;
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                ErrorMessage = "Отмена операции";
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Произошла ошибка";
                return null;
            }
        }
    }
}

