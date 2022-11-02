using System;
using System.Net.Http.Headers;
using System.Text.Json;
using WeatherCore.Interfaces;
using WeatherCore.Models;
using WeatherCore.Providers;

namespace WeatherCore.Services
{
    public class WeatherService
    {
        private readonly IOptions _options;
        public LocationStruct Location { get; set; }
        public IProgress<double> Progress { get; private set; }
        public WeatherService(IProgress<double> progress, IOptions options)
        {
            Progress = progress;
            _options = options;
        }

        public async Task<IModel> RunAsync(CancellationToken token)
        {
            string? requiredLocation = _options.RequiredLocation;
            if (requiredLocation != null) { Location = await Locator.TryGetLocationByCityNameAsync(requiredLocation, token); }
            else { Location = await Locator.GetLocationByIpAsync(token); }

            if (_options.RequiredMode == IOptions.Mode.Now) { return await GetWeatherModelAsync(token); }
            else { return await GetForecastModelAsync(token); }
        }
        private async Task<WeatherModel> GetWeatherModelAsync(CancellationToken token)
        {
            var sunModelTask = GetSunModelAsync(token);

            string url = UrlProvider.GetWeatherUrl(Location.latitude, Location.longitude);
            var filename = ConfigHelper.TempFilename + "w";
            using (var input = new FileStream(filename, FileMode.Create))
                await HttpHelper.GetFileAsync(url, input, token, Progress);

            using var output = File.OpenRead(filename);
            var model = await JsonSerializer.DeserializeAsync<WeatherModel>(output, cancellationToken: token);

            if (model == null) { throw new NullReferenceException($"Model is null: {nameof(model)}"); }

            model.SunModel = await sunModelTask;
            //File.Delete(FILE_NAME);
            return model;
        }
        private async Task<ForecastModel> GetForecastModelAsync(CancellationToken token)
        {
            var sunModelsTask = Task.Run(() => GetSunModelsAsync(token), token);

            string url = UrlProvider.GetForecastUrl(Location.latitude, Location.longitude);
            var filename = ConfigHelper.TempFilename + "f";
            using (var input = new FileStream(filename, FileMode.Create))
                await HttpHelper.GetFileAsync(url, input, token, Progress);

            using var output = File.OpenRead(filename);
            var model = await JsonSerializer.DeserializeAsync<ForecastModel>(output, cancellationToken: token);

            if (model == null) { throw new NullReferenceException($"Model is null: {nameof(model)}"); }

            var sunModels = await sunModelsTask;

            foreach (var item in model.list)
                foreach (var sun in sunModels)
                    item.SunModel = sun;

            return model;
        }
        private async Task<SunModel[]> GetSunModelsAsync(CancellationToken token)
        {
            var models = new SunModel[5];
            var date = DateTime.Today;
            for (int i = 0; i < models.Length; i++)
            {
                token.ThrowIfCancellationRequested();
                models[i] = await GetSunModelAsync(token, date.ToString());
                date = date.AddDays(1);
            }
            return models;
        }
        private async Task<SunModel> GetSunModelAsync(CancellationToken token, string? date = null)
        {
            string url;
            if (date != null) { url = UrlProvider.GetSunUrl(Location.latitude, Location.longitude, date); }
            else { url = UrlProvider.GetSunUrl(Location.latitude, Location.longitude); }
            var filename = ConfigHelper.TempFilename + "s";
            using (var input = new FileStream(filename, FileMode.Create))
                await HttpHelper.GetFileAsync(url, input, token, Progress);
            using var output = File.OpenRead(filename);
            var model = await JsonSerializer.DeserializeAsync<SunModel>(output, cancellationToken: token);
            if (model == null) { throw new NullReferenceException($"Model is null: {nameof(model)}"); }
            return model;
        }



    }
}
