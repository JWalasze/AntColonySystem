namespace AntColonyNamespace
{
    internal class CompletedGraph : Graph
    {
        public CompletedGraph(int numberOfCities) : base(numberOfCities)
        {
            foreach (var i in this.AdjacencyList)
            {
                var p = ((Vertex _Vertex, List<Edge> _Edges))i;
                Console.WriteLine(p._Vertex);
            }
        }

        public CompletedGraph(Graph g) : base()
        {
            foreach (var i in g.AdjacencyList)
            {
                var p = ((Vertex _Vertex, List<Edge> _Edges))i;
                Console.WriteLine(p._Vertex.VertexIndex);
            }
        }
    }
}
