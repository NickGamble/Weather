using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Weather.ConsoleApp.Dtos;

namespace Weather.ConsoleApp.Database
{
    public class FileCityDataAccess : ICityDataAccess
    {
        private const string FilePath = "Cities.json";

        public List<SearchCityDto> GetAllCities()
        {
            if (!File.Exists(FilePath))
                using (var fs = File.Create(FilePath));

            var cities = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(cities)) 
                return new List<SearchCityDto>();

            return JsonConvert.DeserializeObject<List<SearchCityDto>>(cities).ToList();
        }

        public void SaveCity(SearchCityDto newCity)
        {
            // TODO: This is super inefficient
            var cities = File.ReadAllText(FilePath);

            int maxId = 0;
            List<SearchCityDto> allCities = new List<SearchCityDto>();
            if (!string.IsNullOrEmpty(cities))
            { 
                allCities = JsonConvert.DeserializeObject<List<SearchCityDto>>(cities);
                if (CityAlreadyExists(allCities, newCity))
                    return;
                maxId = allCities.Max(x => x.Id);
            }

            newCity.Id = maxId + 1;
            allCities.Add(newCity);
            var allCitiesJson = JsonConvert.SerializeObject(allCities);

            File.WriteAllText(FilePath, allCitiesJson); // TODO: Very inefficient to blow whole file contents away each time
        }

        private bool CityAlreadyExists(List<SearchCityDto> allCities, SearchCityDto newCity)
        {
            return allCities.Any(x => x.Key == newCity.Key);
        }
    }
}
