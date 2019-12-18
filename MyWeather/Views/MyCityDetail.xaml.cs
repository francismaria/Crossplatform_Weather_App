using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MyWeather.Connection;
using MyWeather.Data;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace MyWeather.Views
{
    public partial class MyCityDetail : ContentPage
    {
        private string id;
        private string cityName;
        private string cityCountry;
        private List<int> nextDayTemperatures;
        private int nextDayTempDiff;
        private int minTempNextDay;
        private int maxTempNextDay;

        private RESTService _restService;

        public MyCityDetail(string id, string cityName, string cityCountry)
        {
            InitializeComponent();

            this.id = id;
            this.cityName = cityName;
            this.cityCountry = cityCountry;
            this.nextDayTemperatures = new List<int>();

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
                await SendCurrentWeatherRequest();
                await SendNextDayForecastRequest();

                // Forces the skia chart graphic to paint just after it got the response from the server
                canvasView.PaintSurface += OnCanvasViewPaintSurface;
                canvasView.InvalidateSurface();

                HideActivityIndicator();
            }
            catch (Exception e)
            {
                Debug.WriteLine("_SendAPIRequest: Connection exception occurred. " + e);
                ShowErrorMessage();
            }
        }

        private async Task SendCurrentWeatherRequest()
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

            //weatherIcon.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, weatherData.Weather[0].Icon, Constants.OpenWeatherIconExtension);
            weatherIcon.Source = ImageSource.FromFile(GetIconPath(weatherData.Weather[0].Icon));
            weatherIcon.WidthRequest = 100;
            weatherIcon.HeightRequest = 100;
        }

        private string ConvertTempIntToStr(double temp)
        {
            return Convert.ToInt32(temp).ToString();
        }

        private async Task SendNextDayForecastRequest()
        {
            List<WeatherData> forecastNextDay = await _restService.GetNextDayWeatherDataAsync(GenerateNextDayRequestUri(Constants.OpenWeatherNextDayMapEndpoint));
            forecastNextDay = GetNextDayForecasts(forecastNextDay);
            BindNextDayForecastWeatherInformation(forecastNextDay);
            GetMaxTempNextDayForecast();
        }

        private List<WeatherData> GetNextDayForecasts(List<WeatherData> forecast)
        {
            int count = 0;
            List<WeatherData> nextDay = new List<WeatherData>();

            foreach (WeatherData w in forecast)
            {
                DateTime dt = DateTime.ParseExact(w.Dt_Txt, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                if (dt.Day == (DateTime.Now.Day + 1))
                {
                    break;
                }
                count++;
            }

            int window = count + 8;

            for (int i = count; i < window; i++)
            {
                nextDay.Add(forecast[i]);
            }

            return nextDay;
        }

        //    <!--PaintSurface="OnCanvasViewPaintSurface"-->

        private void BindNextDayForecastWeatherInformation(List<WeatherData> forecastNextDay)
        {
            /*icon0.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[0].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon3.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[1].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon6.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[2].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon9.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[3].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon12.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[4].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon15.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[5].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon18.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[6].Weather[0].Icon, Constants.OpenWeatherIconExtension);
            icon21.Source = GenerateIconRequestUri(Constants.OpenWeatherIconsEndpoint, forecastNextDay[7].Weather[0].Icon, Constants.OpenWeatherIconExtension);*/

            // 00h
            icon0.Source = ImageSource.FromFile(GetIconPath(forecastNextDay[0].Weather[0].Icon));
            hour00Temp.Text = ((int)forecastNextDay[0].Main.Temperature).ToString() + "ºC";
            nextDayTemperatures.Add((int)forecastNextDay[0].Main.Temperature);
            // 03h
            icon3.Source = ImageSource.FromFile(GetIconPath(forecastNextDay[1].Weather[0].Icon));
            hour03Temp.Text = ((int)forecastNextDay[1].Main.Temperature).ToString() + "ºC";
            nextDayTemperatures.Add((int)forecastNextDay[1].Main.Temperature);
            // 06h
            icon6.Source = ImageSource.FromFile(GetIconPath(forecastNextDay[2].Weather[0].Icon));
            hour06Temp.Text = ((int)forecastNextDay[2].Main.Temperature).ToString() + "ºC";
            nextDayTemperatures.Add((int)forecastNextDay[2].Main.Temperature);
            // 09h
            icon9.Source = ImageSource.FromFile(GetIconPath(forecastNextDay[3].Weather[0].Icon));
            hour09Temp.Text = ((int)forecastNextDay[3].Main.Temperature).ToString() + "ºC";
            nextDayTemperatures.Add((int)forecastNextDay[3].Main.Temperature);
            // 12h
            icon12.Source = ImageSource.FromFile(GetIconPath(forecastNextDay[4].Weather[0].Icon));
            hour12Temp.Text = ((int)forecastNextDay[4].Main.Temperature).ToString() + "ºC";
            nextDayTemperatures.Add((int)forecastNextDay[4].Main.Temperature);
            // 15h
            icon15.Source = ImageSource.FromFile(GetIconPath(forecastNextDay[5].Weather[0].Icon));
            hour15Temp.Text = ((int)forecastNextDay[5].Main.Temperature).ToString() + "ºC";
            nextDayTemperatures.Add((int)forecastNextDay[5].Main.Temperature);
            // 18h
            icon18.Source = ImageSource.FromFile(GetIconPath(forecastNextDay[6].Weather[0].Icon));
            hour18Temp.Text = ((int)forecastNextDay[6].Main.Temperature).ToString() + "ºC";
            nextDayTemperatures.Add((int)forecastNextDay[6].Main.Temperature);
            // 21h
            icon21.Source = ImageSource.FromFile(GetIconPath(forecastNextDay[7].Weather[0].Icon));
            hour21Temp.Text = ((int)forecastNextDay[7].Main.Temperature).ToString() + "ºC";
            nextDayTemperatures.Add((int)forecastNextDay[7].Main.Temperature);
        }

        private void GetMaxTempNextDayForecast()
        {
            maxTempNextDay = nextDayTemperatures[0];
            minTempNextDay = nextDayTemperatures[0];

            for (int i = 1; i < nextDayTemperatures.Count; i++)
            {
                if(nextDayTemperatures[i] > maxTempNextDay)
                {
                    maxTempNextDay = nextDayTemperatures[i];
                }
                if (nextDayTemperatures[i] < minTempNextDay)
                {
                    minTempNextDay = nextDayTemperatures[i];
                }
            }

            nextDayTempDiff = Math.Abs(maxTempNextDay) - Math.Abs(minTempNextDay);
        }


        private void ShowErrorMessage()
        {
            loader.IsVisible = false;
            DisplayAlert("Connection Error", "An error occurred.\nPlease try again later.", "OK");
        }

        // Due to some Android bugs we can not fetch the images from the internet.
        // Thus, we have every image for the icons.

        // LOCAL
        private string GetIconPath(string iconID)
        {
            string requestUri = "icon_";
            requestUri += iconID;
            return requestUri;
        }

        // CONNECTED
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


        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint linePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.DeepSkyBlue,
                StrokeWidth = 5
            };

            SKPaint pointPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.OrangeRed,
                StrokeWidth = 1,
                StrokeJoin = SKStrokeJoin.Round
            };

            const int CHART_HEIGHT = 100;
            const int REMAINING_DAYS_TEMP = 7;
            const int STACKS_MARGIN = 20;

            double stackWidth = icon12.Width;

            SKPoint prevPoint = new SKPoint((float)stackWidth, CHART_HEIGHT - GetRatio(nextDayTemperatures[0]));

            for (int i = 1; i <= REMAINING_DAYS_TEMP; i++)
            {
                // iOS scale factor = 1.8
                // Android scale factor = 3.5
                SKPoint newPoint = new SKPoint(prevPoint.X + ((float)stackWidth * (float)3.5 + STACKS_MARGIN), CHART_HEIGHT - GetRatio(nextDayTemperatures[i]));

                canvas.DrawLine(prevPoint, newPoint, linePaint);

                canvas.DrawCircle(prevPoint.X, prevPoint.Y, 10, pointPaint);
                canvas.DrawCircle(newPoint.X, newPoint.Y, 10, pointPaint);

                prevPoint = newPoint;
            }
        }

        private int GetRatio(int currTemp)
        {
            int diff = nextDayTempDiff - (maxTempNextDay-currTemp);
            return ((Math.Abs(diff) - nextDayTempDiff) * 100) / nextDayTempDiff;
        }
    }
}
