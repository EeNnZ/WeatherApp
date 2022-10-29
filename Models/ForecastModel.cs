using System.Text;
using System.Text.RegularExpressions;
using WeatherCore.Interfaces;

namespace WeatherCore.Models
{
    public class ForecastModel : IModel
    {

        public List[] list { get; set; } = null!;
        public City city { get; set; } = null!;

        public IDictionary<string, object> AsDictionary()
        { 
            var dict = list.GroupBy(x =>
            {
                var day_txt = x.dt_txt[..10];
                string day = DateTime.Parse(day_txt).ToString("dddd");
                return day;
            }).ToDictionary(
                x => x.Key, 
                n => (object)n.ToArray()
                );
            dict.Add("Place", $"{city.name}, {city.country}");
            return dict;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"City: {city.name} | Country: {city.country}");
            var grouped = list.GroupBy(x => x.dt_txt[..10]);
            foreach (var group in grouped)
            {
                sb.AppendLine($"------ Day: {group.Key}");
                foreach (var item in group)
                {
                    sb.AppendLine($"{item.ToString()}");
                    sb.AppendLine();
                }
            }
            //foreach (var item in list)
            //{
            //    sb.AppendLine($"------ {item.ToString()}");
            //    sb.AppendLine();
            //}
            return sb.ToString();
        }
    }

    public class City
    {
        public string name { get; set; } = null!;
        public string country { get; set; } = null!;
    }

    public class List
    {
        public Main main { get; set; } = null!;
        public Weather[] weather { get; set; } = null!;
        public Clouds clouds { get; set; } = null!;
        public Wind wind { get; set; } = null!;
        public int visibility { get; set; }
        public string dt_txt { get; set; } = null!;
        public Rain rain { get; set; } = null!;
        public SunModel SunModel { get; set; } = null!;
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Time: {dt_txt.Split(" ").Last()}");
            foreach (var w in weather) { sb.AppendLine(w.ToString()); }
            sb.AppendLine($"Temp: {main.temp} ℃");
            sb.AppendLine($"Feels like: {main.feels_like} ℃");
            sb.AppendLine($"Pressure: {main.pressure}p");
            sb.AppendLine($"Humidity: {main.humidity}%");
            sb.AppendLine($"Rain 3h: {rain?._3h}");
            sb.AppendLine($"{wind.ToString()}");
            sb.AppendLine(SunModel.ToString());
            sb.AppendLine("------------------------------------");
            return sb.ToString();
        }
    }

    public class Clouds
    {
        public int all { get; set; }
    }


    public class Rain
    {
        public float _3h { get; set; }
    }

}
