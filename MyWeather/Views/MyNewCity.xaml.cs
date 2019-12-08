using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MyWeather.Views
{
    public partial class MyNewCity : ContentPage
    {
        public MyNewCity()
        {
            InitializeComponent();
        }

        private async void SearchCity(object sender, EventArgs e)
        {

        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            citySearchResults.ItemsSource = /*DataService.GetSearchResults(searchBar.Text);*/ null;
        }
    }
}
