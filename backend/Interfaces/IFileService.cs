namespace backend.Interfaces
{
    public interface IFileService
    {
        Task WriteFile(IFormFile file);
        Task WriteFiles(IFormFile[] files);
        void CleanFolderOfFiles(string path);
    }
}
