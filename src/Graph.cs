using System;

namespace AntColonyNamespace
{
    internal class Graph
    {
        private AdjacencyList _AdjacencyList;

        public AdjacencyList AdjacencyList
        {
            get { return this._AdjacencyList; }
        }

        public Graph(int numberOfVertexes)
        {
            this._AdjacencyList = new AdjacencyList();
        }

        public void AddVertex()
        {
            this._AdjacencyList.AddVertex();
        }

        public int GetNumberOfVertexes()
        {
            return this._AdjacencyList.NumberOfVertexes;
        }

        public void AddUndirectedEdge(Edge newEdge)
        {
            this._AdjacencyList.AddUndirectedEdge(newEdge);
        }

        public void AddDirectedEdge(Edge newEdge)
        {
            this._AdjacencyList.AddDirectedEdge(newEdge);
        }

        public override string ToString()
        {
            return this._AdjacencyList.ToString();
        }
    }
}
