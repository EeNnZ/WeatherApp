namespace WeatherCore
{
    public struct LocationStruct
    {
        public readonly string city;
        public readonly float latitude;
        public readonly float longitude;

        public LocationStruct(float latitude, float longitude, string city)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.city = city;
        }
    }
}
