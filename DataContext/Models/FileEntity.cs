using System;
using System.Collections.Generic;
using System.Text;

namespace DataContext.Models
{
    public class FileEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Название файла
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Содержимое файла
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// Размер файла
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime InsertDateUtc { get; set; }
        /// <summary>
        /// Хэш MD5
        /// </summary>
        public string Md5Hash { get; set; }
        /// <summary>
        /// Хэш SHA256
        /// </summary>
        public string Sha256Hash { get; set; }
        /// <summary>
        /// Пользователь (владелец файла)
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// Пользователь (владелец файла)
        /// </summary>
        public ApplicationUser? User { get; set; }
    }
}
