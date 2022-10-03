using System.Collections;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class CityGraph : IEnumerable
    {
        private AdjacencyList _AdjacencyList;

        public CityGraph()
        {
            this._AdjacencyList = new AdjacencyList();
        }

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

        public void AddCity(int index, double latitude, double longitude, int demand)
        {
            this._AdjacencyList.AddCity(index, latitude, longitude, demand);
        }

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

        public double GetTotalDemandOfCities()
        {
            var demands = 0.0;
            foreach (var tuple in this)
            {
                demands += tuple._City.Demand;
            }

            return demands;
        }

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

        public void AddDirectedEdge(int startCityIndex, int endCityINdex, Edge newDirectedEdge)
        {
            this._AdjacencyList.AddDirectedEdge(startCityIndex, endCityINdex, newDirectedEdge);
        }

        public ReadOnlyCollection<EdgeWithDestinationCity> GetEdgesFromCity(int cityIndex)
        {
            return this[cityIndex];
        }

        public Edge GetEdgeBetweenTwoCities(int firstCityIndex, int secondCityIndex)
        {
            return this._AdjacencyList.GetEdgeBetweenTwoCities(firstCityIndex, secondCityIndex);
        }

        public Edge GetEdgeToDepot(int cityIndex)
        {
            return this._AdjacencyList.GetEdgeToDepot(cityIndex);
        }

        public EdgeWithDestinationCity GetEdgeWithDestinationCityToDepot(int cityIndex)
        {
            return this._AdjacencyList.GetEdgeWithDestinationCityToDepot(cityIndex);
        }

        public override string ToString()
        {
            return this._AdjacencyList.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public CityGraphEnum GetEnumerator()
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
