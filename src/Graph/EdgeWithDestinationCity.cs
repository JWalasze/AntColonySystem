namespace AntColonyNamespace
{
    internal class EdgeWithDestinationCity
    {
        public int DestinationCity { get; private set; }

        public Edge EdgeToDestCity { get; private set; }

        public EdgeWithDestinationCity(Edge Edge, int DestCity)
        {
            this.EdgeToDestCity = Edge;
            this.DestinationCity = DestCity;
        }
    }
}
