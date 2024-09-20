using CVRecognizingService.Domain.Entities;

namespace CVRecognizingService.Infrastructure.DataAccess.DBMaps
{
    public static class DataBaseMap
    {
        public static Dictionary<Type, KeyValuePair<string, string>> Map = new Dictionary<Type, KeyValuePair<string, string>>()
        {
            { typeof(BaseDocument), new KeyValuePair<string, string>("Documents", "Documents") },
            { typeof(ProcessedData), new KeyValuePair<string, string>("Documents", "ProcessedData") },
            { typeof(ProcessingStatus), new KeyValuePair<string, string>("Documents", "ProcessingStatus") },
            { typeof(ProcessingLog), new KeyValuePair<string, string>("Documents", "ProcessingLogs") },
        };
    };
}
