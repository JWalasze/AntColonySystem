namespace AntColonyNamespace
{
    /*Klasa dziedziczaca po CityGraph nastawiona na liczenie
    odleglosci pomiedzy dodanymi wierzcholkami*/
    internal class BenchmarkGraph : CityGraph
    {
        //Konstruktor
        public BenchmarkGraph() : base() { }

        //Tworzymy sciezki w grafie na podstawie dodanych wspolrzednych miast
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

        public void AddInitialPheromoneValues(double InitialPheromoneLevel)
        {
            foreach (var tuple in this)
            {
                foreach (var path in tuple._Edges)
                {
                    path.EdgeToDestCity.PheromoneLevel = InitialPheromoneLevel;
                }
            }
        }
    }
}
