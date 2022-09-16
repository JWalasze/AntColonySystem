namespace AntColonyNamespace
{
    internal class CompletedGraph : Graph
    {
        public CompletedGraph(int numberOfCities) : base(numberOfCities)
        {
            for (
                int currentVertexStart = 0, numberEdgesToCreate = numberOfCities;
                currentVertexStart < numberOfCities;
                ++currentVertexStart, --numberEdgesToCreate
            )
            {
                for (
                    int currentVertexEnd = currentVertexStart + 1;
                    currentVertexEnd < numberOfCities;
                    ++currentVertexEnd
                )
                {
                    this.AdjacencyList.AddUndirectedEdge(
                        currentVertexStart,
                        currentVertexEnd,
                        new Edge(new Random().Next())
                    );
                }
            }
            Console.WriteLine(this.ToString());
        }
    }
}
