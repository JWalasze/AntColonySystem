namespace AntColonyNamespace
{
    internal class CompletedGraph : CityGraph
    {
        public CompletedGraph() : base() { }

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
                    this.AdjacencyList.AddUndirectedEdge(
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
                            1
                        )
                    );
                }
            }
        }
    }
}
