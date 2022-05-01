using Fries.Models.LoggingService;

namespace Fries.Models.Requests.LoggingService
{
    public class GetOutermostLogsRequest : GetLogsRequest
    {
        public OutermostLogsFilterModel FilterModel { get; set; }
    }
}
