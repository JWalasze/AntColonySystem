namespace AntColonyNamespace
{
    internal class GiantTourSolution
    {
        private List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)> _GiantItinerary;

        private List<double> _UsedCapacitiesInItineraries;

        public GiantTourSolution()
        {
            this._GiantItinerary = new List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)>();
            this._UsedCapacitiesInItineraries = new List<double>();
        }

        public void AddPathOfSolution(int startCityIndex, Edge edge, int endCityIndex)
        {
            this._GiantItinerary.Add((startCityIndex, edge, endCityIndex));
        }

        public void AddUsedCapacityInItinerary(double usedCapacity)
        {
            this._UsedCapacitiesInItineraries.Add(usedCapacity);
        }

        public void AddNoDepotLeavingPathOfSolution()
        {
            this._GiantItinerary.Add((0, new Edge(0, 0), 0));
        }

        public void ForEach(Action<(int, Edge, int)> action)
        {
            this._GiantItinerary.ForEach(partOfPath =>
            {
                action(partOfPath);
            });
        }

        public double GetGiantIteneraryDistance()
        {
            var giantDistance = 0.0;
            this._GiantItinerary.ForEach(partOfPath =>
            {
                giantDistance += partOfPath._Edge.Distance;
            });

            return giantDistance;
        }

        public bool IsCityVisited(int cityIndex)
        {
            if (cityIndex == 0)
            {
                return false;
            }

            return this._GiantItinerary.Any(
                partOfPath =>
                    partOfPath._StartCityIndex == cityIndex || partOfPath._EndCityIndex == cityIndex
            );
        }

        public void PrintGiantTourSolution()
        {
            Console.WriteLine(this.ToString());
        }

        public void PrintItineraryAllApart()
        {
            string str = string.Empty;
            var itineraryNumber = 1;
            var lastPartOfPath = this._GiantItinerary.Last();

            str += "Marszruta " + itineraryNumber;
            str += Environment.NewLine;

            this._GiantItinerary.ForEach(partOfPath =>
            {
                str += partOfPath._StartCityIndex;
                str += " -> ";

                if (partOfPath._EndCityIndex == 0)
                {
                    str += 0;
                    str +=
                        " | zapelnienie tira: "
                        + this._UsedCapacitiesInItineraries[itineraryNumber - 1];
                    str += Environment.NewLine;
                    if (!lastPartOfPath.Equals(partOfPath))
                    {
                        str += "Maszruta " + ++itineraryNumber;
                        str += Environment.NewLine;
                    }
                }
            });

            Console.WriteLine(str);
        }

        public override string ToString()
        {
            string str = string.Empty;
            var lastPartOfPath = this._GiantItinerary.Last();

            this._GiantItinerary.ForEach(partOfPath =>
            {
                str += partOfPath._StartCityIndex;
                str += " -> ";

                if (lastPartOfPath.Equals(partOfPath))
                {
                    str += partOfPath._EndCityIndex;
                }
            });

            return str;
        }
    }
}
