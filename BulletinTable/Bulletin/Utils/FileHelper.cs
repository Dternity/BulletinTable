namespace BulletinTable.Utils
{
    public static class FileHelper
    {
        public const string FolderSlasher = "/";
        public static string GetFullPath(string filename, string path, string ext)
        {
            return path + FolderSlasher + filename + ext;
        } 
    }
}
