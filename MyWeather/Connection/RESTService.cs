using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyWeather.Data;

namespace MyWeather.Connection
{
    public class RESTService
    {
        HttpClient client;

        public RESTService()
        {
            client = new HttpClient();
        }

        public async Task<WeatherData> GetWeatherDataAsync(string uri)
        {
            WeatherData weatherData = null;

           
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
            } else
            {
                throw new Exception();
            }
            

            return weatherData;
        }

        public async Task<List<WeatherData>> GetNextDayWeatherDataAsync(string uri)
        {
            
            WeatherForecastData forecastData = null;
            List<WeatherData> weatherForecastData = null;


            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                forecastData = JsonConvert.DeserializeObject<WeatherForecastData>(content);
                weatherForecastData = forecastData.Items;
            }
            else
            {
                throw new Exception();
            }
            return weatherForecastData;
        }
    }
}
