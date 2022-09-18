using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class AdjacencyList /*: IEnumerable*/
    {
        //Lista krotki: Wierzcholek z lista krawedzi z niego
        private List<(City _City, List<EdgeWithDestinationCity> _Edges)> _AdjacencyList;

        //Konstruktor
        public AdjacencyList()
        {
            this._AdjacencyList = new List<(City _City, List<EdgeWithDestinationCity> _Edges)>();
        }

        //Zwraca ilosc wierzcholkow w grafie jako pole property
        public int NumberOfCities
        {
            get { return this.GetNumberOfCities(); }
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
        public ReadOnlyCollection<EdgeWithDestinationCity> this[int cityNumber]
        {
            get
            {
                if (cityNumber < 0 || cityNumber >= this.NumberOfCities)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(cityNumber),
                        "City index is out of range!!!"
                    );
                }
                else
                {
                    return new ReadOnlyCollection<EdgeWithDestinationCity>(
                        this._AdjacencyList[cityNumber]._Edges
                    );
                }
            }
        }

        //Dodanie skierowanej krawedzi do listy sasiedztwa
        public void AddDirectedEdge(int startCityIndex, int endCityIndex, Edge newDirectedEdge)
        {
            if (
                startCityIndex >= this.NumberOfCities
                || endCityIndex >= this.NumberOfCities
                || startCityIndex < 0
                || endCityIndex < 0
            )
            {
                throw new Exception("Incorrect new edge!!!");
            }
            else
            {
                this._AdjacencyList[startCityIndex]._Edges.Add(
                    new EdgeWithDestinationCity(newDirectedEdge, endCityIndex)
                );
            }
        }

        //Dodanie nieskierowanej krawedzi do listy sasiedztwa
        public void AddUndirectedEdge(
            int firstCityIndex,
            int secondCityIndex,
            Edge newUndirectedEdge
        )
        {
            if (
                firstCityIndex >= this.NumberOfCities
                || secondCityIndex >= this.NumberOfCities
                || firstCityIndex < 0
                || secondCityIndex < 0
            )
            {
                throw new Exception("Incorrect new edge!!!");
            }
            else
            {
                this._AdjacencyList[firstCityIndex]._Edges.Add(
                    new EdgeWithDestinationCity(newUndirectedEdge, secondCityIndex)
                );
                this._AdjacencyList[secondCityIndex]._Edges.Add(
                    new EdgeWithDestinationCity(newUndirectedEdge, firstCityIndex)
                );
            }
        }

        //Zwykly get na wierzcholek
        public City GetCity(int cityIndex)
        {
            var city = this._AdjacencyList[cityIndex]._City;
            if (city.Index != cityIndex)
            {
                return this._AdjacencyList.Find(tuple => tuple._City.Index == cityIndex)._City;
            }
            else
            {
                return city;
            }
        }

        //Dodanie nowego wierzcholka
        public void AddCity(double latitude, double longitude, double demand)
        {
            this._AdjacencyList.Add(
                (
                    new City(this.GetNumberOfCities(), latitude, longitude, demand),
                    new List<EdgeWithDestinationCity>()
                )
            );
        }

        //Zwraca ilosc wierzcholkow - uzyte w property NumberOfVertexes
        private int GetNumberOfCities()
        {
            return this._AdjacencyList.Count;
        }

        //Zwraca liste sasiedztwa jako string
        public override string ToString()
        {
            string str = string.Empty;
            this._AdjacencyList.ForEach(tuple =>
            {
                str += tuple._City.Index + ":";
                tuple._Edges.ForEach(edgeWithDestVertex =>
                {
                    str += " " + edgeWithDestVertex.DestinationCity;
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
