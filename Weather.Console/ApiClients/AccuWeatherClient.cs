using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Weather.ConsoleApp.Dtos;
using System.Net.Http.Headers;
using System.Linq;

namespace Weather.ConsoleApp.ApiClients
{    
    public class AccuWeatherClient : IWeatherClient
    {
        private const string ApiKey = "38KCQHwcRy2xNrvF5LbXKVj7XnrI6caU";
        private const string BaseAddress = "http://dataservice.accuweather.com/";

        public List<SearchCityDto> CitySearch(string searchString)
        {
            var urlString = $"{BaseAddress}locations/v1/cities/AU/search?apikey={ApiKey}&q={searchString}";
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(urlString).Result;
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<SearchCityDto>>().Result;
                return result;
            }            
        }

        public CurrentWeatherDto GetWeather(string cityKey)
        {
            var urlString = $"{BaseAddress}currentconditions/v1/{cityKey}?apikey={ApiKey}";
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(urlString).Result;
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<CurrentWeatherDto>>().Result;
                return result.First();
            }
        }
    }
}
