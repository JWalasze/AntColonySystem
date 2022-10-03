namespace AntColonyNamespace
{
    internal class City
    {
        public double Index { get; private set; }

        public double Demand { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public City(int cityIndex, double latitude, double longitude, double demand)
        {
            this.Index = cityIndex;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Demand = demand;
        }
    }
}
