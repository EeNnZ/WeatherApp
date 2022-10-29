using WeatherCore.Services;

namespace WeatherCore.Interfaces
{
    public interface IModel
    {
        string ToString();
        IDictionary<string, object> AsDictionary();
    }
}
