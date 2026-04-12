namespace BlazorApp.Client.Settings
{
    public class CloudSettings
    {
        public static CloudSettings Empty => new CloudSettings()
        {
            EnableMaxPersonalClouds = true,
            MaxPersonalClouds = 0,
            EnableMaxPersonalCloudSize = true,
            MaxPersonalCloudSize = 0,
            EnableMaxGlobalClouds = true,
            MaxGlobalClouds = 0,
            EnableMaxGlobalCloudSize = true,
            MaxGlobalCloudSize = 0,
        };
        /// <summary>
        /// Использовать ограничение на кол-во дисков
        /// </summary>
        public bool EnableMaxPersonalClouds { get; set; } = true;
        /// <summary>
        /// Максимальное кол-во дисков (по умолчанию 5)
        /// </summary>
        public long MaxPersonalClouds { get; set; } = 5;
        /// <summary>
        /// Использовать ограничение на максимальный размер диска
        /// </summary>
        public bool EnableMaxPersonalCloudSize { get; set; } = true;
        /// <summary>
        /// Максимальный размер диска (по умолчанию 10_737_418_240 байт = 10 ГБ)
        /// </summary>
        public long MaxPersonalCloudSize { get; set; } = 10_737_418_240;
        /// <summary>
        /// Использовать ограничение на кол-во расшаренных дисков
        /// </summary>
        public bool EnableMaxGlobalClouds { get; set; } = true;
        /// <summary>
        /// Максимальное кол-во расшаренных дисков (по умолчанию 5)
        /// </summary>
        public long MaxGlobalClouds { get; set; } = 5;
        /// <summary>
        /// Использовать расшаренного ограничение на максимальный размер диска
        /// </summary>
        public bool EnableMaxGlobalCloudSize { get; set; } = true;
        /// <summary>
        /// Максимальный размер расшаренного диска (по умолчанию 10_737_418_240 байт = 10 ГБ)
        /// </summary>
        public long MaxGlobalCloudSize { get; set; } = 10_737_418_240;
    }
}
