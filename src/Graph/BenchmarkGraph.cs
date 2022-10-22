namespace AntColonyNamespace
{
    internal class BenchmarkGraph : CityGraph
    {
        public BenchmarkGraph() : base() { }

        public void CreateCompletedGraphBasedOnCityCoord()
        {
            for (
                int currentCityStart = 0, numberEdgesToCreate = this.GetNumberOfCities();
                currentCityStart < this.GetNumberOfCities();
                ++currentCityStart, --numberEdgesToCreate
            )
            {
                for (
                    int currentCityEnd = currentCityStart + 1;
                    currentCityEnd < this.GetNumberOfCities();
                    ++currentCityEnd
                )
                {
                    this.AddUndirectedEdge(
                        currentCityStart,
                        currentCityEnd,
                        new Edge(
                            Math.Sqrt(
                                Math.Pow(
                                    this.GetCity(currentCityEnd).Latitude
                                        - this.GetCity(currentCityStart).Latitude,
                                    2
                                )
                                    + Math.Pow(
                                        this.GetCity(currentCityEnd).Longitude
                                            - this.GetCity(currentCityStart).Longitude,
                                        2
                                    )
                            ),
                            0
                        )
                    );
                }
            }
        }

        public void SetInitialPheromoneValues(double initialPheromoneLevel)
        {
            foreach (var tuple in this)
            {
                foreach (var path in tuple._Edges)
                {
                    if (tuple._City.Index < path.DestinationCity)
                    {
                        path.EdgeToDestCity.PheromoneLevel = initialPheromoneLevel;
                    }
                }
            }
        }
    }
}
