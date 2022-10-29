using CommandLine;
using WeatherCore.Interfaces;


namespace WeatherConsoleApp
{
    public class Options : IOptions
    {
        [Option('l', "location", HelpText = "If passed - search for match. Otherwise, use current ip location.", Required = false, Default = null)]
        public string? RequiredLocation { get; set; }

        [Option('m', "mode", HelpText = "Allowed values: 1 -> now, 2 - forecast.", Required = true)]
        public int Mode { get; set; }
        public IOptions.Mode RequiredMode { get => (IOptions.Mode)Mode; set { } }
    }
}
