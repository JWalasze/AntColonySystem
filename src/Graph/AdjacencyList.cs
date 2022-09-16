using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class AdjacencyList /*: IEnumerable*/
    {
        //Lista krotki: Wierzcholek z lista krawedzi z niego
        private List<(Vertex _Vertex, List<EdgeWithDestVertex> _Edges)> _AdjacencyList;

        //Konstruktor
        public AdjacencyList()
        {
            this._AdjacencyList = new List<(Vertex _Vertex, List<EdgeWithDestVertex> _Edges)>();
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
        public ReadOnlyCollection<EdgeWithDestVertex> this[int vertexNumber]
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
                    return new ReadOnlyCollection<EdgeWithDestVertex>(
                        this._AdjacencyList[vertexNumber]._Edges
                    );
                }
            }
        }

        //Dodanie skierowanej krawedzi do listy sasiedztwa
        public void AddDirectedEdge(int startVertexIndex, int endVertexIndex, Edge newDirectedEdge)
        {
            if (
                startVertexIndex >= this.NumberOfVertexes
                || endVertexIndex >= this.NumberOfVertexes
                || startVertexIndex < 0
                || endVertexIndex < 0
            )
            {
                throw new Exception("Incorrect new edge!!!");
            }
            else
            {
                this._AdjacencyList[startVertexIndex]._Edges.Add(
                    new EdgeWithDestVertex(newDirectedEdge, endVertexIndex)
                );
            }
        }

        //Dodanie nieskierowanej krawedzi do listy sasiedztwa
        public void AddUndirectedEdge(
            int firstVertexIndex,
            int secondVertexIndex,
            Edge newUndirectedEdge
        )
        {
            if (
                firstVertexIndex >= this.NumberOfVertexes
                || secondVertexIndex >= this.NumberOfVertexes
                || firstVertexIndex < 0
                || secondVertexIndex < 0
            )
            {
                throw new Exception("Incorrect new edge!!!");
            }
            else
            {
                this._AdjacencyList[firstVertexIndex]._Edges.Add(
                    new EdgeWithDestVertex(newUndirectedEdge, secondVertexIndex)
                );
                this._AdjacencyList[secondVertexIndex]._Edges.Add(
                    new EdgeWithDestVertex(newUndirectedEdge, firstVertexIndex)
                );
            }
        }

        //Zwykly get na wierzcholek
        public Vertex GetVertex(int vertexIndex)
        {
            return this._AdjacencyList.Find(tuple => tuple._Vertex.Index == vertexIndex)._Vertex;
        }

        //Dodanie nowego wierzcholka
        public void AddVertex()
        {
            this._AdjacencyList.Add(
                (new Vertex(this.GetNumberOfVertexes()), new List<EdgeWithDestVertex>())
            );
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
            this._AdjacencyList.ForEach(tuple =>
            {
                str += tuple._Vertex.Index + ":";
                tuple._Edges.ForEach(edgeWithDestVertex =>
                {
                    str += " " + edgeWithDestVertex._DestVertex;
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
