using System.Text;
using WeatherCore.Interfaces;

namespace WeatherCore.Models
{
    public class IpLocationModel : IModel
    {
        public string country { get; set; } = null!;
        public string city { get; set; } = null!;
        public float lat { get; set; }
        public float lon { get; set; }

        public IDictionary<string, object> AsDictionary()
        {
            var dict = new Dictionary<string, object>()
            {
                {nameof(country), country},
                {nameof(city), city},
                {nameof(lat), lat},
                {nameof(lon), lon},
            };
            return dict;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Country: {country}");
            sb.AppendLine($"City: {city}");
            sb.AppendLine($"Latitude: {lat}");
            sb.AppendLine($"Longitude: {lon}");
            return sb.ToString();
        }
    }

}
