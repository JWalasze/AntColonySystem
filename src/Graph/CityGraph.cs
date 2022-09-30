using System.Collections;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    /*Klasa reprezentująca graf z wierzchołkami, jako miasta i
    krawędziami, jako odległościami między nimi*/
    internal class CityGraph : IEnumerable
    {
        //Lista sasiedztwa grafu
        private AdjacencyList _AdjacencyList;

        //Konstruktor grafu
        public CityGraph()
        {
            this._AdjacencyList = new AdjacencyList();
        }

        //Dodanie miasta do grafu
        public void AddCity(int index, double latitude, double longitude, int demand)
        {
            this._AdjacencyList.AddCity(index, latitude, longitude, demand);
        }

        //Zwraca liczbę krawędzi w grafie
        public int GetNumberOfEdges()
        {
            var counter = 0;
            foreach (var tuple in this)
            {
                foreach (var edge in tuple)
                {
                    if (edge.DestinationCity > tuple._City.Index)
                    {
                        ++counter;
                    }
                }
            }
            return counter;
        }

        public double GetTotalDemandOfCities()
        {
            var demands = 0.0;
            foreach (var tuple in this._AdjacencyList)
            {
                demands += tuple._City.Demand;
            }
            return demands;
        }

        public ReadOnlyCollection<EdgeWithDestinationCity> this[int cityNumber]
        {
            get
            {
                if (cityNumber < 0 || cityNumber >= this._AdjacencyList.NumberOfCities)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(cityNumber),
                        "City index is out of range!!!"
                    );
                }
                else
                {
                    return new ReadOnlyCollection<EdgeWithDestinationCity>(
                        this._AdjacencyList[cityNumber]
                    );
                }
            }
        }

        //Zwraca ilosc wierzchokow w grafie
        public int GetNumberOfCities()
        {
            return this._AdjacencyList.NumberOfCities;
        }

        public City GetCity(int cityIndex)
        {
            return this._AdjacencyList.GetCity(cityIndex);
        }

        public double GetDemandOfCity(int cityIndex)
        {
            return this._AdjacencyList.GetCity(cityIndex).Demand;
        }

        //Dodaje nieskierowana krawedz do grafu - info o wierzcholkach w newEdge
        public void AddUndirectedEdge(
            int firstCityIndex,
            int secondCityIndex,
            Edge newUndirectedEdge
        )
        {
            this._AdjacencyList.AddUndirectedEdge(
                firstCityIndex,
                secondCityIndex,
                newUndirectedEdge
            );
        }

        //Dodanie skierowana krawedz - info o wierzcholkach w newEdge
        public void AddDirectedEdge(int startCityIndex, int endCityINdex, Edge newDirectedEdge)
        {
            this._AdjacencyList.AddDirectedEdge(startCityIndex, endCityINdex, newDirectedEdge);
        }

        //Metoda zeby zwrocic krawedzie z podanego wierzcholka
        public ReadOnlyCollection<EdgeWithDestinationCity> GetEdgesFromCity(int cityIndex)
        {
            return this._AdjacencyList[cityIndex];
        }

        public Edge GetEdgeBetweenTwoCities(int firstCityIndex, int secondCityIndex)
        {
            return this._AdjacencyList.GetEdgeBetweenTwoCities(firstCityIndex, secondCityIndex);
        }

        public Edge GetEdgeToDepot(int cityIndex)
        {
            return this._AdjacencyList.GetEdgeToDepot(cityIndex);
        }

        //Zwraca graf w reprezentacji listy jako string
        public override string ToString()
        {
            return this._AdjacencyList.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return new CityGraphEnum(this._AdjacencyList);
        }
    }

    internal class CityGraphEnum : IEnumerator
    {
        private AdjacencyList _AdjacencyList;

        private int Position = -1;

        public CityGraphEnum(AdjacencyList AdjacencyList)
        {
            this._AdjacencyList = AdjacencyList;
        }

        public bool MoveNext()
        {
            ++this.Position;
            return this.Position < this._AdjacencyList.NumberOfCities;
        }

        public void Reset()
        {
            this.Position = -1;
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
                    return this._AdjacencyList.GetTuple(this.Position);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
