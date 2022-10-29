using System.Text;
using WeatherCore.Interfaces;

namespace WeatherCore.Models
{
    public class GeocodeModel : IModel
    {
        public string name { get; set; } = null!;
        public float lat { get; set; }
        public float lon { get; set; }
        public string country { get; set; } = null!;

        public IDictionary<string, object> AsDictionary()
        {
            var dict = new Dictionary<string, object>()
            {
                {nameof(name), name},
                {nameof(lat), lat},
                {nameof(lon), lon},
                {nameof(country), country},
            };
            return dict;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name: {name}");
            sb.AppendLine($"Latitude: {lat}");
            sb.AppendLine($"Longitude: {lon}");
            sb.AppendLine($"Country: {country}");
            return sb.ToString();
        }
    }
}
