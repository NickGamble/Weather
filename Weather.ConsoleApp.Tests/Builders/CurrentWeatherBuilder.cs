using System;
using System.Collections.Generic;
using System.Text;
using Weather.ConsoleApp.Dtos;

namespace Weather.ConsoleApp.Tests.Builders
{
    public static class CurrentWeatherBuilder
    {
        public static CurrentWeatherDto Build(string description, string temperatureValue, string temperatureUnit, DateTime dateTime)
        {
            return new CurrentWeatherDto
            {
                LocalObservationDateTime = dateTime,
                Temperature = new Temperature { Metric = new Metric { Unit = temperatureUnit, Value = temperatureValue } },
                WeatherText = description
            };
        }
    }
}
