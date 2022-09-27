using System;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class CityGraph
    {
        //Lista sasiedztwa grafu
        private AdjacencyList _AdjacencyList;

        //SPRAWDZIC CZY MOZNA FOREACHOWAC ADJACENCY LIST I CZY MOZNA COS UPROSCIC !!!!!!!!
        public CityGraph()
        {
            this._AdjacencyList = new AdjacencyList();
        }

        public void ForEach(Action<(City _City, List<EdgeWithDestinationCity> _Edges)> action)
        {
            foreach (var tuple in this._AdjacencyList)
            {
                action(tuple);
            }
        }

        //Dodanie wierzcholka do grafu - nowy vertex ma zwiekszony o 1 index
        public void AddCity(int index, double latitude, double longitude, int demand)
        {
            this._AdjacencyList.AddCity(index, latitude, longitude, demand);
        }

        public int GetNumberOfEdges()
        {
            var counter = 0;
            foreach (var tuple in this._AdjacencyList)
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

        public EdgeWithDestinationCity GetEdgeToDepot(int cityIndex)
        {
            return this._AdjacencyList.GetEdgeToDepot(cityIndex);
        }

        //Zwraca graf w reprezentacji listy jako string
        public override string ToString()
        {
            return this._AdjacencyList.ToString();
        }
    }
}
