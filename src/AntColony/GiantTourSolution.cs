namespace AntColonyNamespace
{
    internal class GiantTourSolution
    {
        private List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)> _GiantItinerary;

        public GiantTourSolution()
        {
            this._GiantItinerary = new List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)>();
        }

        public void AddPathOfSolution(int startCityIndex, Edge edge, int endCityIndex)
        {
            this._GiantItinerary.Add((startCityIndex, edge, endCityIndex));
        }

        public void ForEach(Action<(int, Edge, int)> action)
        {
            this._GiantItinerary.ForEach(path =>
            {
                action(path);
            });
        }

        public double GetGiantIteneraryDistance()
        {
            var giantDistance = 0.0;
            this._GiantItinerary.ForEach(path =>
            {
                giantDistance += path._Edge.Distance;
            });

            return giantDistance;
        }

        public bool IsCityVisited(int cityIndex)
        {
            return this._GiantItinerary.Any(
                path => path._StartCityIndex == cityIndex || path._EndCityIndex == cityIndex
            );
        }

        public void PrintGiantTourSolution()
        {
            Console.WriteLine(this.ToString());
        }

        public override string ToString()
        {
            string str = string.Empty;
            var lastPartOfPath = this._GiantItinerary.Last();
            this._GiantItinerary.ForEach(partOfPath =>
            {
                str += partOfPath._StartCityIndex;
                str += " -> ";
                str += partOfPath._EndCityIndex;
                if (!partOfPath.Equals(lastPartOfPath))
                {
                    str += " | ";
                }
            });
            return str;
        }
    }
}
