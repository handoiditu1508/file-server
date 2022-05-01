using Fries.Models.LoggingService;

namespace Fries.Models.Requests.LoggingService
{
    public class GetNearbyLogsRequest : GetLogsRequest
    {
        public NearbyLogsFilterModel FilterModel { get; set; }
    }
}
