using System;
using System.Diagnostics;
using MyWeather.Connection;
using MyWeather.Data;

using Xamarin.Forms;

namespace MyWeather.Views
{
    public partial class MyCityDetail : ContentPage
    {
        private int id;
        private RESTService _restService;

        public MyCityDetail(int id, string cityName, string cityCountry)
        {
            InitializeComponent();
            Debug.WriteLine("enteref");
            this.id = id;
            _restService = new RESTService();

            SendAPIRequest();
        }

        private async void SendAPIRequest()
        {
            try
            {
                WeatherData weatherData = await _restService.GetWeatherDataAsync(GenerateRequestUri(Constants.OpenWeatherMapEndpoint));
                BindingContext = weatherData;
            } catch (Exception e)
            {
                Debug.WriteLine("_SendAPIRequest: Connection exception occurred.");
                ShowErrorMessage();
            }
        }

        private void ShowErrorMessage()
        {
            loader.IsVisible = false;
            DisplayAlert("Connection Error", "An error occurred.\nPlease try again later.", "OK");
        }

        private string GenerateRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            requestUri += $"?q={this.id}";
            requestUri += "&units=imperial"; // or units=metric
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }
    }
}
