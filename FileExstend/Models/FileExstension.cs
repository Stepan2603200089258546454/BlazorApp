using System;
using System.Collections.Generic;
using System.Text;

namespace FileExstend.Models
{
    public enum FileType
    {
        UNKNOWN,
        /// <summary>
        /// Изображения
        /// </summary>
        Image,
        /// <summary>
        /// Видео
        /// </summary>
        Video,
        /// <summary>
        /// Аудио
        /// </summary>
        Audio,
        /// <summary>
        /// Текст
        /// </summary>
        Text,
        /// <summary>
        /// Архив
        /// </summary>
        Archive,
        /// <summary>
        /// Исполняемый файл
        /// </summary>
        Executable,
        /// <summary>
        /// Web файл
        /// </summary>
        Web,
        /// <summary>
        /// Файл БД
        /// </summary>
        DataBase,
        /// <summary>
        /// Файл исходного кода
        /// </summary>
        Code,
        /// <summary>
        /// Специальные файлы
        /// </summary>
        Special,
        /// <summary>
        /// 3D файлы
        /// </summary>
        _3D,
        /// <summary>
        /// Шрифт
        /// </summary>
        Font,

        PDF,

        Document,
        MSWord,
        MSExcel,
        MSPowerPoint,
        EBook,

        /// <summary>
        /// Потоковый файл
        /// </summary>
        Stream,
    }
    public class FileExstension
    {
        public static FileExstension Stream { get; private set; } = new FileExstension()
        {
            MIME = "application/octet-stream",
            Type = FileType.Stream,
        };

        public static FileExstension Default(string exstension)
        {
            return new FileExstension()
            {
                Exstension = exstension,
                Description = "Неизвестный тип файла, присвоено потоковое значение",
                MIME = "application/octet-stream",
                Type = FileType.Stream,
            };
        }

        /// <summary>
        /// Расширение в формате .txt
        /// </summary>
        public string Exstension { get; internal set; } = string.Empty;
        /// <summary>
        /// MIME тип файла
        /// </summary>
        public string MIME { get; internal set; } = string.Empty;
        /// <summary>
        /// Описание типа файла
        /// </summary>
        public string Description { get; internal set; } = string.Empty;
        /// <summary>
        /// Общий тип файла например текст или изображение
        /// </summary>
        public FileType Type { get; internal set; } = FileType.UNKNOWN;
    }
}
