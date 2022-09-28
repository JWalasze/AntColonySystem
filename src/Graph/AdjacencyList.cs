using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class AdjacencyList : IEnumerable
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

        //Zwraca ilosc wierzcholkow - uzyte w property NumberOfVertexes
        private int GetNumberOfCities()
        {
            return this._AdjacencyList.Count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public AdjacencyListEnum GetEnumerator()
        {
            return new AdjacencyListEnum(this._AdjacencyList);
        }

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

        public Edge GetEdgeBetweenTwoCities(int firstCityIndex, int secondCityIndex)
        {
            foreach (var edgeWithDestCity in this._AdjacencyList[firstCityIndex]._Edges)
            {
                if (edgeWithDestCity.DestinationCity == secondCityIndex)
                {
                    return edgeWithDestCity.EdgeToDestCity;
                }
            }
            throw new Exception("Brak sciezki pomiedzy miastami");
        }

        public EdgeWithDestinationCity GetEdgeToDepot(int cityIndex)
        {
            var edgeToDepot = this._AdjacencyList[cityIndex]._Edges[0];
            if (edgeToDepot.DestinationCity != 0)
            {
                throw new Exception("Zle zwrocona krawedz do depotu!!!");
            }
            else if (cityIndex == 0)
            {
                throw new Exception("Juz jestesmy w depocie!!!");
            }
            else
            {
                return edgeToDepot;
            }
        }

        //Dodanie nowego wierzcholka
        public void AddCity(int index, double latitude, double longitude, double demand)
        {
            this._AdjacencyList.Add(
                (new City(index, latitude, longitude, demand), new List<EdgeWithDestinationCity>())
            );
        }

        public override string ToString()
        {
            string str = string.Empty;
            this._AdjacencyList.ForEach(tuple =>
            {
                str += tuple._City.Index + " (" + tuple._City.Demand + ")" + ":";
                tuple._Edges.ForEach(edgeWithDestVertex =>
                {
                    str +=
                        " "
                        + edgeWithDestVertex.DestinationCity
                        + "("
                        + Math.Round(edgeWithDestVertex.EdgeToDestCity.Distance, 2)
                        + ")"
                        + "("
                        + Math.Round(edgeWithDestVertex.EdgeToDestCity.PheromoneLevel, 2)
                        + ")";
                    str += Environment.NewLine;
                });
                str += Environment.NewLine;
            });
            return str;
        }
    }

    internal class AdjacencyListEnum : IEnumerator
    {
        private List<(City _City, List<EdgeWithDestinationCity> _Edges)> _AdjacencyList;

        private int position = -1;

        public AdjacencyListEnum(
            List<(City _City, List<EdgeWithDestinationCity> _Edges)> AdjacencyList
        )
        {
            this._AdjacencyList = AdjacencyList;
        }

        public bool MoveNext()
        {
            ++this.position;
            return this.position < this._AdjacencyList.Count;
        }

        public void Reset()
        {
            this.position = -1;
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public (City _City, List<EdgeWithDestinationCity> _Edges) Current
        {
            get
            {
                try
                {
                    return this._AdjacencyList[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
