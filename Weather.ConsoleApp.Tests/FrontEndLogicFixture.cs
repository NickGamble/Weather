using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Weather.ConsoleApp.ApiClients;
using Weather.ConsoleApp.Database;
using Weather.ConsoleApp.Dtos;
using Weather.ConsoleApp.Tests.Builders;

namespace Weather.ConsoleApp.Tests
{
    [TestFixture]
    public class FrontEndLogicFixture
    {
        FrontEndLogic _sut;
        IWeatherClient _weatherClient;
        ICityDataAccess _cityDataAccess;
        IConsoleWrapper _consoleWrapper;
        public FrontEndLogicFixture()
        {
            _weatherClient = Substitute.For<IWeatherClient>();
            _cityDataAccess = Substitute.For<ICityDataAccess>();
            _consoleWrapper = Substitute.For<IConsoleWrapper>();

            _sut = new FrontEndLogic(_weatherClient, _cityDataAccess, _consoleWrapper, 1);

            _weatherClient.CitySearch(Arg.Any<string>()).Returns(new List<SearchCityDto>());
            _weatherClient.GetWeather(Arg.Any<string>()).Returns(new CurrentWeatherDto());
            _cityDataAccess.GetAllCities().Returns(new List<SearchCityDto>());
            _consoleWrapper.WriteLine(Arg.Any<string>());
            _consoleWrapper.ReadLine().Returns(string.Empty);

        }

        [Test]
        public void WeatherLoop_NoCitiesStored_ShowsWelcomeMessage()
        {
            // Act
            _sut.WeatherLoop();

            // Assert
            _consoleWrapper.Received(1).WriteLine("Welcome to the weather!");
            _consoleWrapper.Received(1).WriteLine("Please type the name of an Australian city and press enter");
            _consoleWrapper.DidNotReceive().WriteLine("or type city number from list below and press enter");
        }

        [Test]
        public void WeatherLoop_TwoCitiesStored_ShowsWelcomeMessage()
        {
            // Assemble
            var city1Name = "Bayswater";
            var state1Name = "WA";
            var id1 = 1;
            var city2Name = "Blayswater";
            var state2Name = "VIC";
            var id2 = 2;

            _cityDataAccess.GetAllCities().Returns(new List<SearchCityDto> { SearchCityBuilder.Build(city1Name, state1Name, id1), SearchCityBuilder.Build(city2Name, state2Name, id2) });

            // Act
            _sut.WeatherLoop();

            // Assert
            _consoleWrapper.Received(1).WriteLine("Welcome to the weather!"); 
            _consoleWrapper.Received(1).WriteLine("Please type the name of an Australian city and press enter"); 
            _consoleWrapper.Received(2).WriteLine("or type city number from list below and press enter"); // Received again after no results found
            _consoleWrapper.Received(2).WriteLine($"{id1}. {city1Name}, {state1Name}"); // Received again after no results found
            _consoleWrapper.Received(2).WriteLine($"{id2}. {city2Name}, {state2Name}"); // Received again after no results found
            _consoleWrapper.Received(1).WriteLine("No results found.");
            _consoleWrapper.Received(1).WriteLine($"Please enter city name");
            
        }
    }
}
