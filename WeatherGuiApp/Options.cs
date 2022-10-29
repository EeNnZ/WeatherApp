using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherCore.Interfaces;

namespace WeatherGuiApp
{
    internal class Options : IOptions
    {
        public string? RequiredLocation { get; set; }
        public IOptions.Mode RequiredMode { get; set; }
    }
}
