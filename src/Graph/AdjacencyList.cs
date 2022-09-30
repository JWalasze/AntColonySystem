using System.Collections;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class AdjacencyList : IEnumerable
    {
        /*Lista krotek składająca się z miasta i listy wychodzących z niego dróg -
        wspólnie tworzą listę sąsiedztwa*/
        private List<(City _City, List<EdgeWithDestinationCity> _Edges)> _AdjacencyList;

        //Konstruktor listy sąsiedztwa
        public AdjacencyList()
        {
            this._AdjacencyList = new List<(City _City, List<EdgeWithDestinationCity> _Edges)>();
        }

        //Zwraca ilosc wierzchołków w grafie, jako pole property
        public int NumberOfCities
        {
            get { return this.GetNumberOfCities(); }
        }

        //Pozwala nam na dostep do krawedzi danego miasta za pomoca []
        public ReadOnlyCollection<EdgeWithDestinationCity> this[int cityIndex]
        {
            get
            {
                if (cityIndex < 0 || cityIndex >= this.NumberOfCities)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(cityIndex),
                        "City index is out of range!!!"
                    );
                }
                else
                {
                    return new ReadOnlyCollection<EdgeWithDestinationCity>(
                        this._AdjacencyList[cityIndex]._Edges
                    );
                }
            }
        }

        //Zwraca ilość wierzchołków użyte w polu NumberOfCIties
        private int GetNumberOfCities()
        {
            return this._AdjacencyList.Count;
        }

        //Dodanie nowego miasta do listy sąsiedztwa
        public void AddCity(int index, double latitude, double longitude, double demand)
        {
            this._AdjacencyList.Add(
                (new City(index, latitude, longitude, demand), new List<EdgeWithDestinationCity>())
            );
        }

        //Zwraca nam miasto na podstawie podanego indeksu
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

        //Dodanie skierowanej krawędzi do listy sąsiedztwa
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

        //Dodanie nieskierowanej krawędzi do listy sąsiedztwa
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

        //Zwraca nam odcinek pomiędzy 2 konkretnymi miastami
        public Edge GetEdgeBetweenTwoCities(int firstCityIndex, int secondCityIndex)
        {
            foreach (var pathToCity in this[firstCityIndex])
            {
                if (pathToCity.DestinationCity == secondCityIndex)
                {
                    return pathToCity.EdgeToDestCity;
                }
            }
            throw new Exception("Brak sciezki pomiedzy miastami");
        }

        //Zwraca nam odcinek z konkretnego miasta do depotu (depot = city z indeksem 0)
        public Edge GetEdgeToDepot(int cityIndex)
        {
            var pathToDepot = this[cityIndex][0];
            if (pathToDepot.DestinationCity != 0)
            {
                throw new Exception("Zle zwrocona krawedz do depotu!!!");
            }
            else if (cityIndex == 0)
            {
                throw new Exception("Juz jestesmy w depocie!!!");
            }
            else
            {
                return pathToDepot.EdgeToDestCity;
            }
        }

        public (City _City, List<EdgeWithDestinationCity> _Edges) GetTuple(int tupleIndex)
        {
            return this._AdjacencyList[tupleIndex];
        }

        //Przedstawia listę sąsiedztwa w postaci stringa
        public override string ToString()
        {
            string str = string.Empty;
            this._AdjacencyList.ForEach(tuple =>
            {
                str += tuple._City.Index + " (" + tuple._City.Demand + ")" + ":";
                tuple._Edges.ForEach(edgeWithDestCity =>
                {
                    str +=
                        " "
                        + edgeWithDestCity.DestinationCity
                        + "("
                        + Math.Round(edgeWithDestCity.EdgeToDestCity.Distance, 2)
                        + ")"
                        + ", "
                        + "("
                        + Math.Round(edgeWithDestCity.EdgeToDestCity.PheromoneLevel, 5)
                        + ")";
                    str += Environment.NewLine;
                });
                str += Environment.NewLine;
            });

            return str;
        }

        /*Zaimplementowana metoda interfejsu IEnumerable zwracająca instancję innego interfejsu
        Klasa AdjacencyListEnum implementuje interfejs iEnumerator*/
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public AdjacencyListEnum GetEnumerator()
        {
            return new AdjacencyListEnum(this._AdjacencyList);
        }
    }

    //Klasa potrzebna do iteracji po obiekcie AdjacencyList
    internal class AdjacencyListEnum : IEnumerator
    {
        private List<(City _City, List<EdgeWithDestinationCity> _Edges)> _AdjacencyList;

        private int Position = -1;

        public AdjacencyListEnum(
            List<(City _City, List<EdgeWithDestinationCity> _Edges)> AdjacencyList
        )
        {
            this._AdjacencyList = AdjacencyList;
        }

        public bool MoveNext()
        {
            ++this.Position;
            return this.Position < this._AdjacencyList.Count;
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
                    return this._AdjacencyList[Position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
