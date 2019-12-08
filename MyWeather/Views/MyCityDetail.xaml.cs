using System;
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
            SendAPIRequest();
        }

        private void InitializeToolbar()
        {
            var toolBarItem = new ToolbarItem
            {
                Text = this.cityName + ", " + this.cityCountry
            };

            ToolbarItems.Add(toolBarItem);
        }

        private async void SendAPIRequest()
        {
            try
            {
                WeatherData weatherData = await _restService.GetWeatherDataAsync(GenerateRequestUri(Constants.OpenWeatherMapEndpoint));
                BindingContext = weatherData;

                HideActivityIndicator();
                ShowContentPanel();
            } catch (Exception e)
            {
                Debug.WriteLine("_SendAPIRequest: Connection exception occurred. " + e);
                ShowErrorMessage();
            }
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

        private void ShowErrorMessage()
        {
            loader.IsVisible = false;
            DisplayAlert("Connection Error", "An error occurred.\nPlease try again later.", "OK");
        }

        private string GenerateRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            requestUri += $"?id={this.id}";
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }
    }
}
