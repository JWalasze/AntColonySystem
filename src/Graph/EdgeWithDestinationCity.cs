namespace AntColonyNamespace
{
    internal class EdgeWithDestinationCity
    {
        public int DestinationCity { get; private set; }

        public Edge EdgeToDestinationCity { get; private set; }

        public EdgeWithDestinationCity(Edge Edge, int DestCity)
        {
            this.EdgeToDestinationCity = Edge;
            this.DestinationCity = DestCity;
        }
    }
}
