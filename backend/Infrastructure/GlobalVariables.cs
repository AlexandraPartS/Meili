namespace backend.Infrastructure
{
    public static class GlobalVariables
    {
        //App Environment
        public const string WebRootPath = "..\\frontend";
        private const string FilesFolder = "Files";
        private static string separator;
        private static readonly HttpContext httpContext;
        private static readonly IWebHostEnvironment webHostEnvironment;
        public const string RegexPhonePattern = @"^\+(?:[\s\-]?[0-9]●?){6,14}[0-9]$";

        //File storage
        public static readonly string FileStoragePath;
        //Avatar settings
        private const string AvatarNameExtensionSet = "avatar.png";

        static GlobalVariables()
        {
            separator = Path.DirectorySeparatorChar.ToString();
            httpContext = new HttpContextAccessor().HttpContext;
            webHostEnvironment = (IWebHostEnvironment)httpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
            FileStoragePath = Path.Combine( new string[]{ webHostEnvironment.WebRootPath, FilesFolder });
        }

        public static string AbsolutePathOfUserIdFolder(long id)
        {
            return Path.Combine(new string[] { FileStoragePath, id.ToString() });
        }
        public static string AvatarAbsolutePathAndName(long id)
        {
            return Path.Combine(new string[] { FileStoragePath, id.ToString(), AvatarNameExtensionSet });
        }
        public static string AvatarRelativePathAndName(long id)
        {
            return Path.Combine(new string[] { separator, FilesFolder, id.ToString(), AvatarNameExtensionSet });
        }
    }
}

