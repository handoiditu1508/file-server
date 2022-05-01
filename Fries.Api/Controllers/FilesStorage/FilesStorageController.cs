﻿using Fries.Helpers.Extensions;
using Fries.Models.Common;
using Fries.Models.Requests.FilesStorage;
using Fries.Services.Abstractions.FilesUpload;
using Microsoft.AspNetCore.Mvc;

namespace Fries.Api.Controllers.FilesStorage
{
    [Route("api/[controller]")]
    [ApiController]
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
        /// <param name="destinationFolder">Folder to store files.</param>
        /// <param name="fileContent.data">File's bytes array.</param>
        /// <param name="fileContent.fileName">File's name.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(nameof(StoreFile))]
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
        /// <param name="destinationFolder">Folder to store files.</param>
        /// <param name="fileContents">List of file contents.</param>
        /// <param name="fileContents.data">File's bytes array.</param>
        /// <param name="fileContents.fileName">File's name.</param>
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
        [HttpDelete]
        [Route("{path}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleError), StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteFile(string path)
        {
            try
            {
                _filesStorageService.DeleteFile(path);
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
        /// <param name="paths">Folder or file paths to delete.</param>
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
