using System;
namespace MyWeather.Model
{
    public class APICityModel
    {
        class Coord
        {
            private string lon;
            private string lat;

            public Coord(string lon, string lat)
            {
                this.lon = lon;
                this.lat = lat;
            }

            public string getLongitude()
            {
                return lon;
            }

            public string getLatitude()
            {
                return lat;
            }
        }

        private string id;
        private string name;
        private string country;
        private Coord coord;
        

        public APICityModel(string id, string name, string country, string lon, string lat)
        {
            this.id = id;
            this.name = name;
            this.country = country;
            this.coord = new Coord(lon, lat);
        }


    }
}
