namespace DataContext.Models.Cloud
{
    public class CloudFileData
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Содержимое файла
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// Размер файла
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Хэш MD5
        /// </summary>
        public string Md5Hash { get; set; }
        /// <summary>
        /// Хэш SHA256
        /// </summary>
        public string Sha256Hash { get; set; }
        public CloudItem CloudItem { get; set; }
    }
}
