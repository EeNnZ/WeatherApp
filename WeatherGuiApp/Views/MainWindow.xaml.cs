using System.Windows;
using WeatherCore;
using System;
using WeatherCore.Services;
using System.Threading.Tasks;
using WeatherCore.Interfaces;
using System.Windows.Controls;
using WeatherGuiApp.ViewModels;
using System.Windows.Data;
using WeatherGuiApp.Views;
using WeatherCore.Providers;

namespace WeatherGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainViewModel _viewModel;
        readonly IProgress<double> _progress;
        public MainWindow()
        {
            InitializeComponent();
            HttpHelper.InitializeClient();
            ConfigHelper.BuildConfig();
            _progress = new Progress<double>(value => progressBar.Value = value);
            _viewModel = new MainViewModel(_progress, Dispatcher);
            DataContext = _viewModel;
            weatherControl.DataContext = _viewModel;
        }
    }
}
