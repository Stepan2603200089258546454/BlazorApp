using BlazorApp.Client.Models;
using BlazorApp.Client.Settings;
using BlazorApp.Helpers;
using DataContext.Models;
using FileExstend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace BlazorApp.API
{
    public static class FileUploadAPI
    {
        public static IEndpointRouteBuilder MapFileUploadAPIEndpoints(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            endpoints.MapPost("/File/Upload", Upload)
                .DisableAntiforgery();
            endpoints.MapGet("/File/GetSettings", GetSettings);
            endpoints.MapGet("/File/GetFiles", GetFiles);
            endpoints.MapPost("/File/Download", Download);
            endpoints.MapPost("/File/Delete", Delete)
                .DisableAntiforgery();

            return endpoints;
        }

        private static async Task<IResult> GetSettings(IOptionsMonitor<FileUploadSettings> options)
        {
            return TypedResults.Ok(options.CurrentValue);
        }
        private static async Task<IResult> GetFiles([FromServices] FileHelper fileHelper)
        {
            return TypedResults.Ok(fileHelper.GetFiles());
        }
        private static async Task<IResult> Upload([FromForm] IFormFileCollection files, HttpContext context, [FromServices] FileHelper fileHelper)
        {
            if (files is null || files?.Count() <= 0)
                return TypedResults.BadRequest();

            var saveResult = await fileHelper.SaveFiles(files!);

            return TypedResults.Ok<IEnumerable<FileUploadResult>>(saveResult.Value?.Where(x => x.IsSave == false));
        }
        private static async Task<IResult> Download([FromForm] string Path, HttpContext context, [FromServices] FileHelper fileHelper)
        {
            Result<byte[]> result = await fileHelper.DownloadAsync(Path);
            if (result.Success)
                return TypedResults.File(
                    fileContents: result.Value!,
                    contentType: FileExstensionHelper.GetExtensionType(in Path).MIME,
                    fileDownloadName: FileExstensionHelper.GetFileName(in Path)
                    );
            else
                return TypedResults.BadRequest(result.ErrorMessage);
        }
        private static async Task<IResult> Delete([FromForm] string Path, HttpContext context, [FromServices] FileHelper fileHelper)
        {
            return TypedResults.Ok(fileHelper.Delete(Path));
        }
    }
}
