namespace AntColonyNamespace
{
    internal class GiantTourSolution : ICloneable
    {
        public List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)> GiantItinerary;

        public List<double> UsedCapacitiesInItineraries;

        public GiantTourSolution()
        {
            this.GiantItinerary = new List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)>();
            this.UsedCapacitiesInItineraries = new List<double>();
        }

        public void AddPathOfSolution(int startCityIndex, Edge edge, int endCityIndex)
        {
            this.GiantItinerary.Add((startCityIndex, edge, endCityIndex));
        }

        public void AddUsedCapacityInItinerary(double usedCapacity)
        {
            this.UsedCapacitiesInItineraries.Add(usedCapacity);
        }

        public void AddNoDepotLeavingPathOfSolution()
        {
            this.GiantItinerary.Add((0, new Edge(0, 0), 0));
        }

        public void ResetSolution()
        {
            this.GiantItinerary = new List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)>();
            this.UsedCapacitiesInItineraries = new List<double>();
        }

        public void ForEach(Action<(int, Edge, int)> action)
        {
            this.GiantItinerary.ForEach(partOfPath =>
            {
                action(partOfPath);
            });
        }

        public double GetGiantIteneraryDistance()
        {
            var giantDistance = 0.0;
            this.GiantItinerary.ForEach(partOfPath =>
            {
                giantDistance += partOfPath._Edge.Distance;
            });

            return giantDistance;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool IsCityVisited(int cityIndex)
        {
            if (cityIndex == 0)
            {
                return false;
            }

            return this.GiantItinerary.Any(
                partOfPath =>
                    partOfPath._StartCityIndex == cityIndex || partOfPath._EndCityIndex == cityIndex
            );
        }

        public bool IsEdgeInSolution(Edge checkingEdge)
        {
            return this.GiantItinerary.Any(partOfPath => partOfPath._Edge == checkingEdge);
        }

        public void PrintGiantTourSolution()
        {
            Console.WriteLine(this.ToString());
        }

        public string PrintItineraryAllApart()
        {
            string str = string.Empty;
            var itineraryNumber = 1;
            var lastPartOfPath = this.GiantItinerary.Last();

            str += "Marszruta " + itineraryNumber;
            str += Environment.NewLine;

            this.GiantItinerary.ForEach(partOfPath =>
            {
                str += partOfPath._StartCityIndex;
                str += " -> ";
                str += "(" + partOfPath._Edge.PheromoneLevel + ")";

                if (partOfPath._EndCityIndex == 0)
                {
                    str += 0;
                    str +=
                        " | zapelnienie tira: "
                        + this.UsedCapacitiesInItineraries[itineraryNumber - 1];
                    str += Environment.NewLine;
                    if (!lastPartOfPath.Equals(partOfPath))
                    {
                        str += "Maszruta " + ++itineraryNumber;
                        str += Environment.NewLine;
                    }
                }
            });

            return str;
        }

        public override string ToString()
        {
            string str = string.Empty;
            var lastPartOfPath = this.GiantItinerary.Last();

            this.GiantItinerary.ForEach(partOfPath =>
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
