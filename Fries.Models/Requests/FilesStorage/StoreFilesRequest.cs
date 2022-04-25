using Fries.Models.FilesStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fries.Models.Requests.FilesStorage
{
    public class StoreFileRequest
    {
        public string DestinationFolder { get; set; }

        public IEnumerable<FileContent> FileContents { get; set; }
    }
}
