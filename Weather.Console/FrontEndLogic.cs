using System;
using System.Linq;
using Weather.ConsoleApp.ApiClients;

namespace Weather.ConsoleApp
{
    public class FrontEndLogic
    {
        private readonly IWeatherClient _weatherClient;

        public FrontEndLogic(IWeatherClient weatherClient)
        {
            _weatherClient = weatherClient;
        }

        private void WelcomeMessage()
        {
            Console.WriteLine("Welcome to the weather!");
            Console.WriteLine("Please type the name of an Australian city and press enter");
        }

        public void WeatherLoop()
        {
            WelcomeMessage();

            
            while (true)
            {
                var citySearch = Console.ReadLine();
                CitySearch(citySearch);
                Console.WriteLine($"Please enter city name");
            }
        }

        private void CitySearch(string searchString)
        {
            try
            {
                var results = _weatherClient.CitySearch(searchString);
                if (results.Any())
                {
                    foreach (var city in results)
                    {
                        Console.WriteLine($"City found: {city.LocalizedName}, {city.ParentCity.LocalizedName}");
                        Console.WriteLine($"Getting current weather...");

                        var weatherDto = _weatherClient.GetWeather(city.Key);
                        Console.WriteLine($"{weatherDto.LocalObservationDateTime.ToShortTimeString()} - {weatherDto.LocalObservationDateTime.ToShortDateString()}{Environment.NewLine}" +
                                          $"{weatherDto.WeatherText} {weatherDto.Temperature.Metric.Value}{weatherDto.Temperature.Metric.Unit}{Environment.NewLine}");
                    }
                }
                else
                {
                    Console.WriteLine("No results found.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unexpected error");
            }
        }
    }
}
