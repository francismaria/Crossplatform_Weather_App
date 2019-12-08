using System;
using Newtonsoft.Json;

namespace MyWeather.Data
{
    public class WeatherData
    {
        [JsonProperty("name")]
        public string Title { get; set; }

    }
}
