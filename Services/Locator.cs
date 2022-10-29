using System.Net;
using WeatherCore.Interfaces;
using WeatherCore.Models;
using WeatherCore.Providers;

namespace WeatherCore.Services
{
    public static class Locator
    {
        public static async Task<LocationStruct> TryGetLocationByCityNameAsync(string requestedCity)
        {
            var geocodes = await GetGeocodeModelsAsync(requestedCity);
#if !DEBUG
            var target = geocodes.First(x => x.country.Equals("ru", StringComparison.InvariantCultureIgnoreCase));
#else
            var target = geocodes.First();
#endif
            if (target != null) { return new LocationStruct(target.lat, target.lon, target.name); }
            else throw new Exception("Cannot find this place, try again");
        }
        public static async Task<LocationStruct> GetLocationByIpAsync()
        {
            var model = await GetIpLocationModelAsync();
            return new LocationStruct(model.lat, model.lon, model.city);
        }
        private static async Task<IpLocationModel> GetIpLocationModelAsync()
        {
            string ipString = await GetIp();
            string url = UrlProvider.GetIpLocationUrl(ipString);
            var content = await HttpHelper.GetContentAsync(url);
            var model = await content.ReadAsAsync<IpLocationModel>();
            return model;
        }
        private static async Task<List<GeocodeModel>> GetGeocodeModelsAsync(string requestedCity)
        {
            string url = UrlProvider.GetGeocodeUrl(requestedCity);
            var content = await HttpHelper.GetContentAsync(url);
            var geocodes = await content.ReadAsAsync<List<GeocodeModel>>();
            return geocodes;
        }
        private static async Task<string> GetIp()
        {
            var response = await HttpHelper.Client.GetAsync("http://icanhazip.com/");
            response.EnsureSuccessStatusCode();
            string ip = await response.Content.ReadAsStringAsync();
            return ip;
        }
    }
}
