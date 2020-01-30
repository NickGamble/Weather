using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Weather.ConsoleApp.Dtos;

namespace Weather.ConsoleApp.ApiClients
{
    public interface IWeatherClient
    {
        List<SearchCityDto> CitySearch(string searchString);

        CurrentWeatherDto GetWeather(string cityKey);
    }
}
