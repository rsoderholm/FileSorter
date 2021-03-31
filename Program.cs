using System;
using System.IO;
using System.Linq;

namespace FileSorter
{
    internal class Program
    {
        public static string BasePath = $"C:\\Users\\{Environment.UserName}\\Downloads";

        private static void Main(string[] args)
        {
            RunWatcher();
        }

        private static void RunWatcher()
        {
            using var watcher = new FileSystemWatcher(BasePath);
            watcher.Filter = "";
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
            watcher.Changed += OnChanged;
            //watcher.Created += OnCreated;
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
                var filePath = file.Substring(BasePath.Length + 1);
                var extension = Path.GetExtension(filePath);

                if (IsFolder(file))
                {
                    continue;
                }

                var fileType = DetermineFileType(extension);

                var directory = CreateOrUpdateDirectory(fileType);

                try
                {
                    Console.WriteLine($"Moving {filePath} to {directory}");
                    File.Move(Path.Combine(BasePath, filePath), Path.Combine(directory, filePath));
                    counter++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Couldn't move {filePath} to {directory}");
                }
            }

            Console.WriteLine($"Moved {counter} files in total.");
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.EndsWith(".tmp"))
                return;

            if (e.ChangeType != WatcherChangeTypes.Changed)
                return;

            Console.WriteLine($"Changes for {e.Name}");
            Sort();
        }

        //private static void OnCreated(object sender, FileSystemEventArgs e)
        //{
        //    if (e.Name.Contains(".tmp"))
        //        return;

        //    if (e.ChangeType != WatcherChangeTypes.Created)
        //        return;
        //    Console.WriteLine("File created");
        //    Sort();
        //}

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
            var zipExtensions = new string[] { ".zip", ".rar", ".7z" };
            var imageExtensions = new string[] { ".png", ".jpg", ".gif", ".jpeg", ".eps", ".bmp", ".tif", ".tiff" };
            var installerExtensions = new string[] { ".exe", ".msi" };
            var bookExtensions = new string[] { ".epub", ".mobi", ".pdf" };
            var textExtensions = new string[] { ".txt", ".doc", ".docx", ".rtf" };
            var videoExtensions = new string[] { ".avi", ".mp4", ".mpeg", ".mpg" };

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
            if (extension == ".torrent")
                return "Torrents";
            return "Various";
        }
    }
}