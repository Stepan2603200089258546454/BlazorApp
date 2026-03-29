namespace BlazorApp.API
{
    public static class ApplicationAPI
    {
        public static IEndpointRouteBuilder UsingAPIEndpoints(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            endpoints.MapFileUploadAPIEndpoints();
            endpoints.MapCloudAPIEndpoints();

            return endpoints;
        }
    }
}
