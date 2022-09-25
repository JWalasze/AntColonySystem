namespace AntColonyNamespace
{
    internal class City
    {
        public double Index { get; protected set; }

        public double Demand { get; protected set; }

        public double Latitude { get; protected set; }

        public double Longitude { get; protected set; }

        public City(int cityIndex, double latitude, double longitude, double demand)
        {
            this.Index = cityIndex;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Demand = demand;
        }
    }
}
