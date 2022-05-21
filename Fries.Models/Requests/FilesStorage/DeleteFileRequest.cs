namespace Fries.Models.Requests.FilesStorage
{
    public class DeleteFileRequest
    {
        public string Path { get; set; }
        public bool? IsFile { get; set; }
    }
}
