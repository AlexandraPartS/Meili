using backend.Infrastructure;
using backend.Interfaces;
using System.IO;

namespace backend.Services
{
    public class FileService : IFileService
    {
        public async Task CreateIdUserFolderAsync(long id)
        {
            String folder = GlobalVariables.AbsolutePathOfUserIdFolder(id);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public async Task DeleteIdUserFolderAsync(long id)
        {
            String folder = GlobalVariables.AbsolutePathOfUserIdFolder(id);
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder);
            }
        }

        public void CleanFolderOfAvatar(long id)
        {
            String folder = GlobalVariables.AbsolutePathOfUserIdFolder(id);
            var filesPathList = Directory.GetFiles(folder);
            if (filesPathList.Any())
            {
                foreach (var filePath in filesPathList)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    fileInfo.Delete();
                }
            }
            if (Directory.GetDirectories(folder).Length == 0)
            {
                Directory.Delete(folder);
            }
        }

        public async Task WriteFileAsync(long id, IFormFile uploadedFile)
        {
            using (var stream = uploadedFile.OpenReadStream())
            {
                using (var img = await Image.LoadAsync(stream))
                {
                    await img.SaveAsync(GlobalVariables.AvatarAbsolutePathAndName(id));
                }
            }
        }

    }
}