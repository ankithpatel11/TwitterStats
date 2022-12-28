using Microsoft.Extensions.Configuration;

namespace TwitterStatistics.Shared.Data.Helpers
{
    public static class ApplicationSettings
    {
        public static string GetAppSettings(string config)
        {
            var configuration = new ConfigurationBuilder()
                               .AddJsonFile("appsettings.json", false, true)
                               .Build();
            var result = configuration.GetSection("AppSettings").GetValue<string>(config);
            return result;
        }

    }
}
