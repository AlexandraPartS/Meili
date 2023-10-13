namespace backend.Interfaces
{
    public interface IFileService
    {
        Task WriteFileAsync(long id, IFormFile file);
        void CleanFolderOfAvatar(long id);
        Task CreateIdUserFolderAsync(long id);
        Task DeleteIdUserFolderAsync(long id);
    }
}
