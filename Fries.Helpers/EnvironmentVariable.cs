namespace Fries.Helpers
{
    public static class EnvironmentVariable
    {
        public static string AspNetCoreEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}
