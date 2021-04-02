using System.Collections.Generic;

namespace FileSorter.Extensions
{
    public static class FileExtensions
    {
        public static string[] ZipExtensions = new string[] { ".zip", ".rar", ".7z" };
        public static string[] ImageExtensions = new string[] { ".png", ".jpg", ".gif", ".jpeg", ".eps", ".bmp", ".tif", ".tiff" };
        public static string[] InstallerExtensions = new string[] { ".exe", ".msi" };
        public static string[] BookExtensions = new string[] { ".epub", ".mobi", ".pdf" };
        public static string[] DocumentExtensions = new string[] { ".txt", ".doc", ".docx", ".rtf" };
        public static string[] VideoExtensions = new string[] { ".avi", ".mp4", ".mpeg", ".mpg" };

        public static Dictionary<string, string[]> Extensions = new Dictionary<string, string[]>();

        //public FileExtensions()
        //{
        //    Extensions.Add("ZipExtensions", ZipExtensions);
        //    Extensions.Add("ImageExtensions", ImageExtensions);
        //    Extensions.Add("InstallerExtensions", InstallerExtensions);
        //    Extensions.Add("BookExtensions", BookExtensions);
        //    Extensions.Add("DocumentExtensions", DocumentExtensions);
        //    Extensions.Add("VideoExtensions", VideoExtensions);
        //}
    }
}