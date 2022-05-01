using Fries.Models.FilesStorage;

namespace Fries.Models.Requests.FilesStorage
{
    public class StoreFileRequest
    {
        public string DestinationFolder { get; set; }

        public FileContent FileContent { get; set; }
    }
}
