using System.Collections.Generic;
using Weather.ConsoleApp.Dtos;

namespace Weather.ConsoleApp.Database
{
    public interface ICityDataAccess
    {
        void SaveCity(SearchCityDto city);

        List<SearchCityDto> GetAllCities();
    }
}
