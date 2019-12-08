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

        private Coord CoordObj;


        public APICityModel(string id, string name, string country, string lon, string lat)
        {
            ID = id;
            Name = name;
            Country = country;

            CoordObj = new Coord(lon, lat);

            Longitude = CoordObj.getLongitude();
            Latitude = CoordObj.getLatitude();
        } 

        public string ID
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Country
        {
            get; set;
        }

        public string Longitude
        {
            get; set;
        }

        public string Latitude
        {
            get; set;
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                APICityModel p = (APICityModel)obj;
                return ID == p.ID;
            }
        }
    }
}
