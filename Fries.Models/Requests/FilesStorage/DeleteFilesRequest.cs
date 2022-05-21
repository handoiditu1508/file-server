namespace Fries.Models.Requests.FilesStorage
{
    public class DeleteFilesRequest
    {
        public IEnumerable<DeleteFileRequest> Files { get; set; }
    }
}
