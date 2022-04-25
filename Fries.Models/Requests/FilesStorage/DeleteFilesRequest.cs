using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fries.Models.Requests.FilesStorage
{
    public class DeleteFilesRequest
    {
        public IEnumerable<string> Paths { get; set; }
    }
}
