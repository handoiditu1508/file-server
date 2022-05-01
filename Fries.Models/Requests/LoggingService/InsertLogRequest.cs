using Fries.Models.LoggingService;

namespace Fries.Models.Requests.LoggingService
{
    public class InsertLogRequest
    {
        public LoggingModel Log { get; set; }
        public string CollectionName { get; set; }
    }
}
