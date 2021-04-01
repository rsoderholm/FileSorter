using FileSorter.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileSorter
{
    internal class Program
    {
        public static string BasePath = $"C:\\Users\\{Environment.UserName}\\Downloads";

        private static void Main(string[] args)
        {
            Sort();
            RunWatcher();
        }

        private static void RunWatcher()
        {
            using var watcher = new FileSystemWatcher(BasePath);
            watcher.Filter = "";
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
            watcher.Created += OnCreated;
            Console.WriteLine("Watching for files. Press enter to exit:");
            Console.ReadLine();
        }

        private static void Sort()
        {
            var files = Directory.GetFileSystemEntries(BasePath);

            int counter = 0;

            var fileCounter = files.Count(x => !IsFolder(x));
            Console.WriteLine($"{fileCounter} new files found. Attempting to sort.");
            foreach (var file in files)
            {
                var fileName = file.Substring(BasePath.Length + 1);
                var extension = Path.GetExtension(fileName);

                if (IsFolder(file))
                {
                    continue;
                }

                var fileType = DetermineFileType(extension);

                var directory = CreateOrUpdateDirectory(fileType);

                if (FileExistsInDirectory(directory, fileName))
                {
                    Console.WriteLine($"{fileName} exists already, deleting instead");
                    DeleteFile(file);
                    return;
                }

                try
                {
                    Console.WriteLine($"Moving {fileName} to {directory}");
                    File.Move(Path.Combine(BasePath, fileName), Path.Combine(directory, fileName));
                    counter++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Couldn't move {fileName} to {directory}");
                }
            }

            Console.WriteLine($"Moved {counter} files in total.");
        }

        private static void DeleteFile(string path)
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

        private static bool FileExistsInDirectory(string directoryPath, string file)
        {
            var fileNames = Directory.GetFiles(directoryPath).Select(x => Path.GetFileName(x));

            return fileNames.Contains(file);
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(500);
            if (e.FullPath.EndsWith(".tmp")) return;

            if (e.ChangeType != WatcherChangeTypes.Created) return;
            try
            {
                Sort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static bool IsFolder(string path)
        {
            var fileAttributes = File.GetAttributes(path);

            return (fileAttributes & FileAttributes.Directory) != 0;
        }

        public static string CreateOrUpdateDirectory(string extension)
        {
            if (!Directory.Exists($"{BasePath}\\{extension}"))
            {
                try
                {
                    Directory.CreateDirectory($"{BasePath}\\{extension}");
                    return $"{BasePath}\\{extension}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when creating directory");
                }
            }

            return $"{BasePath}\\{extension}";
        }

        private static string DetermineFileType(string extension)
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