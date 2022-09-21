namespace AntColonyNamespace
{
    internal class EdgeWithDestinationCity : Edge
    {
        public int DestinationCity { get; private set; } //DAC ze jak aktualizujemy to wtedy logika zeby obydwie krawedzie zrobilo

        public EdgeWithDestinationCity(Edge Edge, int DestCity)
            : base(Edge.Distance, Edge.InitialPheromoneLevel)
        {
            this.DestinationCity = DestCity;
        }
    }
}
