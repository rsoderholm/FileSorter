using FileSorter.Extensions;
using FileSorter.Handlers;
using System;
using System.IO;
using System.Linq;

namespace FileSorter.Services
{
    public class SortService
    {
        private FileHandler _fileHandler;
        private string _basePath;

        public SortService(FileHandler fileHandler, string basePath)
        {
            _fileHandler = fileHandler;
            _basePath = basePath;
        }

        public string DefaultBasePath()
        {
            return $"C:\\Users\\{Environment.UserName}\\Downloads";
        }

        public void Sort()
        {
            var files = Directory.GetFileSystemEntries(_basePath);

            int counter = 0;

            var fileCounter = files.Count(x => !IsFolder(x));

            var filteredFiles = files.Where(x => (!IsFolder(x))).ToList();

            if (!filteredFiles.Any())
                return;

            Console.WriteLine($"{fileCounter} new files found. Attempting to sort.");
            foreach (var file in filteredFiles)
            {
                var fileName = file.Substring(_basePath.Length + 1);
                var extension = Path.GetExtension(fileName);

                var fileType = _fileHandler.DetermineFolderByExtension(extension);

                var directory = _fileHandler.CreateOrUpdateDirectory(fileType);

                if (FileExistsInDirectory(directory, fileName))
                {
                    Console.WriteLine($"{fileName} exists already, deleting instead");
                    _fileHandler.Delete(file);
                    return;
                }

                try
                {
                    Console.WriteLine($"Moving {fileName} to {directory}");
                    _fileHandler.Move(Path.Combine(_basePath, fileName), Path.Combine(directory, fileName));
                    counter++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Couldn't move {fileName} to {directory}");
                }
            }

            Console.WriteLine($"Moved {counter} files in total.");
        }

        private bool FileExistsInDirectory(string directoryPath, string file)
        {
            var fileNames = Directory.GetFiles(directoryPath).Select(x => Path.GetFileName(x));

            return fileNames.Contains(file);
        }

        private bool IsFolder(string path)
        {
            var fileAttributes = File.GetAttributes(path);

            return (fileAttributes & FileAttributes.Directory) != 0;
        }

        private string DetermineFileType(string extension)
        {
            if (FileExtensions.ImageExtensions.Contains(extension))
                return "Images";
            if (FileExtensions.ZipExtensions.Contains(extension))
                return "Zippables";
            if (FileExtensions.InstallerExtensions.Contains(extension))
                return "Installers";
            if (FileExtensions.BookExtensions.Contains(extension))
                return "Books";
            if (FileExtensions.DocumentExtensions.Contains(extension))
                return "Documents";
            if (FileExtensions.VideoExtensions.Contains(extension))
                return "Videos";
            if (extension == ".torrent")
                return "Torrents";
            return "Various";
        }
    }
}