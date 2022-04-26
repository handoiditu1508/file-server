namespace Fries.Helpers
{
    public static class FileHelper
    {
        public static bool IsDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);

            return attr.HasFlag(FileAttributes.Directory);
        }
    }
}
