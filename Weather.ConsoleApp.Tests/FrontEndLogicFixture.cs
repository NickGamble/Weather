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

        [SetUp]
        public void Init()
        {
            _weatherClient = Substitute.For<IWeatherClient>();
            _cityDataAccess = Substitute.For<ICityDataAccess>();
            _consoleWrapper = Substitute.For<IConsoleWrapper>();

            _weatherClient.CitySearch(Arg.Any<string>()).Returns(new List<SearchCityDto>());
            _weatherClient.GetWeather(Arg.Any<string>()).Returns(new CurrentWeatherDto());
            _cityDataAccess.GetAllCities().Returns(new List<SearchCityDto>());
            _consoleWrapper.WriteLine(Arg.Any<string>());
            _consoleWrapper.ReadLine().Returns(string.Empty);

            _sut = new FrontEndLogic(_weatherClient, _cityDataAccess, _consoleWrapper, 1);
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
            _sut = new FrontEndLogic(_weatherClient, _cityDataAccess, _consoleWrapper, 1);

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

        [Test]
        public void WeatherLoop_SuccessfulCitySearch_DisplaysWeatherDetailsAndSavesCity()
        {
            // Assemble
            var cityName = "TestCity";
            var stateName = "TestState";
            var id = 1;
            var key = "ABC";
            var searchCityDto = SearchCityBuilder.Build(cityName, stateName, id, key);

            var weatherDate = DateTime.Now;
            var weatherDescription = "Sunny and cloudy and rainy and dry";
            var tempValue = "24";
            var tempUnit = "C";
            var weatherDto = CurrentWeatherBuilder.Build(weatherDescription, tempValue, tempUnit, weatherDate);

            _weatherClient.CitySearch(cityName).Returns(new List<SearchCityDto> { searchCityDto });
            _consoleWrapper.ReadLine().Returns(cityName);
            _weatherClient.GetWeather(key).Returns(weatherDto);

            // Act 
            _sut.WeatherLoop();

            // Assert
            _cityDataAccess.Received(1).SaveCity(searchCityDto);

            _weatherClient.Received(1).GetWeather(key);
            _consoleWrapper.Received(1).WriteLine($"{weatherDto.LocalObservationDateTime.ToShortTimeString()} - {weatherDto.LocalObservationDateTime.ToShortDateString()}{Environment.NewLine}" +
                              $"{weatherDto.WeatherText} {weatherDto.Temperature.Metric.Value}{weatherDto.Temperature.Metric.Unit}{Environment.NewLine}");

        }

        [Test]
        public void WeatherLoop_EnterStoredCityId_DisplaysWeatherDetailsCallsSaveCity()
        {
            // Assemble
            var cityName = "TestCity";
            var stateName = "TestState";
            var id = 1;
            var key = "ABC";
            var searchCityDto = SearchCityBuilder.Build(cityName, stateName, id, key);

            var weatherDate = DateTime.Now;
            var weatherDescription = "Sunny and cloudy and rainy and dry";
            var tempValue = "24";
            var tempUnit = "C";
            var weatherDto = CurrentWeatherBuilder.Build(weatherDescription, tempValue, tempUnit, weatherDate);

            _cityDataAccess.GetAllCities().Returns(new List<SearchCityDto> { searchCityDto });
            _consoleWrapper.ReadLine().Returns(id.ToString());
            _weatherClient.GetWeather(key).Returns(weatherDto);

            // Act 
            _sut.WeatherLoop();

            // Assert
            _consoleWrapper.Received(1).WriteLine("Welcome to the weather!");
            _consoleWrapper.Received(1).WriteLine("Please type the name of an Australian city and press enter");
            _consoleWrapper.Received(2).WriteLine("or type city number from list below and press enter");
            _consoleWrapper.Received(1).WriteLine($"City found: {cityName}, {stateName}");
            _consoleWrapper.Received(1).WriteLine($"Getting current weather...");
            _cityDataAccess.Received(1).SaveCity(searchCityDto);

            _weatherClient.Received(1).GetWeather(key);
            _consoleWrapper.Received(1).WriteLine($"{weatherDto.LocalObservationDateTime.ToShortTimeString()} - {weatherDto.LocalObservationDateTime.ToShortDateString()}{Environment.NewLine}" +
                              $"{weatherDto.WeatherText} {weatherDto.Temperature.Metric.Value}{weatherDto.Temperature.Metric.Unit}{Environment.NewLine}");
        }
    }
}
