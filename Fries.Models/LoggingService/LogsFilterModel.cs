using Microsoft.Extensions.Logging;

namespace Fries.Models.LoggingService
{
    public class LogsFilterModel
    {
        public string? Group { get; set; }
        public IEnumerable<LogLevel>? LogLevels { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
