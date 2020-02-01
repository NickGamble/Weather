using System;
using Weather.ConsoleApp.ApiClients;

namespace Weather.ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            var frontEndLogic = new FrontEndLogic(new AccuWeatherClient()); // TODO: Use IoC container to build up dependencies
            frontEndLogic.WeatherLoop();
            
            Console.ReadLine();

        }
    }
}
