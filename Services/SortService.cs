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
            var files = Directory.GetFiles(_basePath).ToList();

            int counter = 0;

            if (!files.Any())
                return;

            Console.WriteLine($"{files.Count} new files found. Attempting to sort.");
            foreach (var file in files)
            {
                var fileName = file.Substring(_basePath.Length + 1);
                var fileExtension = Path.GetExtension(fileName);

                var fileType = _fileHandler.DetermineFolderByExtension(fileExtension);

                var saveDirectory = _fileHandler.CreateOrUpdateDirectory(fileType);

                if (_fileHandler.FileExistsInDirectory(saveDirectory, fileName))
                {
                    Console.WriteLine($"{fileName} already exists, deleting file instead");
                    _fileHandler.Delete(file);
                    return;
                }

                try
                {
                    Console.WriteLine($"Moving {fileName} to {saveDirectory}");
                    _fileHandler.Move(Path.Combine(_basePath, fileName), Path.Combine(saveDirectory, fileName));
                    counter++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Couldn't move {fileName} to {saveDirectory}");
                }
            }

            Console.WriteLine($"Moved {counter} files in total.");
        }
    }
}