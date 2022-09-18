namespace AntColonyNamespace
{
    internal class EdgeWithDestinationCity : Edge
    {
        public int DestinationCity { get; private set; }

        public EdgeWithDestinationCity(Edge Edge, int DestCity)
            : base(Edge.Distance, Edge.PheromoneLevel)
        {
            this.DestinationCity = DestCity;
        }
    }
}