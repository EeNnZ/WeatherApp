﻿using System;
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

        public async Task<IModel> RunAsync()
        {
            string? requiredLocation = _options.RequiredLocation;
            if (requiredLocation != null) { Location = await Locator.TryGetLocationByCityNameAsync(requiredLocation); }
            else { Location = await Locator.GetLocationByIpAsync(); }

            if (_options.RequiredMode == IOptions.Mode.Now) { return await GetWeatherModelAsync(); }
            else { return await GetForecastModelAsync(); }
        }
        private async Task<WeatherModel> GetWeatherModelAsync()
        {
            var sunModelTask = GetSunModelAsync();

            string url = UrlProvider.GetWeatherUrl(Location.latitude, Location.longitude);
            var filename = ConfigHelper.TempFilename + "w";
            using (var input = new FileStream(filename, FileMode.Create))
                await HttpHelper.GetFileAsync(url, input, Progress);

            using var output = File.OpenRead(filename);
            var model = await JsonSerializer.DeserializeAsync<WeatherModel>(output);

            if (model == null) { throw new NullReferenceException($"Model is null: {nameof(model)}"); }

            model.SunModel = await sunModelTask;
            //File.Delete(FILE_NAME);
            return model;
        }
        private async Task<ForecastModel> GetForecastModelAsync()
        {
            var sunModelsTask = Task.Run(() => GetSunModelsAsync());

            string url = UrlProvider.GetForecastUrl(Location.latitude, Location.longitude);
            var filename = ConfigHelper.TempFilename + "f";
            using (var input = new FileStream(filename, FileMode.Create))
                await HttpHelper.GetFileAsync(url, input, Progress);

            using var output = File.OpenRead(filename);
            var model = await JsonSerializer.DeserializeAsync<ForecastModel>(output);

            if (model == null) { throw new NullReferenceException($"Model is null: {nameof(model)}"); }

            var sunModels = await sunModelsTask;

            foreach (var item in model.list)
                foreach (var sun in sunModels)
                    item.SunModel = sun;

            return model;
        }
        private async Task<SunModel[]> GetSunModelsAsync()
        {
            var models = new SunModel[5];
            var date = DateTime.Today;
            for (int i = 0; i < models.Length; i++)
            {
                models[i] = await GetSunModelAsync(date.ToString());
                date = date.AddDays(1);
            }
            return models;
        }
        private async Task<SunModel> GetSunModelAsync(string? date = null)
        {
            string url;
            if (date != null) { url = UrlProvider.GetSunUrl(Location.latitude, Location.longitude, date); }
            else { url = UrlProvider.GetSunUrl(Location.latitude, Location.longitude); }
            var filename = ConfigHelper.TempFilename + "s";
            using (var input = new FileStream(filename, FileMode.Create))
                await HttpHelper.GetFileAsync(url, input, Progress);
            using var output = File.OpenRead(filename);
            var model = await JsonSerializer.DeserializeAsync<SunModel>(output);
            if (model == null) { throw new NullReferenceException($"Model is null: {nameof(model)}"); }
            return model;
        }



    }
}
