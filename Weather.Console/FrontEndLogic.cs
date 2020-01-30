using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void WelcomeMessage()
        {
            Console.WriteLine("Welcome to the weather!");
            Console.WriteLine("Please type the name of an Australian city and press enter");
        }

        public void CitySearch(string searchString)
        {
            try
            {
                var result = _weatherClient.CitySearch(searchString);
                var resultCount = result.Count;
                if (resultCount == 1)
                {
                    var city = result.Single();
                    Console.WriteLine($"City found: {city.LocalizedName},{city.ParentCity}");
                    Console.WriteLine($"Getting current weather...");

                    var weatherDto = _weatherClient.GetWeather(city.Key);
                    Console.WriteLine(@$"{weatherDto.LocalObservationDateTime}{Environment.NewLine}{weatherDto.WeatherText} {weatherDto.Temperature.Metric.Value}{weatherDto.Temperature.Metric.Unit}");                    
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
