﻿using System;
using System.IO;
using System.Linq;

namespace FileSorter
{
    class Program
    {

        public static string BasePath = $"C:\\Users\\{Environment.UserName}\\Downloads";
        static void Main(string[] args)
        {
            var files = Directory.GetFileSystemEntries(BasePath);

            foreach (var file in files)
            {
                var fileName = file.Substring(BasePath.Length + 1);
                var extension = file.Split('.').Last();

                var fileType = DetermineFileType(extension);

                var directory = GetCreatedOrUpdatedDirectoryPath(fileType);

                if (string.IsNullOrEmpty(directory))
                    continue;
                try
                {
                    File.Move(Path.Combine(BasePath, fileName), Path.Combine(directory, fileName));
                }
                catch (Exception)
                {
                    Console.WriteLine($"Couldn't move {fileName} to {directory}");
                }
            }
        }
        public static string GetCreatedOrUpdatedDirectoryPath(string extension)
        {
            if (!Directory.Exists($"{BasePath}/{extension}"))
            {
                try
                {
                    Directory.CreateDirectory($"{BasePath}/{extension}");
                    return $"{BasePath}\\{extension}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error when creating directory");
                }
            }

            return $"{BasePath}/{extension}";
        }

        private static string DetermineFileType(string extension)
        {
            var zipExtensions = new string[] { "zip", "rar", "7z" };
            var imageExtensions = new string[] { "png", "jpg", "gif", "jpeg", "eps", "bmp", "tif", "tiff" };
            var installerExtensions = new string[] { "exe", "msi" };
            var bookExtensions = new string[] { "epub", "mobi", "pdf" };
            var textExtensions = new string[] { "txt", "doc", "docx", "rtf" };
            var videoExtensions = new string[] { "avi", "mp4", "mpeg", "mpg" };

            if (imageExtensions.Contains(extension))
                return "Images";
            if (zipExtensions.Contains(extension))
                return "Zip 1";
            if (installerExtensions.Contains(extension))
                return "Installers";
            if (bookExtensions.Contains(extension))
                return "Books";
            if (textExtensions.Contains(extension))
                return "Documents";
            if (videoExtensions.Contains(extension))
                return "Videos";
            if (extension == "torrent")
                return "Torrents";
            return "Various";
        }

    }
}
