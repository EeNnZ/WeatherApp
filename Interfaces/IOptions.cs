namespace WeatherCore.Interfaces
{
    public interface IOptions
    {
        public enum Mode { Now = 1, Forecast = 2 }
        public string? RequiredLocation { get; set; }
        public Mode RequiredMode { get; set; }
    }
}
