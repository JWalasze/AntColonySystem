using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class AdjacencyList /*: IEnumerable*/
    {
        //Lista krotki: Wierzcholek z lista krawedzi z niego
        private List<(Vertex _Vertex, List<Edge> _Edges)> _AdjacencyList;

        //Konstruktor
        public AdjacencyList()
        {
            this._AdjacencyList = new List<(Vertex _Vertex, List<Edge> _Edges)>();
        }

        //Zwraca ilosc wierzcholkow w grafie jako pole property
        public int NumberOfVertexes
        {
            get { return this.GetNumberOfVertexes(); }
        }

        // IEnumerator IEnumerable.GetEnumerator()
        // {
        //     return (IEnumerator)GetEnumerator();
        // }

        // public AdjacencyListEnum GetEnumerator()
        // {
        //     return new AdjacencyListEnum(this._AdjacencyList);
        // }

        //Pozwala nam na dostep do krawedzi danego wierzcholka za pomoca []
        public ReadOnlyCollection<Edge> this[int vertexNumber]
        {
            get
            {
                if (vertexNumber < 0 || vertexNumber >= this.NumberOfVertexes)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(vertexNumber),
                        "Vertex index is out of range!!!"
                    );
                }
                else
                {
                    return new ReadOnlyCollection<Edge>(this._AdjacencyList[vertexNumber]._Edges);
                }
            }
        }

        //Dodanie skierowanej krawedzi do listy sasiedztwa
        public void AddDirectedEdge(Edge newDirectedEdge)
        {
            if (
                newDirectedEdge.StartVertex >= this.NumberOfVertexes
                || newDirectedEdge.EndVertex >= this.NumberOfVertexes
                || newDirectedEdge.StartVertex < 0
                || newDirectedEdge.EndVertex < 0
            )
            {
                throw new Exception("Incorrect new edge!!!");
            }
            else
            {
                this._AdjacencyList[newDirectedEdge.StartVertex]._Edges.Add(newDirectedEdge);
            }
        }

        //Dodanie nieskierowanej krawedzi do listy sasiedztwa
        public void AddUndirectedEdge(Edge newUndirectedEdge)
        {
            if (
                newUndirectedEdge.StartVertex >= this.NumberOfVertexes
                || newUndirectedEdge.EndVertex >= this.NumberOfVertexes
                || newUndirectedEdge.StartVertex < 0
                || newUndirectedEdge.EndVertex < 0
            )
            {
                throw new Exception("Incorrect new edge!!!");
            }
            else
            {
                this._AdjacencyList[newUndirectedEdge.StartVertex]._Edges.Add(newUndirectedEdge);
                this._AdjacencyList[newUndirectedEdge.EndVertex]._Edges.Add(newUndirectedEdge);
            }
        }

        public Vertex GetVertex(int vertexIndex)
        {
            return this._AdjacencyList
                .Find(element => element._Vertex.VertexIndex == vertexIndex)
                ._Vertex;
        }

        //Dodanie nowego wierzcholka
        public void AddVertex(Vertex newVertex)
        {
            this._AdjacencyList.Add((newVertex, new List<Edge>()));
        }

        //Zwraca ilosc wierzcholkow - uzyte w property NumberOfVertexes
        private int GetNumberOfVertexes()
        {
            return this._AdjacencyList.Count;
        }

        //Zwraca liste sasiedztwa jako string
        public override string ToString()
        {
            string str = string.Empty;
            this._AdjacencyList.ForEach(item =>
            {
                str += item._Vertex.VertexIndex + ":";
                item._Edges.ForEach(edge =>
                {
                    str += " " + edge.EndVertex;
                });
                str += Environment.NewLine;
            });
            return str;
        }
    }

    // internal class AdjacencyListEnum : IEnumerator
    // {
    //     private List<(Vertex _Vertex, List<Edge> _Edges)> _AdjacencyList;

    //     private int position = -1;

    //     public AdjacencyListEnum(List<(Vertex _Vertex, List<Edge> _Edges)> AdjacencyList)
    //     {
    //         this._AdjacencyList = AdjacencyList;
    //     }

    //     public bool MoveNext()
    //     {
    //         ++this.position;
    //         return this.position < this._AdjacencyList.Count;
    //     }

    //     public void Reset()
    //     {
    //         this.position = -1;
    //     }

    //     object IEnumerator.Current
    //     {
    //         get { return Current; }
    //     }

    //     public (Vertex _Vertex, List<Edge> _Edges) Current
    //     {
    //         get
    //         {
    //             try
    //             {
    //                 return this._AdjacencyList[position];
    //             }
    //             catch (IndexOutOfRangeException)
    //             {
    //                 throw new InvalidOperationException();
    //             }
    //         }
    //     }
    // }
}
