using Fries.Models.LoggingService;

namespace Fries.Models.Requests.LoggingService
{
    public class InsertLogsRequest
    {
        public IEnumerable<LoggingModel> Logs { get; set; }
        public string CollectionName { get; set; }
    }
}
