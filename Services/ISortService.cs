namespace FileSorter.Services
{
    public interface ISortService
    {
        string DefaultBasePath();

        void Sort();
    }
}