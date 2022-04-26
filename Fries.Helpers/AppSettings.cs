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
    }
}
