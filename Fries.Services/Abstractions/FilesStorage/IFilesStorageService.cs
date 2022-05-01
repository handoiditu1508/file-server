using Fries.Models.Requests.FilesStorage;

namespace Fries.Services.Abstractions.FilesUpload
{
    public interface IFilesStorageService
    {
        Task StoreFile(StoreFileRequest request);
        Task StoreFiles(StoreFilesRequest request);
        void DeleteFile(string path);
        void DeleteFiles(DeleteFilesRequest request);
        Task<byte[]> GetFile(string path);
    }
}
