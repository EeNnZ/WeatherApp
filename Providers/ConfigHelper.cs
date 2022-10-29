using Microsoft.Extensions.Configuration;

namespace WeatherCore.Providers
{
    public static class ConfigHelper
    {
        public static string WeatherBaseUri { get; private set; } = null!;
        public static string ForecasBasetUri { get; private set; } = null!;
        public static string SunBaseUri { get; private set; } = null!;
        public static string GeocodeBaseUri { get; private set; } = null!;
        public static string IpBaseUri { get; private set; } = null!;
        public static string OwAuthKey { get; private set; } = null!;
        public static string TempFilename { get; private set; } = null!;

        public static void BuildConfig()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            OwAuthKey = config.GetSection("ow_auth_key").Value;
            GeocodeBaseUri = config.GetSection("geocode_uri").Value;
            IpBaseUri = config.GetSection("ip_location_uri").Value;
            WeatherBaseUri = config.GetSection("weather_uri").Value;
            ForecasBasetUri = config.GetSection("forecast_uri").Value;
            SunBaseUri = config.GetSection("sun_uri").Value;
            TempFilename = config.GetSection("temp_filename").Value;
        }
    }
}
