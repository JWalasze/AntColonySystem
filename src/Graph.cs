using System;

namespace AntColonyNamespace
{
    internal class Graph
    {
        //Adjacency list
        private List<LinkedList<Vertex>> adjacencyList;

        public Graph(int numberOfVertexes)
        {
            this.adjacencyList = new List<LinkedList<Vertex>>(numberOfVertexes);

            for (int i = 0; i < numberOfVertexes; ++i)
            {
                this.adjacencyList.Add(new LinkedList<Vertex>());
                // this.adjacencyList[i] = new LinkedList<Vertex>();
            }
        }

        public void AddVertex()
        {
            this.adjacencyList.Add(new LinkedList<Vertex>());
        }

        public int GetNumberOfVertexes()
        {
            return this.adjacencyList.Count;
        }

        public void AddUndirectedEdge(int startVertex, int endVertex)
        {
            if (this.adjacencyList[startVertex].Count == 0)
            {
                this.adjacencyList[startVertex].AddFirst(new Vertex());
            }
            else
            {
                this.adjacencyList[startVertex].AddLast(new Vertex());
            }

            if (this.adjacencyList[endVertex].Count == 0)
            {
                this.adjacencyList[endVertex].AddFirst(new Vertex());
            }
            else
            {
                this.adjacencyList[endVertex].AddLast(new Vertex());
            }
        }

        public void AddDirectedEdge(int startVertex, int endVertex)
        {
            if (this.adjacencyList[startVertex].Count == 0)
            {
                this.adjacencyList[startVertex].AddFirst(new Vertex());
            }
            else
            {
                this.adjacencyList[startVertex].AddLast(new Vertex());
            }
        }

        public override string ToString()
        {
            string str = string.Empty;
            int counterOfVertexes = 0;
            foreach (LinkedList<Vertex> linkedListOfVertex in adjacencyList)
            {
                str += "Vertex " + counterOfVertexes++ + ": head";
                foreach (Vertex vertex in linkedListOfVertex)
                {
                    str += " -> " + vertex.VertexIndex;
                }
                str += Environment.NewLine;
            }
            return str;
        }
    }
}
