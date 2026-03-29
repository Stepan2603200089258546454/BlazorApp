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
        /// <summary>
        /// Использовать ограничение по максимальному размеру файла
        /// </summary>
        public bool EnableMaxFileSize { get; set; } = true;
        /// <summary>
        /// Максимальный размер файла (по умолчанию 1_073_741_824 байт = 1 ГБ)
        /// </summary>
        public long MaxFileSize { get; set; } = 1_073_741_824;
        /// <summary>
        /// Использовать ограничение по максимальному кол-во файлов
        /// </summary>
        public bool EnableMaxAllowedFiles { get; set; } = true;
        /// <summary>
        /// Максимальное кол-во файлов (по умолчанию 10)
        /// </summary>
        public int MaxAllowedFiles { get; set; } = 10;
    }
}
