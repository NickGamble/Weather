using System;
using Weather.ConsoleApp.ApiClients;
using Weather.ConsoleApp.Database;

namespace Weather.ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                var frontEndLogic = new FrontEndLogic(new AccuWeatherClient(), new FileCityDataAccess(), new ConsoleWrapper()); // TODO: Use IoC container to build up dependencies
                frontEndLogic.WeatherLoop();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Unexpected exception: {ex.Message}");
            }
            
            Console.ReadLine();

        }
    }
}
