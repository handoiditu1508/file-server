using Fries.Models.Requests.FilesStorage;

namespace Fries.Services.Abstractions.FilesUpload
{
    public interface IFilesStorageService
    {
        Task StoreFiles(StoreFileRequest request);
        void DeleteFiles(DeleteFilesRequest request);
        Task<byte[]> GetFile(string path);
    }
}
