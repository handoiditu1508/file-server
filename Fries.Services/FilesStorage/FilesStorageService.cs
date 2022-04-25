using Fries.Helpers;
using Fries.Models.Requests.FilesStorage;
using Fries.Services.Abstractions.FilesUpload;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fries.Services.FilesStorage
{
    public class FilesStorageService : IFilesStorageService
    {
        private readonly string _rootPath;
        //private readonly ILogger<FilesUploadService> _logger;

        public FilesStorageService()
        {
            var homePath = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ?
                Environment.GetEnvironmentVariable("HOME") :
                Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            _rootPath = Path.Combine(homePath, "my-files-storage");
        }

        public async Task StoreFiles(StoreFileRequest request)
        {
            Directory.CreateDirectory(_rootPath);

            var tasks = request.FileContents.Select(fileContent =>
            {
                var filePath = Path.Combine(_rootPath, fileContent.FileName);

                return File.WriteAllBytesAsync(filePath, fileContent.Data);
            });

            await Task.WhenAll(tasks);
        }

        public void DeleteFiles(DeleteFilesRequest request)
        {
            foreach(var path in request.Paths)
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
                    //_logger.LogError();
                }
            }
        }

        public async Task<byte[]> GetFile(string path)
        {
            var fullPath = Path.Combine(_rootPath, path);

            if(File.Exists(fullPath))
            {
                var data = await File.ReadAllBytesAsync(fullPath);
                return data;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
