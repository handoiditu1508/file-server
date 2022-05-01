using Fries.Models.LoggingService;

namespace Fries.Models.Responses.LoggingService
{
    public class GetLogsResponse
    {
        public IEnumerable<LoggingModel> Logs { get; set; }
        public long Size { get; set; }
        public long MaxSize { get; set; }
        public int Count { get; set; }
        public int Max { get; set; }
    }
}
