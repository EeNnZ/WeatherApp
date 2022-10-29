using System.Net;
using System.Net.Http.Headers;
using WeatherCore.Interfaces;
using WeatherCore.Models;

namespace WeatherCore.Providers
{
    public static class UrlProvider
    {
        public static string GetWeatherUrl(float lat, float lon)
        {
            if (lat != 0.0 || lon != 0.0)
            {
                string url = $"{ConfigHelper.WeatherBaseUri}lat={lat}&lon={lon}&appid={ConfigHelper.OwAuthKey}&units=metric";
                return url;
            }
            else { throw new Exception("location is not specified"); }
        }
        public static string GetForecastUrl(float lat, float lon)
        {
            if (lat != 0.0 || lon != 0.0)
            {
                string url = $"{ConfigHelper.ForecasBasetUri}lat={lat}&lon={lon}&appid={ConfigHelper.OwAuthKey}&units=metric";
                return url;
            }
            else { throw new Exception("location is not specified"); }
        }
        public static string GetSunUrl(float lat, float lon, string? date = null)
        {
            if (lat != 0.0 || lon != 0.0)
            {
                //          https://api.sunrise-sunset.org/json?lat=36.7201600&lng=-4.4203400&date=2022-08-14
                string url;
                if (date != null)
                {
                    var dt = DateTime.Parse(date).Date;
                    date = dt.ToString("yyyy-MM-dd");
                    url = $"{ConfigHelper.SunBaseUri}lat={lat}&lng={lon}&formatted=1&date={date}";
                    return url;
                }
                url = $"{ConfigHelper.SunBaseUri}lat={lat}&lng={lon}&formatted=1&date=today";
                return url;
            }
            else { throw new Exception("location is not specified"); };
        }

        public static string GetGeocodeUrl(string requestedCity)
        {
            string url = $"{ConfigHelper.GeocodeBaseUri}q={requestedCity}&limit=5&appid={ConfigHelper.OwAuthKey}";
            return url;
        }
        public static string GetIpLocationUrl(string ip)
        {
            string url = $"{ConfigHelper.IpBaseUri}{ip}";
            return url;
        }
    }
}
