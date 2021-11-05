namespace FileSorter.Handlers
{
    public interface IFileHandler
    {
        string DetermineFolderByExtension(string fileExtension);

        string CreateOrUpdateDirectory(string fileExtension);

        void Move(string from, string to);

        void Delete(string path);

        public bool FileExistsInDirectory(string directoryPath, string file);
    }
}