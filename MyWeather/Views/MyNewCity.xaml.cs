using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using MyWeather.Model;
using MyWeather.ViewModel;

namespace MyWeather.Views
{
    public partial class MyNewCity : ContentPage
    {
        private MyCitiesViewModel model;

        public MyNewCity(MyCitiesViewModel model)
        {
            InitializeComponent();
            InitializeListView();
            this.model = model;
        }

        private void InitializeListView()
        {
            citySearchResults.ItemsSource = CitiesData.Cities;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            citySearchResults.ItemsSource = CitiesData.Cities.Where(p => p.Name.ToLower().Contains(searchBar.Text.ToLower()));
        }

        private async void AddToMyCities(Object sender, ItemTappedEventArgs e)
        {
            var details = e.Item as APICityModel;

            model.AddNewMyCity(details);

            await Navigation.PopAsync();
        }

    }
}
