namespace AntColonyNamespace
{
    internal class EdgeWithDestVertex
    {
        public Edge _Edge { get; private set; }

        public int _DestVertex { get; private set; }

        public EdgeWithDestVertex(Edge Edge, int DestVertex)
        {
            this._Edge = Edge;
            this._DestVertex = DestVertex;
        }
    }
}
