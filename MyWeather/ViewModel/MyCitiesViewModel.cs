using System;
using MyWeather.Model;
using System.Collections.ObjectModel;

namespace MyWeather.ViewModel
{
    public class MyCitiesViewModel
    {
        public ObservableCollection<APICityModel> MyCitiesCollection { get; set; }

        public MyCitiesViewModel()
        {
            MyCitiesCollection = new ObservableCollection<APICityModel>();
        }

        public void AddNewMyCity(APICityModel newCity)
        {
            if (MyCitiesCollection.Contains(newCity))
            {
                return;
            }
            MyCitiesCollection.Add(newCity);
        }

        public void RemoveMyCity(APICityModel city)
        {
            // TODO: remove my city feature
        }
    }
}
