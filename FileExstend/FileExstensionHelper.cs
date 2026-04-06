using FileExstend.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileExstend
{
    public class FileExstensionHelper
    {
        public static IReadOnlyCollection<FileExstension> FilesExstension { get; } = new List<FileExstension>()
        {
            new FileExstension()
            {
                Exstension = ".txt", MIME = "text/plain", Type = FileType.Text, Description = "Текст",
            },
            new FileExstension()
            {
                Exstension = ".log", MIME = "text/plain", Type = FileType.Text, Description = "Текст (логи)",
            },
            new FileExstension()
            {
                Exstension = ".csv", MIME = "text/csv", Type = FileType.Text, Description = "Текст (данные)",
            },
            new FileExstension()
            {
                Exstension = ".rtf", MIME = "application/rtf", Type = FileType.Text, Description = "Текст (форматированный)",
            },
            new FileExstension()
            {
                Exstension = ".md", MIME = "text/markdown", Type = FileType.Text, Description = "Текст (разметка)",
            },
            new FileExstension()
            {
                Exstension = ".markdown", MIME = "text/markdown", Type = FileType.Text, Description = "Текст (разметка)",
            },
            

            new FileExstension()
            {
                Exstension = ".pdf", MIME = "application/pdf", Type = FileType.PDF, Description = "Документ",
            },
            new FileExstension()
            {
                Exstension = ".doc", MIME = "application/msword", Type = FileType.MSWord, Description = "Документ (MS Word)",
            },
            new FileExstension()
            {
                Exstension = ".docx", MIME = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Type = FileType.MSWord, Description = "Документ (MS Word)",
            },
            new FileExstension()
            {
                Exstension = ".odt", MIME = "application/vnd.oasis.opendocument.text", Type = FileType.Document, Description = "Документ (OpenOffice/LibreOffice)",
            },
            new FileExstension()
            {
                Exstension = ".xls", MIME = "application/vnd.ms-excel", Type = FileType.MSExcel, Description = "Таблица (MS Excel)",
            },
            new FileExstension()
            {
                Exstension = ".xlsx", MIME = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Type = FileType.MSExcel, Description = "Таблица (MS Excel)",
            },
            new FileExstension()
            {
                Exstension = ".ods", MIME = "application/vnd.oasis.opendocument.spreadsheet", Type = FileType.Document, Description = "Таблица (OpenOffice/LibreOffice)",
            },
            new FileExstension()
            {
                Exstension = ".ppt", MIME = "application/vnd.ms-powerpoint", Type = FileType.MSPowerPoint, Description = "Презентация (MS PowerPoint)",
            },
            new FileExstension()
            {
                Exstension = ".pptx", MIME = "application/vnd.openxmlformats-officedocument.presentationml.presentation", Type = FileType.MSPowerPoint, Description = "Презентация (MS PowerPoint)",
            },
            new FileExstension()
            {
                Exstension = ".odp", MIME = "application/vnd.oasis.opendocument.presentation", Type = FileType.Document, Description = "Презентация (OpenOffice/LibreOffice)",
            },
            new FileExstension()
            {
                Exstension = ".epub", MIME = "application/epub+zip", Type = FileType.EBook, Description = "Электронная книга",
            },
            new FileExstension()
            {
                Exstension = ".mobi", MIME = "application/x-mobipocket-ebook", Type = FileType.EBook, Description = "Электронная книга",
            },
            new FileExstension()
            {
                Exstension = ".fb2", MIME = "application/x-fictionbook+xml", Type = FileType.EBook, Description = "Электронная книга",
            },


            new FileExstension()
            {
                Exstension = ".jpg", MIME = "image/jpeg", Type = FileType.Image, Description = "Изображение",
            },
            new FileExstension()
            {
                Exstension = ".jpeg", MIME = "image/jpeg", Type = FileType.Image, Description = "Изображение",
            },
            new FileExstension()
            {
                Exstension = ".png", MIME = "image/png", Type = FileType.Image, Description = "Изображение",
            },
            new FileExstension()
            {
                Exstension = ".gif", MIME = "image/gif", Type = FileType.Image, Description = "Изображение (анимация)",
            },
            new FileExstension()
            {
                Exstension = ".bmp", MIME = "image/bmp", Type = FileType.Image, Description = "Изображение",
            },
            new FileExstension()
            {
                Exstension = ".webp", MIME = "image/webp", Type = FileType.Image, Description = "Изображение",
            },
            new FileExstension()
            {
                Exstension = ".tiff", MIME = "image/tiff", Type = FileType.Image, Description = "Изображение (сжатие без потерь)",
            },
            new FileExstension()
            {
                Exstension = ".tif", MIME = "image/tiff", Type = FileType.Image, Description = "Изображение (сжатие без потерь)",
            },
            new FileExstension()
            {
                Exstension = ".ico", MIME = "image/x-icon", Type = FileType.Image, Description = "Изображение (иконка)",
            },
            new FileExstension()
            {
                Exstension = ".psd", MIME = "image/vnd.adobe.photoshop", Type = FileType.Image, Description = "Изображение (редактируемое, Photoshop)",
            },
            new FileExstension()
            {
                Exstension = ".svg", MIME = "image/svg+xml", Type = FileType.Image, Description = "Векторное изображение",
            },
            new FileExstension()
            {
                Exstension = ".ai", MIME = "application/postscript", Type = FileType.Image, Description = "Векторное изображение (Adobe Illustrator)",
            },
            new FileExstension()
            {
                Exstension = ".eps", MIME = "application/postscript", Type = FileType.Image, Description = "Векторное изображение",
            },
            new FileExstension()
            {
                Exstension = ".cdr", MIME = "application/vnd.corel-draw", Type = FileType.Image, Description = "Векторное изображение (CorelDRAW)",
            },
            new FileExstension()
            {
                Exstension = ".sketch", MIME = "application/vnd.sketch", Type = FileType.Image, Description = "Векторное изображение (Sketch)",
            },


            new FileExstension()
            {
                Exstension = ".mp3", MIME = "audio/mpeg", Type = FileType.Audio, Description = "Аудио",
            },
            new FileExstension()
            {
                Exstension = ".wav", MIME = "audio/wav", Type = FileType.Audio, Description = "Аудио (несжатое)",
            },
            new FileExstension()
            {
                Exstension = ".flac", MIME = "audio/flac", Type = FileType.Audio, Description = "Аудио (сжатие без потерь)",
            },
            new FileExstension()
            {
                Exstension = ".ogg", MIME = "audio/ogg", Type = FileType.Audio, Description = "Аудио",
            },
            new FileExstension()
            {
                Exstension = ".m4a", MIME = "audio/mp4", Type = FileType.Audio, Description = "Аудио",
            },
            new FileExstension()
            {
                Exstension = ".aac", MIME = "audio/aac", Type = FileType.Audio, Description = "Аудио",
            },
            new FileExstension()
            {
                Exstension = ".opus", MIME = "audio/opus", Type = FileType.Audio, Description = "Аудио",
            },
            new FileExstension()
            {
                Exstension = ".wma", MIME = "audio/x-ms-wma", Type = FileType.Audio, Description = "Аудио (Windows Media)",
            },
            new FileExstension()
            {
                Exstension = ".mid", MIME = "audio/midi", Type = FileType.Audio, Description = "Аудио (MIDI-синтезатор)",
            },
            new FileExstension()
            {
                Exstension = ".midi", MIME = "audio/midi", Type = FileType.Audio, Description = "Аудио (MIDI-синтезатор)",
            },


            new FileExstension()
            {
                Exstension = ".mp4", MIME = "video/mp4", Type = FileType.Video, Description = "Видео",
            },
            new FileExstension()
            {
                Exstension = ".avi", MIME = "video/x-msvideo", Type = FileType.Video, Description = "Видео",
            },
            new FileExstension()
            {
                Exstension = ".mkv", MIME = "video/x-matroska", Type = FileType.Video, Description = "Видео (контейнер)",
            },
            new FileExstension()
            {
                Exstension = ".mov", MIME = "video/quicktime", Type = FileType.Video, Description = "Видео (QuickTime)",
            },
            new FileExstension()
            {
                Exstension = ".webm", MIME = "video/webm", Type = FileType.Video, Description = "Видео",
            },
            new FileExstension()
            {
                Exstension = ".wmv", MIME = "video/x-ms-wmv", Type = FileType.Video, Description = "Видео (Windows Media)",
            },
            new FileExstension()
            {
                Exstension = ".flv", MIME = "video/x-flv", Type = FileType.Video, Description = "Видео (Flash)",
            },
            new FileExstension()
            {
                Exstension = ".mpg", MIME = "video/mpeg", Type = FileType.Video, Description = "Видео",
            },
            new FileExstension()
            {
                Exstension = ".mpeg", MIME = "video/mpeg", Type = FileType.Video, Description = "Видео",
            },
            new FileExstension()
            {
                Exstension = ".3gp", MIME = "video/3gpp", Type = FileType.Video, Description = "Видео (мобильное)",
            },


            new FileExstension()
            {
                Exstension = ".zip", MIME = "application/zip", Type = FileType.Archive, Description = "Архив",
            },
            new FileExstension()
            {
                Exstension = ".rar", MIME = "application/vnd.rar", Type = FileType.Archive, Description = "Архив",
            },
            new FileExstension()
            {
                Exstension = ".7z", MIME = "application/x-7z-compressed", Type = FileType.Archive, Description = "Архив",
            },
            new FileExstension()
            {
                Exstension = ".tar", MIME = "application/x-tar", Type = FileType.Archive, Description = "Архив (без сжатия)",
            },
            new FileExstension()
            {
                Exstension = ".gz", MIME = "application/gzip", Type = FileType.Archive, Description = "Архив",
            },
            new FileExstension()
            {
                Exstension = ".bz2", MIME = "application/x-bzip2", Type = FileType.Archive, Description = "Архив",
            },
            new FileExstension()
            {
                Exstension = ".xz", MIME = "application/x-xz", Type = FileType.Archive, Description = "Архив",
            },


            new FileExstension()
            {
                Exstension = ".exe", MIME = "application/x-msdownload", Type = FileType.Executable, Description = "Исполняемый файл (Windows)",
            },
            new FileExstension()
            {
                Exstension = ".msi", MIME = "application/x-ms-installer", Type = FileType.Executable, Description = "Установщик (Windows)",
            },
            new FileExstension()
            {
                Exstension = ".apk", MIME = "application/vnd.android.package-archive", Type = FileType.Executable, Description = "Приложение (Android)",
            },
            new FileExstension()
            {
                Exstension = ".app", MIME = "application/x-executable", Type = FileType.Executable, Description = "Приложение (macOS)",
            },
            new FileExstension()
            {
                Exstension = ".deb", MIME = "application/vnd.debian.binary-package", Type = FileType.Executable, Description = "Пакет (Debian/Ubuntu)",
            },
            new FileExstension()
            {
                Exstension = ".rpm", MIME = "application/x-rpm", Type = FileType.Executable, Description = "Пакет (Red Hat/Fedora)",
            },
            new FileExstension()
            {
                Exstension = ".bat", MIME = "application/x-msdownload", Type = FileType.Executable, Description = "Скрипт (Windows Batch)",
            },
            new FileExstension()
            {
                Exstension = ".sh", MIME = "application/x-sh", Type = FileType.Executable, Description = "Скрипт (Shell)",
            },
            new FileExstension()
            {
                Exstension = ".jar", MIME = "application/java-archive", Type = FileType.Executable, Description = "Исполняемый архив (Java)",
            },


            new FileExstension()
            {
                Exstension = ".html", MIME = "text/html", Type = FileType.Web, Description = "Веб-страница",
            },
            new FileExstension()
            {
                Exstension = ".htm", MIME = "text/html", Type = FileType.Web, Description = "Веб-страница",
            },
            new FileExstension()
            {
                Exstension = ".css", MIME = "text/css", Type = FileType.Web, Description = "Стили (веб)",
            },
            new FileExstension()
            {
                Exstension = ".js", MIME = "text/javascript", Type = FileType.Web, Description = "Скрипт (веб)",
            },
            new FileExstension()
            {
                Exstension = ".json", MIME = "application/json", Type = FileType.Web, Description = "Данные (веб)",
            },
            new FileExstension()
            {
                Exstension = ".xml", MIME = "application/xml", Type = FileType.Web, Description = "Данные (разметка)",
            },
            new FileExstension()
            {
                Exstension = ".php", MIME = "application/x-httpd-php", Type = FileType.Web, Description = "Скрипт (веб, серверный)",
            },
            new FileExstension()
            {
                Exstension = ".asp", MIME = "application/x-asp", Type = FileType.Web, Description = "Скрипт (веб, серверный)",
            },


            new FileExstension()
            {
                Exstension = ".sql", MIME = "application/sql", Type = FileType.DataBase, Description = "Скрипт базы данных",
            },
            new FileExstension()
            {
                Exstension = ".db", MIME = "application/vnd.sqlite3", Type = FileType.DataBase, Description = "База данных (SQLite)",
            },
            new FileExstension()
            {
                Exstension = ".sqlite", MIME = "application/vnd.sqlite3", Type = FileType.DataBase, Description = "База данных (SQLite)",
            },
            new FileExstension()
            {
                Exstension = ".mdb", MIME = "application/vnd.ms-access", Type = FileType.DataBase, Description = "База данных (Access)",
            },
            new FileExstension()
            {
                Exstension = ".accdb", MIME = "application/vnd.ms-access", Type = FileType.DataBase, Description = "База данных (Access)",
            },
            new FileExstension()
            {
                Exstension = ".dbf", MIME = "application/x-dbf", Type = FileType.DataBase, Description = "База данных (dBASE)",
            },


            new FileExstension()
            {
                Exstension = ".c", MIME = "text/x-c", Type = FileType.Code, Description = "Исходный код (C)",
            },
            new FileExstension()
            {
                Exstension = ".cpp", MIME = "text/x-c++", Type = FileType.Code, Description = "Исходный код (C++)",
            },
            new FileExstension()
            {
                Exstension = ".cc", MIME = "text/x-c++", Type = FileType.Code, Description = "Исходный код (C++)",
            },
            new FileExstension()
            {
                Exstension = ".h", MIME = "text/x-c", Type = FileType.Code, Description = "Заголовочный файл (C/C++)",
            },
            new FileExstension()
            {
                Exstension = ".java", MIME = "text/x-java-source", Type = FileType.Code, Description = "Исходный код (Java)",
            },
            new FileExstension()
            {
                Exstension = ".py", MIME = "text/x-python", Type = FileType.Code, Description = "Исходный код (Python)",
            },
            new FileExstension()
            {
                Exstension = ".cs", MIME = "text/x-csharp", Type = FileType.Code, Description = "Исходный код (C#)",
            },
            new FileExstension()
            {
                Exstension = ".go", MIME = "text/x-go", Type = FileType.Code, Description = "Исходный код (Go)",
            },
            new FileExstension()
            {
                Exstension = ".rb", MIME = "text/x-ruby", Type = FileType.Code, Description = "Исходный код (Ruby)",
            },


            new FileExstension()
            {
                Exstension = ".iso", MIME = "application/x-iso9660-image", Type = FileType.Special, Description = "Образ диска",
            },
            new FileExstension()
            {
                Exstension = ".dmg", MIME = "application/x-apple-diskimage", Type = FileType.Special, Description = "Образ диска (macOS)",
            },
            new FileExstension()
            {
                Exstension = ".img", MIME = "application/octet-stream", Type = FileType.Special, Description = "Образ диска",
            },
            new FileExstension()
            {
                Exstension = ".bin", MIME = "application/octet-stream", Type = FileType.Special, Description = "Бинарный файл",
            },
            new FileExstension()
            {
                Exstension = ".dll", MIME = "application/x-msdownload", Type = FileType.Special, Description = "Библиотека (Windows)",
            },
            new FileExstension()
            {
                Exstension = ".so", MIME = "application/x-sharedlib", Type = FileType.Special, Description = "Библиотека (Linux)",
            },
            new FileExstension()
            {
                Exstension = ".sys", MIME = "application/octet-stream", Type = FileType.Special, Description = "Драйвер (Windows)",
            },
            new FileExstension()
            {
                Exstension = ".ini", MIME = "text/plain", Type = FileType.Special, Description = "Конфигурационный файл",
            },
            new FileExstension()
            {
                Exstension = ".cfg", MIME = "text/plain", Type = FileType.Special, Description = "Конфигурационный файл",
            },
            new FileExstension()
            {
                Exstension = ".conf", MIME = "text/plain", Type = FileType.Special, Description = "Конфигурационный файл",
            },
            new FileExstension()
            {
                Exstension = ".reg", MIME = "text/plain", Type = FileType.Special, Description = "Файл реестра (Windows)",
            },
            new FileExstension()
            {
                Exstension = ".torrent", MIME = "application/x-bittorrent", Type = FileType.Special, Description = "Торрент-файл",
            },


            new FileExstension()
            {
                Exstension = ".stl", MIME = "model/stl", Type = FileType._3D, Description = "3D-модель",
            },
            new FileExstension()
            {
                Exstension = ".obj", MIME = "model/obj", Type = FileType._3D, Description = "3D-модель",
            },
            new FileExstension()
            {
                Exstension = ".fbx", MIME = "application/octet-stream", Type = FileType._3D, Description = "3D-модель (Autodesk)",
            },
            new FileExstension()
            {
                Exstension = ".dwg", MIME = "image/vnd.dwg", Type = FileType._3D, Description = "Чертеж (AutoCAD)",
            },
            new FileExstension()
            {
                Exstension = ".dxf", MIME = "image/vnd.dxf", Type = FileType._3D, Description = "Чертеж (AutoCAD)",
            },


            new FileExstension()
            {
                Exstension = ".ttf", MIME = "font/ttf", Type = FileType.Font, Description = "Шрифт (TrueType)",
            },
            new FileExstension()
            {
                Exstension = ".otf", MIME = "font/otf", Type = FileType.Font, Description = "Шрифт (OpenType)",
            },
            new FileExstension()
            {
                Exstension = ".woff", MIME = "font/woff", Type = FileType.Font, Description = "Шрифт (веб)",
            },
            new FileExstension()
            {
                Exstension = ".woff2", MIME = "font/woff2", Type = FileType.Font, Description = "Шрифт (веб)",
            },
            new FileExstension()
            {
                Exstension = ".eot", MIME = "application/vnd.ms-fontobject", Type = FileType.Font, Description = "Шрифт (Embedded OpenType, IE)",
            },
        };

        /// <summary>
        /// Получить расширение файла
        /// </summary>
        public static string GetExtension(in string filePath)
        {
            return Path.GetExtension(filePath).ToLower();
        }
        /// <summary>
        /// Получить тип расширение файла
        /// </summary>
        public static FileExstension GetExtensionType(in string filePath)
        {
            string ex = GetExtension(in filePath);

            return FilesExstension.FirstOrDefault(x => string.Equals(x.Exstension, ex, StringComparison.InvariantCultureIgnoreCase)) ?? FileExstension.Default(ex);
        }
        /// <summary>
        /// Получить имя файла без расширения
        /// </summary>
        public static string GetFileNameWithoutExtension(in string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }
        /// <summary>
        /// Получить имя файла
        /// </summary>
        public static string GetFileName(in string filePath)
        {
            return Path.GetFileName(filePath);
        }
        /// <summary>
        /// Переводит число байт в читаемый вид
        /// </summary>
        public static string FormatBytes(long bytes)
        {
            return bytes switch
            {
                < 1024 => $"{bytes} байт",
                < 1024 * 1024 => $"{bytes / 1024.0:F2} КБ",
                < 1024 * 1024 * 1024 => $"{bytes / (1024.0 * 1024):F2} МБ",
                < 1024L * 1024 * 1024 * 1024 => $"{bytes / (1024.0 * 1024 * 1024):F2} ГБ",
                < 1024L * 1024 * 1024 * 1024 * 1024 => $"{bytes / (1024.0 * 1024 * 1024 * 1024):F2} ТБ",
                _ => $"{bytes / (1024.0 * 1024 * 1024 * 1024 * 1024):F2} ПБ"
            };
        }
        public static string FormatBytes(long? bytes)
        {
            if (bytes == null) return string.Empty;
            else return FormatBytes(bytes.Value);
        }
    }
}
