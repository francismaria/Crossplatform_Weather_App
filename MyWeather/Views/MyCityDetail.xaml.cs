using System;
using System.Collections.Generic;
using System.Diagnostics;
using MyWeather.Connection;
using MyWeather.Data;

using Xamarin.Forms;

namespace MyWeather.Views
{
    public partial class MyCityDetail : ContentPage
    {
        private string id;
        private string cityName;
        private string cityCountry;

        private RESTService _restService;

        public MyCityDetail(string id, string cityName, string cityCountry)
        {
            InitializeComponent();

            this.id = id;
            this.cityName = cityName;
            this.cityCountry = cityCountry;

            _restService = new RESTService();

            InitializeToolbar();
            SendAPIRequests();
        }

        private void InitializeToolbar()
        {
            var toolBarItem = new ToolbarItem
            {
                Text = this.cityName + ", " + this.cityCountry
            };

            ToolbarItems.Add(toolBarItem);
        }

        private async void SendAPIRequests()
        {
            try
            {
                SendCurrentWeatherRequest();
                SendNextDayForecastRequest();
                HideActivityIndicator();
            }
            catch (Exception e)
            {
                Debug.WriteLine("_SendAPIRequest: Connection exception occurred. " + e);
                ShowErrorMessage();
            }
        }

        private async void SendCurrentWeatherRequest()
        { 
            WeatherData weatherData = await _restService.GetWeatherDataAsync(GenerateRequestUri(Constants.OpenWeatherMapEndpoint));
            BindingContext = weatherData;

            ShowContentPanel();
            BindCurrentWeatherInformation(weatherData);
        }

        private void HideActivityIndicator()
        {
            loader.IsRunning = false;
            loader.HeightRequest = 0;   // removes the gap (sets the height to 0)
        }

        private void ShowContentPanel()
        {
            contentPanel.IsVisible = true;
        }

        private void BindCurrentWeatherInformation(WeatherData weatherData)
        {
            // Temperature
            weatherTemp.Text = ConvertTempIntToStr(weatherData.Main.Temperature) + $"ºC";
            weatherTempMax.Text = ConvertTempIntToStr(weatherData.Main.TempMax) + $"ºC / " + ConvertTempIntToStr(weatherData.Main.TempMax) + $"ºC";

            weatherLabel.Text = weatherData.Weather[0].Visibility;
            // Details
            descriptionLabel.Text = weatherData.Weather[0].Description;
            humidityLabel.Text = weatherData.Main.Humidity.ToString() + $"%";
            pressureLabel.Text = weatherData.Main.Pressure.ToString() + $" hPa";
            windSpeedLabel.Text = weatherData.Wind.Speed.ToString() + $" meter/sec";
            windDirectionLabel.Text = weatherData.Wind.Deg.ToString() + $"º";

            weatherIcon.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, weatherData.Weather[0].Icon, Constants.OpenWeatherIconExtension);
        }

        private string ConvertTempIntToStr(double temp)
        {
            return Convert.ToInt32(temp).ToString();
           
        }

        private async void SendNextDayForecastRequest()
        {
            List<WeatherData> forecastNextDay = await _restService.GetNextDayWeatherDataAsync(GenerateNextDayRequestUri(Constants.OpenWeatherNextDayMapEndpoint));
            forecastNextDay = GetNextDayForecasts(forecastNextDay);

            // 2019 - 12 - 16 00:00:00
         
            BindNextDayForecastWeatherInformation(forecastNextDay);
        }

        private List<WeatherData> GetNextDayForecasts(List<WeatherData> forecast)
        {
            int count = 0;
            List<WeatherData> nextDay = new List<WeatherData>();

            foreach (WeatherData w in forecast)
            {
                DateTime dt = DateTime.ParseExact(w.Dt_Txt, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                
                if(dt.Day == (System.DateTime.Now.Day + 1))
                {
                    break;
                }
                count++;
            }

            int window = count + 8;
            for(int i = count; i < window; i++)
            {
                nextDay.Add(forecast[i]);
            }

            return nextDay;
        }

        private void BindNextDayForecastWeatherInformation(List<WeatherData> forecastNextDay)
        {
            icon0.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[0].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon3.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[1].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon6.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[2].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon9.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[3].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon12.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[4].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon15.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[5].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon18.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[6].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon21.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[7].Weather[0].Icon, Constants.OpenWeatherIconExtension);
        }

        private void ShowErrorMessage()
        {
            loader.IsVisible = false;
            DisplayAlert("Connection Error", "An error occurred.\nPlease try again later.", "OK");
        }

        private string GenerateIconRequestUri(string endpoint, string iconID, string fileExtension)
        {
            string requestUri = endpoint;
            requestUri += iconID;
            requestUri += fileExtension;
            return requestUri;
        }

        private string GenerateNextDayRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            requestUri += $"?id={this.id}";
            requestUri += $"&units=metric";
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }

        private string GenerateRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            requestUri += $"?id={this.id}";
            requestUri += $"&units=metric";
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }
    }
}
