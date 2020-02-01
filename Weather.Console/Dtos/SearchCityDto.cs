using System;
using System.Collections.Generic;
using System.Text;

namespace Weather.ConsoleApp.Dtos
{
    public class SearchCityDto
    {
        public int Id { get; set; }
        public string LocalizedName { get; set; }
        public ParentCity ParentCity { get; set; } = new ParentCity();

        public string Key { get; set; }
    }

    public class ParentCity
    {
        public string LocalizedName { get; set; }
    }
}
