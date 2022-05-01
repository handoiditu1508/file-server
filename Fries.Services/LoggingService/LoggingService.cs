﻿using Fries.Helpers;
using Fries.Helpers.Abstractions;
using Fries.Models.Requests.LoggingService;
using Fries.Models.Responses.LoggingService;
using Fries.Services.Abstractions.LoggingService;

namespace Fries.Services.LoggingService
{
    public class LoggingService : ILoggingService
    {
        private readonly IHttpHelper _httpHelper;

        public LoggingService(IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
            _httpHelper.SetBaseUrl(AppSettings.LoggingService.BaseUrl);
        }

        /// <summary>
        /// Get Latest or Oldest logs.
        /// </summary>
        /// <param name="collectionName">Collection name.</param>
        /// <param name="limit">Maximum logs to retrieve.</param>
        /// <param name="filterModel.group">Group that logs belong to.</param>
        /// <param name="filterModel.logLevels">Critical level of the logs.</param>
        /// <param name="filterModel.startDate">Logs range from.</param>
        /// <param name="filterModel.endDate">Logs range to.</param>
        /// <param name="filterModel.latest">Get by latest logs or oldest log.</param>
        /// <returns>List of logs along with total collection size and count.</returns>
        public async Task<GetLogsResponse> GetOutermostLogs(GetOutermostLogsRequest request)
        {
            GetLogsResponse result = null;

            try
            {
                result = await _httpHelper.Post<GetLogsResponse>("api/MongoLogging/GetOutermostLogs", request);
            }
            catch
            { }

            return result;
        }

        /// <summary>
        /// Get Latest or Oldest logs.
        /// </summary>
        /// <param name="collectionName">Collection name.</param>
        /// <param name="limit">Maximum logs to retrieve.</param>
        /// <param name="filterModel.group">Group that logs belong to.</param>
        /// <param name="filterModel.logLevels">Critical level of the logs.</param>
        /// <param name="filterModel.startDate">Logs range from.</param>
        /// <param name="filterModel.endDate">Logs range to.</param>
        /// <param name="filterModel.pivotId">Id of the pivot log.</param>
        /// <param name="filterModel.getAfterPivotId">Get logs after the pivot log or before the pivot log.</param>
        /// <returns>List of logs along with total collection size and count.</returns>
        public async Task<GetLogsResponse> GetNearbyLogs(GetNearbyLogsRequest request)
        {
            GetLogsResponse result = null;

            try
            {
                result = await _httpHelper.Post<GetLogsResponse>("api/MongoLogging/GetNearbyLogs", request);
            }
            catch
            { }

            return result;
        }

        /// <summary>
        /// Delete log collection by name.
        /// </summary>
        /// <param name="collectionName">Name of the collection to be deleted.</param>
        public async Task DeleteCollection(string collectionName)
        {
            try
            {
                await _httpHelper.Delete($"api/MongoLogging/DeleteCollection/{collectionName}");
            }
            catch
            { }
        }

        /// <summary>
        /// Insert logs into collection.
        /// </summary>
        /// <param name="logs.id">Not required, auto generated!</param>
        /// <param name="logs.createdDate">Not required, auto generated!</param>
        /// <param name="logs.logLevel">Critical level of the logs.</param>
        /// <param name="logs.message">Message of the log.</param>
        /// <param name="logs.stackTrace">Exception stack trace if any.</param>
        /// <param name="logs.source">Exception source if any.</param>
        /// <param name="logs.group">Group that logs belong to.</param>
        /// <param name="logs.code">Exception code.</param>
        /// <param name="collectionName">Name of the collection.</param>
        public async Task InsertLogs(InsertLogsRequest request)
        {
            try
            {
                await _httpHelper.Post("api/MongoLogging/InsertLogs", request);
            }
            catch
            { }
        }

        /// <summary>
        /// Insert log into collection.
        /// </summary>
        /// <param name="log.id">Not required, auto generated!</param>
        /// <param name="log.createdDate">Not required, auto generated!</param>
        /// <param name="log.logLevel">Critical level of the logs.</param>
        /// <param name="log.message">Message of the log.</param>
        /// <param name="log.stackTrace">Exception stack trace if any.</param>
        /// <param name="log.source">Exception source if any.</param>
        /// <param name="log.group">Group that logs belong to.</param>
        /// <param name="log.code">Exception code.</param>
        /// <param name="collectionName">Name of the collection.</param>
        public async Task InsertLog(InsertLogRequest request)
        {
            try
            {
                await _httpHelper.Post("api/MongoLogging/InsertLog", request);
            }
            catch
            { }
        }

        /// <summary>
        /// Get all collection names.
        /// </summary>
        /// <returns>List of string for collection names.</returns>
        public async Task<IEnumerable<string>> GetCollectionNames()
        {
            IEnumerable<string> result = null;

            try
            {
                result = await _httpHelper.Get<IEnumerable<string>>("api/MongoLogging/GetCollectionNames");
            }
            catch
            { }

            return result;
        }
    }
}
