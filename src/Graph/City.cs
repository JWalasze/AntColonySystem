using System;

namespace AntColonyNamespace
{
    internal class City
    {
        //Property zeby zwrocic index
        public double Index { get; private set; }

        //Property zeby zwrocic demand
        public double Demand { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        //Konstruktor
        public City(int vertexIndex, double latitude, double longitude, double demand)
        {
            this.Index = vertexIndex;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Demand = demand;
        }
    }
}
