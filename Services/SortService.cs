using FileSorter.Handlers;
using System;
using System.IO;
using System.Linq;

namespace FileSorter.Services
{
    public class SortService
    {
        private readonly FileHandler _fileHandler;
        private readonly string _basePath;

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
            var paths = Directory.GetFileSystemEntries(_basePath);

            int counter = 0;

            var files = paths.Where(x => (!IsFolder(x))).ToList();

            if (!files.Any())
                return;

            Console.WriteLine($"{files.Count} new files found. Attempting to sort.");
            foreach (var file in files)
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
    }
}