using System.Text;
using WeatherCore.Interfaces;

namespace WeatherCore.Models
{
    public class SunModel : IModel
    {
        public Results results { get; set; } = null!;
        public IDictionary<string, object> AsDictionary()
        {
            var dict = new Dictionary<string, object>()
            {
                { "Sunrise", $"{results.sunrise}" },
                { "Sunset", $"{results.sunset}" },
                { "Solar noon", $"{results.solar_noon}" },
                { "Day length", $"{results.day_length}" }
            };
            return dict;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Sunrise: {results.sunrise}");
            sb.AppendLine($"Sunset: {results.sunset}");
            sb.AppendLine($"Solar noon: {results.solar_noon}");
            sb.AppendLine($"Day length: {results.day_length}");
            return sb.ToString();
        }
    }

    public class Results
    {
        public string sunrise { get; set; } = null!;
        public string sunset { get; set; } = null!;
        public string solar_noon { get; set; } = null!;
        public string day_length { get; set; } = null!;
    }


}
