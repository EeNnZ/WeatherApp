using System.Net;
using WeatherCore.Interfaces;
using WeatherCore.Models;
using WeatherCore.Providers;

namespace WeatherCore.Services
{
    public static class Locator
    {
        public static async Task<LocationStruct> TryGetLocationByCityNameAsync(string requestedCity, CancellationToken token)
        {
            var geocodes = await GetGeocodeModelsAsync(requestedCity, token);
#if !DEBUG
            var target = geocodes.First(x => x.country.Equals("ru", StringComparison.InvariantCultureIgnoreCase));
#else
            var target = geocodes.First();
#endif
            if (target != null) { return new LocationStruct(target.lat, target.lon, target.name); }
            else throw new Exception("Cannot find this place, try again");
        }
        public static async Task<LocationStruct> GetLocationByIpAsync(CancellationToken token)
        {
            var model = await GetIpLocationModelAsync(token);
            return new LocationStruct(model.lat, model.lon, model.city);
        }
        private static async Task<IpLocationModel> GetIpLocationModelAsync(CancellationToken token)
        {
            string ipString = await GetIp(token);
            string url = UrlProvider.GetIpLocationUrl(ipString);
            var content = await HttpHelper.GetContentAsync(url, token);
            var model = await content.ReadAsAsync<IpLocationModel>();
            return model;
        }
        private static async Task<List<GeocodeModel>> GetGeocodeModelsAsync(string requestedCity, CancellationToken token)
        {
            string url = UrlProvider.GetGeocodeUrl(requestedCity);
            var content = await HttpHelper.GetContentAsync(url, token);
            var geocodes = await content.ReadAsAsync<List<GeocodeModel>>(token);
            return geocodes;
        }
        private static async Task<string> GetIp(CancellationToken token)
        {
            var response = await HttpHelper.Client.GetAsync("http://icanhazip.com/", token);
            response.EnsureSuccessStatusCode();
            string ip = await response.Content.ReadAsStringAsync(token);
            return ip;
        }
    }
}
