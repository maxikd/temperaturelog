using Newtonsoft.Json;

namespace WebAPI.Models
{
    public class Forecast
    {
        [JsonProperty("temp")]
        public float Temperature { get; set; }
    }
}
