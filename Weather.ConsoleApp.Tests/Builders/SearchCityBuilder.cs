using Weather.ConsoleApp.Dtos;

namespace Weather.ConsoleApp.Tests.Builders
{
    public static class SearchCityBuilder
    {
        public static SearchCityDto Build(string city, string state, int id = 0, string key = "")
        {
            return new SearchCityDto
            {
                Id = id,
                Key = key,
                LocalizedName = city,
                ParentCity = new ParentCity { LocalizedName = state }
            };
        }
    }
}
