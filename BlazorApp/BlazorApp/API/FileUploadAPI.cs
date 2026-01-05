using BlazorApp.Client.Models;
using BlazorApp.Client.Models.Request;
using BlazorApp.Client.Settings;
using BlazorApp.Helpers;
using BlazorApp.Models.Request;
using DataContext.Models;
using FileExstend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Routing;
using System.Collections.Generic;

namespace BlazorApp.API
{
    public static class FileUploadAPI
    {
        public static IEndpointRouteBuilder MapFileUploadAPIEndpoints(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            endpoints.MapPost(Files.API.Upload, PostUpload)
                .RequireAuthorization()
                .DisableAntiforgery();
            
            endpoints.MapGet(Files.API.GetSettings, GetSettings)
                .RequireAuthorization();
            
            endpoints.MapPost(Files.API.GetFiles, GetFiles)
                .RequireAuthorization();
            
            endpoints.MapPost(Files.API.Download, PostDownload)
                .RequireAuthorization();
            
            endpoints.MapPost(Files.API.Delete, PostDelete)
                .RequireAuthorization()
                .DisableAntiforgery();

            return endpoints;
        }

        private static async Task<IResult> GetSettings(IOptionsMonitor<FileUploadSettings> options)
        {
            return TypedResults.Ok<FileUploadSettings>(options.CurrentValue);
        }
        private static async Task<IResult> GetFiles(FileGetRequest request, [FromServices] FileHelper fileHelper, HttpContext context)
        {
            return TypedResults.Ok<IEnumerable<FileItem>>(await fileHelper.GetFiles(request, context.User));
        }
        private static async Task<IResult> PostUpload([FromForm] FileUploadRequest request, HttpContext context, [FromServices] FileHelper fileHelper)
        {
            if (request.Files is null || request.Files?.Count() <= 0)
                return TypedResults.BadRequest();

            Result<List<FileUploadResult>> saveResult = await fileHelper.SaveFiles(request.Files!, context.User);

            return TypedResults.Ok<IEnumerable<FileUploadResult>>(saveResult.Value?.Where(x => x.IsSave == false));
        }
        private static async Task<IResult> PostDownload([FromForm] FileDownloadRequest request, HttpContext context, [FromServices] FileHelper fileHelper)
        {
            Result<byte[]> result = await fileHelper.DownloadAsync(request.Path, context.User);
            if (result.Success)
                return TypedResults.File(
                    fileContents: result.Value!,
                    contentType: FileExstensionHelper.GetExtensionType(request.Path).MIME,
                    fileDownloadName: FileExstensionHelper.GetFileName(request.Path)
                    );
            else
                return TypedResults.BadRequest(result.ErrorMessage);
        }
        private static async Task<IResult> PostDelete([FromForm] FileDeleteRequest request, HttpContext context, [FromServices] FileHelper fileHelper)
        {
            return TypedResults.Ok<Result>(await fileHelper.Delete(request.Path, context.User));
        }
    }
}
