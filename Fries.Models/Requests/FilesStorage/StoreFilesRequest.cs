using Fries.Models.FilesStorage;

namespace Fries.Models.Requests.FilesStorage
{
    public class StoreFilesRequest
    {
        public string DestinationFolder { get; set; }

        public IEnumerable<FileContent> FileContents { get; set; }
    }
}
