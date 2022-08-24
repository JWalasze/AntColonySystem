using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class AdjacencyList
    {
        private List<List<Edge>> _AdjacencyListOfEdges;

        public AdjacencyList()
        {
            this._AdjacencyListOfEdges = new List<List<Edge>>();
        }

        public int NumberOfVertexes
        {
            get { return this.GetNumberOfVertexes(); }
        }

        //It makes that we can access vertexes and edges through [], but it returns new, readonly collection.
        public ReadOnlyCollection<Edge> this[int vertexNumber]
        {
            get
            {
                if (vertexNumber < 0 || vertexNumber >= this._AdjacencyListOfEdges.Count)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(vertexNumber),
                        "Column index is out of range!!!"
                    );
                }
                else
                {
                    return new ReadOnlyCollection<Edge>(this._AdjacencyListOfEdges[vertexNumber]);
                }
            }
        }

        public void AddDirectedEdge(Edge newDirectedEdge)
        {
            if (
                newDirectedEdge.StartVertex >= this.NumberOfVertexes
                || newDirectedEdge.EndVertex >= this.NumberOfVertexes
                || newDirectedEdge.StartVertex < 0
                || newDirectedEdge.EndVertex < 0
            )
            {
                throw new Exception("Entered vertexes out of range!");
            }
            else
            {
                this._AdjacencyListOfEdges[newDirectedEdge.StartVertex].Add(newDirectedEdge);
            }
        }

        public void AddUndirectedEdge(Edge newUndirectedEdge)
        {
            if (
                newUndirectedEdge.StartVertex >= this.NumberOfVertexes
                || newUndirectedEdge.EndVertex >= this.NumberOfVertexes
                || newUndirectedEdge.StartVertex < 0
                || newUndirectedEdge.EndVertex < 0
            )
            {
                throw new Exception("Entered vertexes out of range!");
            }
            else
            {
                this._AdjacencyListOfEdges[newUndirectedEdge.StartVertex].Add(newUndirectedEdge);
                this._AdjacencyListOfEdges[newUndirectedEdge.EndVertex].Add(newUndirectedEdge);
            }
        }

        public void AddVertex()
        {
            this._AdjacencyListOfEdges.Add(new List<Edge>());
        }

        private int GetNumberOfVertexes()
        {
            return this._AdjacencyListOfEdges.Count;
        }

        private bool IsEdgeBetweenVertexes(int firstVertex, int secondVertex)
        {
            if (this._AdjacencyListOfEdges[firstVertex].Any(edge => edge.EndVertex == secondVertex))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            string str = string.Empty;
            int counterOfVertexes = 0;
            this._AdjacencyListOfEdges.ForEach(vertex =>
            {
                str += counterOfVertexes++ + ":";
                vertex.ForEach(edge =>
                {
                    str += " " + edge.EndVertex;
                });
                str += Environment.NewLine;
            });
            return str;
        }
    }
}
