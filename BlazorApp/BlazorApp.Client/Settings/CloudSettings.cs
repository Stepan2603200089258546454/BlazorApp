namespace BlazorApp.Client.Settings
{
    public class CloudSettings
    {
        public static CloudSettings Empty => new CloudSettings()
        {
            EnableMaxClouds = true,
            MaxClouds = 0,
        };
        /// <summary>
        /// Использовать ограничение на кол-во дисков
        /// </summary>
        public bool EnableMaxClouds { get; set; } = true;
        /// <summary>
        /// Максимальное кол-во дисков (по умолчанию 5)
        /// </summary>
        public long MaxClouds { get; set; } = 5;
        /// <summary>
        /// Использовать ограничение на максимальный размер диска
        /// </summary>
        public bool EnableMaxCloudSize { get; set; } = true;
        /// <summary>
        /// Максимальный размер диска (по умолчанию 10_737_418_240 байт = 10 ГБ)
        /// </summary>
        public long MaxCloudSize { get; set; } = 10_737_418_240; 
    }
}
