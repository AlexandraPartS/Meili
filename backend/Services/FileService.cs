using backend.Infrastructure;
using backend.Interfaces;

namespace backend.Services
{
    public class FileService : IFileService
    {

        public void CleanFolderOfFiles(string path)
        {
            var filesPathList = Directory.GetFiles(path);

            if (filesPathList.Any())
            {
                foreach (var filePath in filesPathList)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    fileInfo.Delete();
                }
            }
        }

        public async Task WriteFile(IFormFile uploadedFile)
        {
            using (var stream = uploadedFile.OpenReadStream())
            {
                using (var img = await Image.LoadAsync(stream))
                {
                    await img.SaveAsync(GlobalVariables.AvatarAbsolutePath);
                }
            }
        }

        public async Task WriteFiles(IFormFile[] files)
        {

        }
    }
}