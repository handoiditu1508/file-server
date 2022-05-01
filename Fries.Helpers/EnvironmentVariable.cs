namespace Fries.Helpers
{
    public static class EnvironmentVariable
    {
        public static string AspNetCoreEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public static string ApiKeyValue => Environment.GetEnvironmentVariable("ApiKeyValue");
    }
}
