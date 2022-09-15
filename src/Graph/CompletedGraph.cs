namespace AntColonyNamespace
{
    internal class CompletedGraph : Graph
    {
        public CompletedGraph(int numberOfCities) : base(numberOfCities)
        {
            for (int i = 0, k = numberOfCities; i < numberOfCities; ++i, --k)
            {
                for (int j = k - 1; j > 0; --j)
                {
                    this.AdjacencyList.AddUndirectedEdge(
                        new Edge(i, numberOfCities - j, new Random().Next())
                    );
                }
            }
            Console.WriteLine(this.ToString());
        }

        // public CompletedGraph(Graph graph) : base(graph.AdjacencyList.NumberOfVertexes)
        // {
        //     foreach ((Vertex _Vertex, List<Edge> _Edges) tuple in graph.AdjacencyList)
        //     {
        //         //var castedTuple = ((Vertex _Vertex, List<Edge> _Edges))tuple;
        //         tuple._Edges.ForEach(edge => {
        //             this.AdjacencyList.
        //         });
        //     }
        // }
    }
}
