using System;
using MyWeather.Model;
using System.Collections.ObjectModel;

namespace MyWeather.ViewModel
{
    public class MyCitiesViewModel
    {
        public ObservableCollection<CityModel> MyCitiesCollection { get; set; }

        public MyCitiesViewModel()
        {
            MyCitiesCollection = new ObservableCollection<CityModel>();
            MyCitiesCollection.Add(new CityModel() { id = 123423, CityName = "Vila Real", CityCountry = "Portugal" });

            
        }
    }
}
