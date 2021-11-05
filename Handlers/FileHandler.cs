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
            var type = FileExtensions.FileExtensions.GetFileType(fileExtension);

            return type.ToString() ?? "Various";
        }

        public string CreateOrUpdateDirectory(string fileExtension)
        {
            string saveDirectory = $"{_basePath}\\{fileExtension}";

            if (!Directory.Exists(saveDirectory))
            {
                try
                {
                    Directory.CreateDirectory(saveDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when creating directory - {ex.Message}");
                }
            }

            return saveDirectory;
        }

        public bool FileExistsInDirectory(string directoryPath, string file)
        {
            var fileNames = Directory.GetFiles(directoryPath).Select(Path.GetFileName);

            return fileNames.Contains(file);
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