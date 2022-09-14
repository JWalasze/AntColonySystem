namespace AntColonyNamespace
{
    internal class CompletedGraph : Graph
    {
        public CompletedGraph(int numberOfCities) : base(numberOfCities)
        {
            foreach (var i in this.AdjacencyList) { }
        }
    }
}
