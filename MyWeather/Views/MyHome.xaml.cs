using System;
using System.Collections.Generic;
using MyWeather.ViewModel;
using MyWeather.Model;

using Xamarin.Forms;

namespace MyWeather.Views
{
    public partial class MyHome : ContentPage
    {
        public MyHome()
        {
            InitializeComponent();
            BindingContext = new MyCitiesViewModel();
        }

        private async void OnItemSelected(Object sender, ItemTappedEventArgs e)
        {   
            var details = e.Item as CityModel;
            await Navigation.PushAsync(new MyCityDetail(details.id, details.CityName, details.CityCountry));
        }

        private async void OpenNewCityPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyNewCity());
        }
    }
}
