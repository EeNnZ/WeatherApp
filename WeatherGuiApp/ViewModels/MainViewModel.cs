using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WeatherCore;
using WeatherCore.Interfaces;
using WeatherCore.Models;
using WeatherCore.Services;
using WeatherGuiApp;

namespace WeatherGuiApp.ViewModels
{
    public class MainViewModel : AbstractBindable
    {
        private CancellationTokenSource _cts = new();
        private WeatherService _service = null!;
        private readonly IProgress<double> _progress;
        //Need dispatcher?
        private readonly Dispatcher _dispatcher;
        private readonly Stopwatch _stopwatch;

        private double _progressValue;
        private long? _elapsed;
        private string? _city;
        private string? _textStatus = "Go";
        private bool _goButtonEnabled = true;
        private bool _currentWeatherVisible = true;

        public IOptions.Mode WorkMode
        {
            get
            {
                if (CurrentWeatherVisible) return IOptions.Mode.Now;
                else return IOptions.Mode.Forecast;
            }
        }
        public double ProgressValue

        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }
        public long? Elapsed
        {
            get => _elapsed;
            set => SetProperty(ref _elapsed, value);
        }
        public string? City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }
        public string? TextStatus
        {
            get => _textStatus;
            set => SetProperty(ref _textStatus, value);
        }
        public bool GoButtonEnabled
        {
            get => _goButtonEnabled;
            set => SetProperty(ref _goButtonEnabled, value);
        }
        public bool CurrentWeatherVisible
        {
            get => _currentWeatherVisible;
            set => SetProperty(ref _currentWeatherVisible, value);
        }
        public bool ForecastVisible => !CurrentWeatherVisible;
        public DelegateCommand GoButtonCommand => new(async () => await DoWork());
        public DelegateCommand CancelCommand => new(() => Cancel());

        public ObservableCollection<KeyValuePair<string, object>> Results { get; private set; }
        //TODO: Implement forecast representation in XAML

        public MainViewModel(IProgress<double> progress, Dispatcher dispatcher)
        {
            _progress = progress;
            _dispatcher = dispatcher;
            _stopwatch = new Stopwatch();
            Results = new();
        }
        public async Task DoWork()
        {
            var options = new Options
            {
                RequiredLocation = City,
                RequiredMode = CurrentWeatherVisible ? IOptions.Mode.Now : IOptions.Mode.Forecast
            };

            _service = new WeatherService(_progress, options);
            _stopwatch.Start();
            SetBusy();

            IModel model;
            try
            {
                model = await GetResultASync(_cts.Token);
            }
            catch (OperationCanceledException e)
            {
                SetCanceled(e.Message);
                RestoreToken();
                return;
            }
            var res = ModelAsDictionary(model);
            UpdateResults(res);

            _stopwatch.Stop();
            Restore();
        }
        private void Cancel()
        {
            if (GoButtonEnabled) return;
            if (_cts.Token.CanBeCanceled)
            {
                _cts.Cancel();
            }
            else MessageBox.Show("Operation cannot be canceled right now", "Cancellation error", MessageBoxButton.OK);
        }

        private void RestoreToken()
        {
            _cts = new();
        }

        private void SetCanceled(string message)
        {
            Restore();
            Results.Add(new KeyValuePair<string, object>("Status", "Canceled"));
        }

        private async Task<IModel> GetResultASync(CancellationToken token)
        {
            var model = await _service.RunAsync(token);
            return model;
        }
        private IEnumerable<KeyValuePair<string, object>> ModelAsDictionary(IModel model)
        {
            var result = model.AsDictionary()
                .Reverse()
                .Select(x => new KeyValuePair<string, object>(x.Key, x.Value))
                .ToArray();
            return result;
        }
        private void UpdateResults(IEnumerable<KeyValuePair<string, object>> res)
        {
            foreach (var item in res)
            {
                Results.Add(item);
            }
        }
        private void SetBusy()
        {
            GoButtonEnabled = false;
            Results.Clear();
            TextStatus = "Processing...";
        }
        private void Restore()
        {
            TextStatus = "Go";
            ProgressValue = 0d;
            Elapsed = _stopwatch?.ElapsedMilliseconds;
            GoButtonEnabled = true;
        }
    }
}
