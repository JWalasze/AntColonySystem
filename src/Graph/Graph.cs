using System;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class Graph
    {
        //Lista sasiedztwa grafu
        private AdjacencyList _AdjacencyList;

        //Zwraca liste sasiedztwa
        public AdjacencyList AdjacencyList
        {
            get { return this._AdjacencyList; }
        }

        //Konstruktory
        public Graph(int NumberOfVertexes)
        {
            this._AdjacencyList = new AdjacencyList();
            for (int i = 0; i < NumberOfVertexes; ++i)
            {
                this.AddVertex();
            }
        }

        public Graph()
        {
            this._AdjacencyList = new AdjacencyList();
        }

        //Dodanie wierzcholka do grafu - nowy vertex ma zwiekszony o 1 index
        public void AddVertex()
        {
            this._AdjacencyList.AddVertex();
        }

        //Zwraca ilosc wierzchokow w grafie
        public int GetNumberOfVertexes()
        {
            return this._AdjacencyList.NumberOfVertexes;
        }

        public Vertex GetVertex(int vertexIndex)
        {
            return this._AdjacencyList.GetVertex(vertexIndex);
        }

        //Dodaje nieskierowana krawedz do grafu - info o wierzcholkach w newEdge
        public void AddUndirectedEdge(int firstVertexIndex, int secondVertexIndex, Edge newEdge)
        {
            this._AdjacencyList.AddUndirectedEdge(firstVertexIndex, secondVertexIndex, newEdge);
        }

        //Dodanie skierowana krawedz - info o wierzcholkach w newEdge
        public void AddDirectedEdge(int startVertexIndex, int endVertexIndex, Edge newEdge)
        {
            this._AdjacencyList.AddDirectedEdge(startVertexIndex, endVertexIndex, newEdge);
        }

        //Metoda zeby zwrocic krawedzie z podanego wierzcholka
        public ReadOnlyCollection<EdgeWithDestVertex> GetEdgesFromVertex(int vertexIndex)
        {
            return this._AdjacencyList[vertexIndex];
        }

        //Zwraca graf w reprezentacji listy jako string
        public override string ToString()
        {
            return this._AdjacencyList.ToString();
        }
    }
}
