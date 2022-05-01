namespace Fries.Models.LoggingService
{
    public class NearbyLogsFilterModel : LogsFilterModel
    {
        public string? PivotId { get; set; }
        public bool GetAfterPivotId { get; set; } = false;
    }
}
