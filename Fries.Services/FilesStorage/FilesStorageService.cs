using Fries.Helpers;
using Fries.Helpers.Extensions;
using Fries.Models.Requests.FilesStorage;
using Fries.Services.Abstractions.FilesUpload;
using Microsoft.Extensions.Logging;

namespace Fries.Services.FilesStorage
{
    public class FilesStorageService : IFilesStorageService
    {
        private readonly string _rootPath;
        private readonly ILogger<FilesStorageService> _logger;

        public FilesStorageService(ILogger<FilesStorageService> logger)
        {
            var homePath = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ?
                Environment.GetEnvironmentVariable("HOME") :
                Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            _rootPath = Path.Combine(homePath, "my-files-storage");
            _logger = logger;
        }

        public async Task StoreFile(StoreFileRequest request)
        {
            if (request == null)
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request));

            if (request.FileContent == null)
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.FileContent));

            if (request.DestinationFolder == null)
                request.DestinationFolder = string.Empty;

            Directory.CreateDirectory(_rootPath);

            var filePath = Path.Combine(_rootPath, request.DestinationFolder, request.FileContent.FileName);

            await File.WriteAllBytesAsync(filePath, request.FileContent.Data);
        }

        public async Task StoreFiles(StoreFilesRequest request)
        {
            if (request == null)
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request));

            if (request.FileContents.IsNullOrEmpty())
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.FileContents));

            if (request.DestinationFolder == null)
                request.DestinationFolder = string.Empty;

            Directory.CreateDirectory(_rootPath);

            var tasks = request.FileContents.Select(async fileContent =>
            {
                try
                {
                    var filePath = Path.Combine(_rootPath, request.DestinationFolder, fileContent.FileName);

                    await File.WriteAllBytesAsync(filePath, fileContent.Data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, string.Empty);
                }
            });

            await Task.WhenAll(tasks);
        }

        public void DeleteFile(string path, bool? isFile = null)
        {
            if (path.IsNullOrWhiteSpace())
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(path));

            var fullPath = Path.Combine(_rootPath, path);

            if (!isFile.HasValue)
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                else if (Directory.Exists(fullPath))
                {
                    Directory.Delete(fullPath, true);
                }
            }
            else
            {
                if (isFile.Value && File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                else if (Directory.Exists(fullPath))
                {
                    Directory.Delete(fullPath, true);
                }
            }
        }

        public void DeleteFiles(DeleteFilesRequest request)
        {
            if (request.Files.IsNullOrEmpty())
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.Files));

            foreach (var file in request.Files)
            {
                try
                {
                    DeleteFile(file.Path, file.IsFile);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, string.Empty);
                }
            }
        }

        public async Task<byte[]> GetFile(string path)
        {
            var fullPath = Path.Combine(_rootPath, path);

            if (File.Exists(fullPath))
            {
                var data = await File.ReadAllBytesAsync(fullPath);
                return data;
            }
            else
            {
                throw CustomException.FilesStorage.FileNotFound(path);
            }
        }
    }
}
