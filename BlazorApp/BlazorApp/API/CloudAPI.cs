using BlazorApp.Client.Models;
using BlazorApp.Models.Request;
using BlazorApp.Services.Cloud;
using FileExstend;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.API
{
    public static class CloudAPI
    {
        public static IEndpointRouteBuilder MapCloudAPIEndpoints(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            endpoints.MapPost("cloud/file/dowload", PostDownload)
                .RequireAuthorization();

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
    }
}
