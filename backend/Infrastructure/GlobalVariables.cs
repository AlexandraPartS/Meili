namespace backend.Infrastructure
{
    public static class GlobalVariables
    {
        //App Environment
        public const string WebRootPath = "..\\frontend";
        private static readonly HttpContext httpContext;
        private static readonly IWebHostEnvironment webHostEnvironment;

        //File storage
        public static readonly string FileStoragePath;
        //Avatar settings
        private const string avatarNameExtensionSet = "avatar.png";
        public static readonly string AvatarRelativePath = Path.Combine(new string[] { "/Files/", avatarNameExtensionSet }); //For DB field
        public static readonly string AvatarAbsolutePath;

        static GlobalVariables()
        {
            httpContext = new HttpContextAccessor().HttpContext;
            webHostEnvironment = (IWebHostEnvironment)httpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
            FileStoragePath = Path.Combine( new string[]{ webHostEnvironment.WebRootPath, "Files"});
            AvatarAbsolutePath = Path.Combine(new string[] { FileStoragePath, avatarNameExtensionSet });
        }
    }
}


