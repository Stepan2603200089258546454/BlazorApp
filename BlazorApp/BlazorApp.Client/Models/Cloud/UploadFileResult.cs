namespace BlazorApp.Client.Models.Cloud
{
    public class UploadFileResult
    {
        public bool Success { get; set; } = false;
        public string FileName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
