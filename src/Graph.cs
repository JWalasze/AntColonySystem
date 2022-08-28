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

        //Konstruktor
        public Graph(int numberOfVertexes)
        {
            this._AdjacencyList = new AdjacencyList();
        }

        //Dodanie wierzcholka do grafu - nowy vertex ma zwiekszony o 1 index
        public void AddVertex()
        {
            this._AdjacencyList.AddVertex(new Vertex());
        }

        //Zwraca ilosc wierzchokow w grafie
        public int GetNumberOfVertexes()
        {
            return this._AdjacencyList.NumberOfVertexes;
        }

        //Dodaje nieskierowana krawedz do grafu - info o wierzcholkach w newEdge
        public void AddUndirectedEdge(Edge newEdge)
        {
            this._AdjacencyList.AddUndirectedEdge(newEdge);
        }

        //Dodanie skierowana krawedz - info o wierzcholkach w newEdge
        public void AddDirectedEdge(Edge newEdge)
        {
            this._AdjacencyList.AddDirectedEdge(newEdge);
        }

        //Zwraca graf w reprezentacji listy jako string
        public override string ToString()
        {
            return this._AdjacencyList.ToString();
        }

        //Metoda zeby zwrocic krawedzie z podanego wierzcholka
        public ReadOnlyCollection<Edge> GetEdgesFromVertex(int vertexIndex)
        {
            return this._AdjacencyList[vertexIndex];
        }
    }
}
