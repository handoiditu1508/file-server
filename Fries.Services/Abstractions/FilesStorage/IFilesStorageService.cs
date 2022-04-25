using Fries.Models.Requests.FilesStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fries.Services.Abstractions.FilesUpload
{
    public interface IFilesStorageService
    {
        Task StoreFiles(StoreFileRequest request);
        void DeleteFiles(DeleteFilesRequest request);
        Task<byte[]> GetFile(string path);
    }
}
