using FileSorter.Handlers;
using FileSorter.Services;
using Logging;
using System;

namespace FileSorter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //var basePath = $"C:\\Users\\{Environment.UserName}\\Downloads";
            var logger = new FileLogger();
            IFileHandler fileHandler = new FileHandler(basePath);
            ISortService sortService = new SortService(fileHandler, basePath, logger);
            sortService.Sort();
        }
    }
}