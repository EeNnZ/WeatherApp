using CommandLine;
using WeatherCore;
using WeatherCore.Providers;
using ShellProgressBar;
using System.ComponentModel;
using WeatherCore.Services;
using System;
using System.Text;

namespace WeatherConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser(x =>
            {
                x.AutoHelp = true;
                x.AutoVersion = false;
                x.CaseSensitive = false;
                x.IgnoreUnknownArguments = true;
                x.HelpWriter = Console.Error;
                x.EnableDashDash = false;
            });

            HttpHelper.InitializeClient();
            ConfigHelper.BuildConfig();

            var progress = new ProgressBar(10, "Progress", new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
                ForegroundColorDone = ConsoleColor.DarkGreen,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593',
                DisplayTimeInRealTime = false
            }).AsProgress<double>();
            Console.OutputEncoding = Encoding.Unicode;
            var options = parser.ParseArguments<Options>(args)
                                .WithParsedAsync(async (op) =>
                                {
                                    var result = new WeatherService(progress, op).RunAsync();
                                    Console.WriteLine(await result);
                                }).ContinueWith(x => Console.Write("Press any to exit"));
            Console.ReadLine();
        }
    }
}