using System;
using System.Collections.Generic;
using System.Linq;
using Weather.ConsoleApp.ApiClients;
using Weather.ConsoleApp.Database;
using Weather.ConsoleApp.Dtos;

namespace Weather.ConsoleApp
{
    public class FrontEndLogic
    {
        private readonly IWeatherClient _weatherClient;
        private readonly ICityDataAccess _cityDataAccess;

        public FrontEndLogic(IWeatherClient weatherClient, ICityDataAccess cityDataAccess)
        {
            _weatherClient = weatherClient;
            _cityDataAccess = cityDataAccess;
        }

        private void WelcomeMessage()
        {
            Console.WriteLine("Welcome to the weather!");
            Console.WriteLine("Please type the name of an Australian city and press enter");

            ListStoredCities();
            
        }    
        
        private void ListStoredCities()
        {
            var allCities = _cityDataAccess.GetAllCities();
            if (allCities.Any())
                Console.WriteLine("or type city number from list below and press enter");

            foreach (var city in allCities)
                Console.WriteLine($"{city.Id}. {city.LocalizedName}, {city.ParentCity.LocalizedName}");
        }

        public void WeatherLoop()
        {
            WelcomeMessage();

            
            while (true)
            {
                var citySearch = Console.ReadLine();
                CitySearch(citySearch);
                Console.WriteLine($"Please enter city name");
                ListStoredCities();
            }
        }

        private void CitySearch(string searchString)
        {
                int cityId = 0;
                var isCityId = int.TryParse(searchString, out cityId);

                IEnumerable<SearchCityDto> results;
                if (isCityId)
                    results = _cityDataAccess.GetAllCities().Where(x => x.Id == cityId);
                else
                    results = _weatherClient.CitySearch(searchString);

            if (results.Any())
            {
                foreach (var city in results)
                {
                    Console.WriteLine($"City found: {city.LocalizedName}, {city.ParentCity.LocalizedName}");
                    Console.WriteLine($"Getting current weather...");

                    _cityDataAccess.SaveCity(city);

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
    }
}
