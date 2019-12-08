﻿using System;
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
            // MyCitiesCollection.Add(CitiesData.Cities[0]);
        }

        public void AddNewMyCity(APICityModel newCity)
        {
            if (MyCitiesCollection.Contains(newCity))
            {
                return;
            }
            MyCitiesCollection.Add(newCity);
        }
    }
}
