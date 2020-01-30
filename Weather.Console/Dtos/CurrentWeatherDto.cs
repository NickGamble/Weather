using System;

namespace Weather.ConsoleApp.Dtos
{
    public class CurrentWeatherDto
    {
        public string LocalObservationDateTime { get; set; }
        public string WeatherText { get; set; }

        public Temperature Temperature { get; set; }
    }

    public class Temperature
    {
        public Metric Metric { get; set; }
    }

    public class Metric
    {
        public string Value { get; set; }
        public string Unit { get; set; }
    }
}
