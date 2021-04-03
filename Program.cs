using FileSorter.Handlers;
using System;
using FileSorter.Services;

namespace FileSorter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var basePath = $"C:\\Users\\{Environment.UserName}\\Downloads";
            FileHandler fileHandler = new FileHandler(basePath);
            SortService sortService = new SortService(fileHandler, basePath);
            sortService.Sort();
        }
    }
}