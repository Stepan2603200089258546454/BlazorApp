using BlazorApp.Client.Models;
using BlazorApp.Models.Request;
using BlazorApp.Services.Cloud;
using FileExstend;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BlazorApp.API
{
    public static class CloudAPI
    {
        public static IEndpointRouteBuilder MapCloudAPIEndpoints(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            endpoints.MapPost("cloud/file/dowload", PostDownload)
                .RequireAuthorization();
            endpoints.MapGet("v/" + CloudProvider.MainFolder + "/{parrentName}/{fileName}", ViewFileFromParrent)
                .RequireAuthorization();
            endpoints.MapGet("v/" + CloudProvider.MainFolder + "/{fileName}", ViewFile)
                .RequireAuthorization();
            endpoints.MapGet(CloudProvider.MainFolder + "/{*path}", (string path) =>
            {
                return Results.NotFound();
            });

            return endpoints;
        }

        private static async Task<IResult> PostDownload([FromForm] CloudFileDownloadRequest request, HttpContext context, [FromServices] CloudProvider cloudProvider)
        {
            Result<byte[]> result = await cloudProvider.DownloadCloudFilesAsync(request.CloudId.Value, request.ParrentId, request.SystemName, context.User);
            if (result.Success)
                return TypedResults.File(
                    fileContents: result.Value!,
                    contentType: FileExstensionHelper.GetExtensionType(request.SystemName).MIME,
                    fileDownloadName: FileExstensionHelper.GetFileName(request.SystemName)
                    );
            else
                return TypedResults.BadRequest(result.ErrorMessage);
        }
        private static async Task<IResult> ViewFileFromParrent(HttpContext context, string parrentName, string fileName, [FromServices] CloudProvider cloudProvider)
        {
            Guid? parrentId = string.IsNullOrEmpty(parrentName) ? null : Guid.Parse(parrentName);

            var result = await cloudProvider.ViewCloudFilesAsync(parrentId, fileName, context.User);
            if (result.Success)
            {
                return Results.File(result.Value.FileStream, result.Value.ContentType);
            }
            else 
                return Results.BadRequest(result.ErrorMessage);
        }
        private static async Task<IResult> ViewFile(HttpContext context, string fileName, [FromServices] CloudProvider cloudProvider)
        {
            var result = await cloudProvider.ViewCloudFilesAsync(null, fileName, context.User);
            if (result.Success)
            {
                return Results.File(result.Value.FileStream, result.Value.ContentType);
            }
            else
                return Results.BadRequest(result.ErrorMessage);
        }
    }
}
