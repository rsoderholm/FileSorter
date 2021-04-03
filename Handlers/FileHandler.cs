using System;
using System.IO;
using System.Linq;

namespace FileSorter.Handlers
{
    public class FileHandler : IFileHandler
    {
        private readonly string _basePath;

        public FileHandler(string basePath)
        {
            _basePath = basePath;
        }

        public string DetermineFolderByExtension(string fileExtension)
        {
            if (FileExtensions.FileExtensions.ImageExtensions.Contains(fileExtension))
                return "Images";
            if (FileExtensions.FileExtensions.ZipExtensions.Contains(fileExtension))
                return "Zippables";
            if (FileExtensions.FileExtensions.InstallerExtensions.Contains(fileExtension))
                return "Installers";
            if (FileExtensions.FileExtensions.BookExtensions.Contains(fileExtension))
                return "Books";
            if (FileExtensions.FileExtensions.DocumentExtensions.Contains(fileExtension))
                return "Documents";
            if (FileExtensions.FileExtensions.VideoExtensions.Contains(fileExtension))
                return "Videos";
            if (fileExtension == ".torrent")
                return "Torrents";
            return "Various";
        }

        public string CreateOrUpdateDirectory(string fileExtension)
        {
            string newDirectory = $"{_basePath}\\{fileExtension}";

            if (!Directory.Exists(newDirectory))
            {
                try
                {
                    Directory.CreateDirectory(newDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when creating directory");
                }
            }

            return newDirectory;
        }

        public void Move(string from, string to)
        {
            File.Move(from, to);
        }

        public void Delete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not delete {path}: {ex.ToString()}");
            }
        }
    }
}