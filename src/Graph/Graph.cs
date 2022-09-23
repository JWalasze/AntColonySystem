using System;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class CityGraph
    {
        //Lista sasiedztwa grafu
        private AdjacencyList _AdjacencyList;

        //Zwraca liste sasiedztwa
        public AdjacencyList AdjacencyList
        {
            get { return this._AdjacencyList; }
        }

        public CityGraph() //DAC to do ANtColony, nie tu
        {
            this._AdjacencyList = new AdjacencyList();
        }

        //Dodanie wierzcholka do grafu - nowy vertex ma zwiekszony o 1 index
        public void AddCity(int index, double latitude, double longitude, int demand)
        {
            this._AdjacencyList.AddCity(index, latitude, longitude, demand);
        }

        public int GetNumberOfEdges()
        {
            var counter = 0;
            for (int i = 0; i < this._AdjacencyList.NumberOfCities; ++i)
            {
                foreach (var edge in this._AdjacencyList[i])
                {
                    if (edge.DestinationCity > i)
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

        public EdgeWithDestinationCity GetEdgeBetweenTwoCities(
            int firstCityIndex,
            int secondCityIndex
        )
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
