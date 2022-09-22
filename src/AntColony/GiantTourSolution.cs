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

        public void ForEach(Action<(int _CityIndex, Edge _Edge)> action)
        {
            this._GiantItinerary.ForEach(tuple =>
            {
                action(tuple);
            });
        }

        public double GetGiantIteneraryDistance()
        {
            var giantDistance = 0.0;
            this._GiantItinerary.ForEach(solutionPart =>
            {
                giantDistance += solutionPart._Edge.Distance;
            });
            return giantDistance;
        }

        public bool IsCityVisited(int cityIndex)
        {
            return this._GiantItinerary.Any(partOfPath => partOfPath._CityIndex == cityIndex);
        }
    }
}
