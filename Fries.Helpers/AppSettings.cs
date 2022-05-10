using Fries.Helpers.Extensions;
using Microsoft.Extensions.Configuration;

namespace Fries.Helpers
{
    public static class AppSettings
    {
        private static IConfigurationRoot _configuration;

        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var appsettingsFileName = EnvironmentVariable.AspNetCoreEnvironment.IsNullOrWhiteSpace() ?
                        "appsettings.json" :
                        $"appsettings.{EnvironmentVariable.AspNetCoreEnvironment}.json";

                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(appsettingsFileName)
                        .Build();
                }
                return _configuration;
            }
        }

        public static class LoggingService
        {
            public static string BaseUrl => Configuration["LoggingService:BaseUrl"];
            public static string DefaultCollectionName => Configuration["LoggingService:DefaultCollectionName"];
        }

        public static class ApiKey
        {
            public static string Name => Configuration["ApiKey:Name"];
            public static string Value => Configuration["ApiKey:Value"].IsNullOrEmpty() ? EnvironmentVariable.ApiKeyValue : Configuration["ApiKey:Value"];
        }
    }
}
