using System.Collections.Generic;

namespace WebAPI.Models
{
    public class CityTemperatureResultListItem
    {
        public string City { get; set; }
        public IEnumerable<object> Temperatures { get; set; }
    }
}
