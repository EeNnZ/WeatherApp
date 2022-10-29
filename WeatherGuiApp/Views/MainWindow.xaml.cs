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
        MainViewModel viewModel;
        IOptions options;
        IProgress<double> progress;
        public MainWindow()
        {
            InitializeComponent();
            HttpHelper.InitializeClient();
            ConfigHelper.BuildConfig();
            progress = new Progress<double>(value => progressBar.Value = value);
            viewModel = new MainViewModel(progress, Dispatcher);
            DataContext = viewModel;
            weatherControl.DataContext = viewModel;
        }
    }
}
