namespace BlazorApp.Client.Settings
{
    public class FileUploadSettings
    {
        public static FileUploadSettings Empty => new FileUploadSettings()
        {
            EnableMaxFileSize = true,
            MaxFileSize = 0,
            EnableMaxAllowedFiles = true,
            MaxAllowedFiles = 0,
        };

        public bool EnableMaxFileSize { get; set; } = true;
        public long MaxFileSize { get; set; } = long.MaxValue;
        public bool EnableMaxAllowedFiles { get; set; } = true;
        public int MaxAllowedFiles { get; set; } = 30;
    }
}
