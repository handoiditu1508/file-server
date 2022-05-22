using Fries.Api.Attributes;
using Fries.Helpers.Extensions;
using Fries.Models.Common;
using Fries.Models.Requests.FilesStorage;
using Fries.Services.Abstractions.FilesUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fries.Api.Controllers.FilesStorage
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuthenticate]
    public class FilesStorageController : ControllerBase
    {
        private readonly IFilesStorageService _filesStorageService;
        private readonly ILogger<FilesStorageController> _logger;

        public FilesStorageController(IFilesStorageService filesStorageService, ILogger<FilesStorageController> logger)
        {
            _filesStorageService = filesStorageService;
            _logger = logger;
        }

        /// <summary>
        /// Store single file.
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     {
        ///        "destinationFolder": "user/123",// Folder to store files.
        ///        "fileContent": {
        ///           "data": [],// File's bytes array.
        ///           "fileName": "avatar.png"// File's name.
        ///        }
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StoreFile(StoreFileRequest request)
        {
            try
            {
                await _filesStorageService.StoreFile(request);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Store multiple files.
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     {
        ///        "destinationFolder": "user/123",// Folder to store files.
        ///        "fileContents": [
        ///           {
        ///              "data": [],// File's bytes array.
        ///              "fileName": "avatar.png"// File's name.
        ///           }
        ///        ]
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route(nameof(StoreFiles))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StoreFiles(StoreFilesRequest request)
        {
            try
            {
                await _filesStorageService.StoreFiles(request);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Delete single files.
        /// </summary>
        /// <param name="path">Folder or file path to delete.</param>
        /// <param name="isFile">Specify wheather path leads to file or folder if both exist, leave empty if only one case exist.</param>
        [HttpDelete]
        [Route("{path}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteFile(string path, bool? isFile = null)
        {
            try
            {
                _filesStorageService.DeleteFile(path, isFile);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Delete multiple files.
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     {
        ///        "files": [//Folder or file paths to delete.
        ///           {
        ///              "path": "user/100/selfie.png"// File path to delete.
        ///           },
        ///           {
        ///              "path": "user/100/product",// Folder path to delete.
        ///              "isFile": false// Specify wheather path leads to file or folder if both exist, leave null if only one case exist.
        ///           }
        ///        ]
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route(nameof(DeleteFiles))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteFiles(DeleteFilesRequest request)
        {
            try
            {
                _filesStorageService.DeleteFiles(request);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToSimpleError());
            }
        }

        /// <summary>
        /// Get file.
        /// </summary>
        /// <param name="path">Path of file.</param>
        /// <returns>File base on path.</returns>
        [HttpGet]
        [Route("{path}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFile(string path)
        {
            try
            {
                var data = await _filesStorageService.GetFile(path);
                return File(data, "application/octet-stream");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToSimpleError());
            }
        }
    }
}
