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
        private int? _loops;
        private readonly IWeatherClient _weatherClient;
        private readonly ICityDataAccess _cityDataAccess;
        private readonly IConsoleWrapper _consoleWrapper;

        public FrontEndLogic(IWeatherClient weatherClient, ICityDataAccess cityDataAccess, IConsoleWrapper consoleWrapper, int? loops = null)
        {
            _weatherClient = weatherClient;
            _cityDataAccess = cityDataAccess;
            _consoleWrapper = consoleWrapper;
            _loops = loops; // Avoid infinite loop for testing purposes. Yuck!
        }

        private void WelcomeMessage()
        {
            _consoleWrapper.WriteLine("Welcome to the weather!");
            _consoleWrapper.WriteLine("Please type the name of an Australian city and press enter");

            ListStoredCities();
            
        }    
        
        private void ListStoredCities()
        {
            var allCities = _cityDataAccess.GetAllCities();
            if (allCities.Any())
                _consoleWrapper.WriteLine("or type city number from list below and press enter");

            foreach (var city in allCities)
                _consoleWrapper.WriteLine($"{city.Id}. {city.LocalizedName}, {city.ParentCity.LocalizedName}");
        }

        public void WeatherLoop()
        {
            WelcomeMessage();

            int loopCount = 0;
            while (_loops == null || _loops > loopCount)
            {
                var citySearch = _consoleWrapper.ReadLine();
                CitySearch(citySearch);
                _consoleWrapper.WriteLine($"Please enter city name");
                ListStoredCities();
                loopCount++;
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
                    _consoleWrapper.WriteLine($"City found: {city.LocalizedName}, {city.ParentCity.LocalizedName}");
                    _consoleWrapper.WriteLine($"Getting current weather...");

                    _cityDataAccess.SaveCity(city);

                    var weatherDto = _weatherClient.GetWeather(city.Key);
                    _consoleWrapper.WriteLine($"{weatherDto.LocalObservationDateTime.ToShortTimeString()} - {weatherDto.LocalObservationDateTime.ToShortDateString()}{Environment.NewLine}" +
                                      $"{weatherDto.WeatherText} {weatherDto.Temperature.Metric.Value}{weatherDto.Temperature.Metric.Unit}{Environment.NewLine}");


                }
            }
            else
            {
                _consoleWrapper.WriteLine("No results found.");
            }
        }
    }
}
