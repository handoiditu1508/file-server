namespace Fries.Models.Requests.FilesStorage
{
    public class DeleteFilesRequest
    {
        public IEnumerable<string> Paths { get; set; }
    }
}
