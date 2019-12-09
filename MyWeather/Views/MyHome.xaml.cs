using System;
using System.Collections.Generic;
using MyWeather.ViewModel;
using MyWeather.Model;

using Xamarin.Forms;

namespace MyWeather.Views
{
    public partial class MyHome : ContentPage
    {
        private MyCitiesViewModel model;

        public MyHome()
        {
            InitializeComponent();

            model = new MyCitiesViewModel();

            BindingContext = model;
        }

        protected override void OnAppearing()
        {
            if (model.MyCitiesCollection.Count == 0)
            {
                savedCitiesList.IsVisible = false;
                noCitiesPanel.IsVisible = true;
            }
            else
            {
                savedCitiesList.IsVisible = true;
                noCitiesPanel.IsVisible = false;
            }
        }


        private async void OnItemSelected(Object sender, ItemTappedEventArgs e)
        {   
            var details = e.Item as APICityModel;
            await Navigation.PushAsync(new MyCityDetail(details.ID, details.Name, details.Country));
        }

        private async void OpenNewCityPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyNewCity(model));
        }
    }
}
