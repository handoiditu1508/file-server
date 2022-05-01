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

        public void DeleteFile(string path)
        {
            if (path.IsNullOrWhiteSpace())
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(path));

            var fullPath = Path.Combine(_rootPath, path);

            if (FileHelper.IsDirectory(path))
            {
                Directory.Delete(fullPath, true);
            }
            else
            {
                File.Delete(fullPath);
            }
        }

        public void DeleteFiles(DeleteFilesRequest request)
        {
            if (request.Paths.IsNullOrEmpty())
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.Paths));

            foreach (var path in request.Paths)
            {
                try
                {
                    var fullPath = Path.Combine(_rootPath, path);

                    if (FileHelper.IsDirectory(path))
                    {
                        Directory.Delete(fullPath, true);
                    }
                    else
                    {
                        File.Delete(fullPath);
                    }
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
