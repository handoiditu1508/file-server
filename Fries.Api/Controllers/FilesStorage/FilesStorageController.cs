using Fries.Models.Requests.FilesStorage;
using Fries.Services.Abstractions.FilesUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fries.Api.Controllers.FilesStorage
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesStorageController : ControllerBase
    {
        private readonly IFilesStorageService _filesStorageService;

        public FilesStorageController(IFilesStorageService filesStorageService)
        {
            _filesStorageService = filesStorageService;
        }

        [HttpPost]
        [Route(nameof(StoreFiles))]
        public async Task<IActionResult> StoreFiles(StoreFileRequest request)
        {
            try
            {
                await _filesStorageService.StoreFiles(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route(nameof(DeleteFiles))]
        public IActionResult DeleteFiles(DeleteFilesRequest request)
        {
            try
            {
                _filesStorageService.DeleteFiles(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(string path)
        {
            try
            {
                var data = await _filesStorageService.GetFile(path);
                return File(data, "application/octet-stream");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
