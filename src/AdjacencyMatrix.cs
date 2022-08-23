using System;

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

        // public Edge this[int columnIndex, int rowIndex]
        // {
        //     get
        //     {
        //         if (columnIndex < 0 || columnIndex >= this._AdjacencyListOfEdges.Count)
        //         {
        //             throw new ArgumentOutOfRangeException(
        //                 nameof(columnIndex),
        //                 "Column index is out of range!!!"
        //             );
        //         }
        //         else if (rowIndex < 0 || rowIndex >= this._AdjacencyListOfEdges.Count)
        //         {
        //             throw new ArgumentOutOfRangeException(
        //                 nameof(rowIndex),
        //                 "Row index is out of range!!!"
        //             );
        //         }
        //         else
        //         {
        //             return this._AdjacencyListOfEdges[columnIndex][rowIndex];
        //         }
        //     }
        // }

        public void AddDirectedEdge(Edge newDirectedEdge, int startVertex, int endVertex)
        {
            if (startVertex >= this.NumberOfVertexes || endVertex >= this.NumberOfVertexes)
            {
                throw new Exception("Entered vertexes out of range!");
            }
            else
            {
                this._AdjacencyListOfEdges[startVertex].Add(newDirectedEdge);
            }
        }

        public void AddUndirectedEdge(Edge newUndirectedEdge, int startVertex, int endVertex)
        {
            if (startVertex >= this.NumberOfVertexes || endVertex >= this.NumberOfVertexes)
            {
                throw new Exception("Entered vertexes out of range!");
            }
            else
            {
                this._AdjacencyListOfEdges[startVertex].Add(newUndirectedEdge);
                this._AdjacencyListOfEdges[endVertex].Add(newUndirectedEdge);
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

        private bool IsEdgeBetweenVertexes(int firstVertex, int secondVertex) { 
            this._AdjacencyListOfEdges[firstVertex].ForEach(n => )
        }

        public override string ToString()
        {
            string str = string.Empty;
            int counterOfVertexes = 0;
            //Do poprawy...foreach do listy a nie tak samo XD
            foreach (List<Edge> listOfVertexes in this._AdjacencyListOfEdges)
            {
                str += counterOfVertexes++ + ":";
                foreach (Edge edge in listOfVertexes)
                {
                    str += " " + edge.VertexIndex;
                }
                str += Environment.NewLine;
            }
            return str;
        }
    }
}
