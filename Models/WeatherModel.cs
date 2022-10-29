using System.Text;
using WeatherCore.Interfaces;

namespace WeatherCore.Models
{
    public class WeatherModel : IModel
    {
        public Weather[] weather { get; set; } = null!;
        public string _base { get; set; } = null!;
        public Main main { get; set; } = null!;
        public int visibility { get; set; }
        public Wind wind { get; set; } = null!;
        public string name { get; set; } = null!;
        public Sys sys { get; set; } = null!;
        public SunModel SunModel { get; set; } = null!;
        public IDictionary<string, object> AsDictionary()
        {
            var dict = new Dictionary<string, object>()
            {
                { "Place", $"{name}, {sys.country}" },
                { "Main", $"{weather.First().main}" },
                { "Description", $"{weather.First().description}" },
                { "Temp", $"{main.temp}" },
                { "Feels like", $"{main.temp}" },
                { "Pressure", $"{main.pressure}" },
                { "Humidity", $"{main.humidity}" },
                { "Visibility", $"{visibility}" },
                { "Wind speed", $"{wind.speed}" },
                { "Sunrise", $"{SunModel.results.sunrise}" },
                { "Sunset", $"{SunModel.results.sunset}" },
                { "Solar noon", $"{SunModel.results.solar_noon}" },
                { "Day length", $"{SunModel.results.day_length}" }
            };
            return dict;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("------------------------------------------------------------------");
            sb.AppendLine($"Place: {name}, {sys.country}\r\n");
            sb.AppendLine($"Main: {weather.First().main}");
            sb.AppendLine($"Description: {weather.First().description}");
            sb.AppendLine($"Temp: {main.temp} \u2103");
            sb.AppendLine($"Feels like: {main.feels_like} ℃");
            sb.AppendLine($"Pressure: {main.pressure}p");
            sb.AppendLine($"Humidity: {main.humidity}%");
            sb.AppendLine($"Visibility: {visibility} meters");
            sb.AppendLine($"Wind speed: {wind.speed}");
            sb.Append(SunModel.ToString());
            sb.AppendLine("------------------------------------------------------------------");
            return sb.ToString();
        }
    }
    public class Sys
    {
        public string country { get; set; } = null!;
    }
    public class Main
    {
        public float temp { get; set; }
        public float feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Temperature: {temp}");
            sb.AppendLine($"Feels like: {feels_like}");
            sb.AppendLine($"Pressure: {pressure}");
            sb.AppendLine($"Humidity: {humidity}");
            return sb.ToString();
        }
    }

    public class Wind
    {
        public float speed { get; set; }
        public override string ToString()
        {
            return $"Wind speed: {speed}";
        }
    }

    public class Weather
    {
        public string main { get; set; } = null!;
        public string description { get; set; } = null!;
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Main: {main}");
            sb.AppendLine($"Description: {description}");
            return sb.ToString();
        }
    }


}
