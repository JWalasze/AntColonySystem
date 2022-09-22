namespace AntColonyNamespace
{
    internal class GiantTourSolution
    {
        private List<(int _CityIndex, Edge _Edge)> _GiantItinerary;

        public GiantTourSolution()
        {
            this._GiantItinerary = new List<(int _CityIndex, Edge _Edge)>();
        }

        public void AddPathToNextCity(int cityIndex, Edge edge)
        {
            this._GiantItinerary.Add((cityIndex, edge));
        }

        public void AddPathToDepot() { }
    }
}
