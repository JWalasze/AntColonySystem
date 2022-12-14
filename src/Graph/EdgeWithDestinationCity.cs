namespace AntColonyNamespace
{
    internal class EdgeWithDestinationCity
    {
        public int DestinationCity { get; private set; }

        public Edge EdgeToDestCity { get; private set; }

        public EdgeWithDestinationCity(Edge edgeToDestCity, int destinationCity)
        {
            this.EdgeToDestCity = edgeToDestCity;
            this.DestinationCity = destinationCity;
        }
    }
}
