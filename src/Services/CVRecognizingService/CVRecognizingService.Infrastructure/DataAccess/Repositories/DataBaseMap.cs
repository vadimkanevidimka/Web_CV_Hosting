using CVRecognizingService.Domain.Entities;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories
{
    public static class DataBaseMap
    {
        public static Dictionary<Type, KeyValuePair<string, string>> Map = new Dictionary<Type, KeyValuePair<string, string>>()
        {
            { typeof(BaseDocument), new KeyValuePair<string, string>("Documents", "Documents") },
        };
    }
}
