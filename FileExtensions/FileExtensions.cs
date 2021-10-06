using System.Collections.Generic;
using System.Linq;

namespace FileSorter.FileExtensions
{
    public static class FileExtensions
    {
        public static string[] ZipExtensions = new string[] { ".zip", ".rar", ".7z" };
        public static string[] ImageExtensions = new string[] { ".png", ".jpg", ".gif", ".jpeg", ".eps", ".bmp", ".tif", ".tiff" };
        public static string[] InstallerExtensions = new string[] { ".exe", ".msi" };
        public static string[] BookExtensions = new string[] { ".epub", ".mobi", ".pdf" };
        public static string[] DocumentExtensions = new string[] { ".txt", ".doc", ".docx", ".rtf", ".xlsx" };
        public static string[] VideoExtensions = new string[] { ".avi", ".mp4", ".mpeg", ".mpg" };
        public static string[] AudioExtensions = new string[] { ".mp3", ".wav", ".au", ".flac", ".aac", ".m4a" };
        public static string[] TorrentExtensions = new string[] { ".torrent" };

        public static Dictionary<FileType, string[]> Extensions = new Dictionary<FileType, string[]>()
        {
            {FileType.Archives, ZipExtensions},
            {FileType.Images, ImageExtensions },
            {FileType.Installers, InstallerExtensions},
            {FileType.Books, BookExtensions },
            {FileType.Documents, DocumentExtensions},
            {FileType.Videos, VideoExtensions },
            {FileType.Audio, AudioExtensions },
            {FileType.Torrents, TorrentExtensions}
        };

        public static FileType GetFileType(string extension)
        {
            var type = Extensions.Where(x
                => x.Value.Contains(extension))
                .Select(x
                    => x.Key).FirstOrDefault();

            return type;
        }
    }
}