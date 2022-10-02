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

        //Mozemy zwrocic liste sciezek z grafu za pomoca operatora []
        public ReadOnlyCollection<EdgeWithDestinationCity> this[int cityIndex]
        {
            get
            {
                if (cityIndex < 0 || cityIndex >= this._AdjacencyList.NumberOfCities)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(cityIndex),
                        "City index is out of range!!!"
                    );
                }
                else
                {
                    return new ReadOnlyCollection<EdgeWithDestinationCity>(
                        this._AdjacencyList[cityIndex]
                    );
                }
            }
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
                foreach (var edge in tuple._Edges)
                {
                    if (edge.DestinationCity > tuple._City.Index)
                    {
                        ++counter;
                    }
                }
            }

            return counter;
        }

        //Zwraca ilosc miast w grafie
        public int GetNumberOfCities()
        {
            return this._AdjacencyList.NumberOfCities;
        }

        //Zwroc miasto na podstawie podanego indeksu
        public City GetCity(int cityIndex)
        {
            return this._AdjacencyList.GetCity(cityIndex);
        }

        //Zwroc konkretne rzadanie wybranego miasta
        public double GetDemandOfCity(int cityIndex)
        {
            return this._AdjacencyList.GetCity(cityIndex).Demand;
        }

        //Zwraca calkowita wartosc rzadan ze wszystkich miast
        public double GetTotalDemandOfCities()
        {
            var demands = 0.0;
            foreach (var tuple in this)
            {
                demands += tuple._City.Demand;
            }

            return demands;
        }

        //Dodaj nieskierowana krawedz do grafu
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

        //Dodaj skierowana krawedz do grafu
        public void AddDirectedEdge(int startCityIndex, int endCityINdex, Edge newDirectedEdge)
        {
            this._AdjacencyList.AddDirectedEdge(startCityIndex, endCityINdex, newDirectedEdge);
        }

        //Zwracamy sciezki wychodzace z podanego miasta
        public ReadOnlyCollection<EdgeWithDestinationCity> GetEdgesFromCity(int cityIndex)
        {
            return this[cityIndex];
        }

        //Zwracamy krawedz pomiedzy dwoma miastami
        public Edge GetEdgeBetweenTwoCities(int firstCityIndex, int secondCityIndex)
        {
            return this._AdjacencyList.GetEdgeBetweenTwoCities(firstCityIndex, secondCityIndex);
        }

        //Zwracamy krawedz powrotna do depotu
        public Edge GetEdgeToDepot(int cityIndex)
        {
            return this._AdjacencyList.GetEdgeToDepot(cityIndex);
        }

        public EdgeWithDestinationCity GetEdgeWithDestinationCityToDepot(int cityIndex)
        {
            return this._AdjacencyList.GetEdgeWithDestinationCityToDepot(cityIndex);
        }

        //Zwraca graf w reprezentacji listy sasiedztwa jako string
        public override string ToString()
        {
            return this._AdjacencyList.ToString();
        }

        //Metody do iterowania w petli foreach
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public CityGraphEnum GetEnumerator()
        {
            return new CityGraphEnum(this._AdjacencyList);
        }
    }

    //Klasa pomocna do iterowania po grafie
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
