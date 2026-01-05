using BlazorApp.Client.Interfaces;
using BlazorApp.Client.Models;
using BlazorApp.Client.Models.Request;
using BlazorApp.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorApp.Services
{
    internal class FileListService : IFileListService
    {
        protected readonly FileHelper _fileHelper;
        protected readonly AuthenticationStateProvider _authStateProvider;
        public string ErrorMessage { get; private set; } = string.Empty;
        public FileGetRequest GetRequest { get; private set; } = new FileGetRequest();

        public FileListService(FileHelper fileHelper, AuthenticationStateProvider authStateProvider)
        {
            _fileHelper = fileHelper;
            _authStateProvider = authStateProvider;
        }

        private async Task<ClaimsPrincipal> GetUser()
        {
            AuthenticationState authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user;
        }

        public async Task<IEnumerable<FileItem>?> GetFiles(CancellationToken cancellationToken = default)
        {
            return await _fileHelper.GetFiles(GetRequest, await GetUser());
        }

        public async Task<Result?> DeleteFile(string filePath, CancellationToken cancellationToken = default)
        {
            return await _fileHelper.Delete(filePath, await GetUser());
        }
    }
}
