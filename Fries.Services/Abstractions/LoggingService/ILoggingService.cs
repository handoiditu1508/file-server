using Fries.Models.Requests.LoggingService;
using Fries.Models.Responses.LoggingService;

namespace Fries.Services.Abstractions.LoggingService
{
    public interface ILoggingService
    {
        Task<GetLogsResponse> GetOutermostLogs(GetOutermostLogsRequest request);
        Task<GetLogsResponse> GetNearbyLogs(GetNearbyLogsRequest request);
        Task DeleteCollection(string collectionName);
        Task InsertLogs(InsertLogsRequest request);
        Task InsertLog(InsertLogRequest request);
        Task<IEnumerable<string>> GetCollectionNames();
    }
}
