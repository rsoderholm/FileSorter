using FileSorter.Handlers;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace FileSorter.Services
{
    public class SortService : ISortService
    {
        private readonly IFileHandler _fileHandler;
        private readonly string _basePath;
        private readonly ILogger _logger;

        public SortService(IFileHandler fileHandler, string basePath, ILogger logger)
        {
            _fileHandler = fileHandler;
            _basePath = basePath;
            _logger = logger;
        }

        public string DefaultBasePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //return $"C:\\Users\\{Environment.UserName}\\Downloads";
        }

        public void Sort()
        {
            try
            {
                var files = Directory.GetFiles(_basePath).ToList();

                int fileCounter = 0;

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
                        continue;
                    }

                    try
                    {
                        Console.WriteLine($"Moving {fileName} to {saveDirectory}");

                        _fileHandler.Move(Path.Combine(_basePath, fileName), Path.Combine(saveDirectory, fileName));
                        fileCounter++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                Console.WriteLine($"Moved {fileCounter} files in total.");
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }
        }
    }
}