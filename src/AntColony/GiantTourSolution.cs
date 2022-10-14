namespace AntColonyNamespace
{
    internal class GiantTourSolution : ICloneable, IComparable
    {
        public List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)> GiantItinerary;

        public List<double> UsedCapacitiesOfTrucks;

        private double GiantTourDistance;

        public GiantTourSolution()
        {
            this.GiantItinerary = new List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)>();
            this.UsedCapacitiesOfTrucks = new List<double>();

            this.GiantTourDistance = 0;
        }

        public void AddPathToGiantSolution(int startCityIndex, Edge edgeBetween, int endCityIndex)
        {
            this.GiantItinerary.Add((startCityIndex, edgeBetween, endCityIndex));
            this.GiantTourDistance += edgeBetween.Distance;
        }

        public void AddNoDepotLivingPathToSolution()
        {
            this.GiantItinerary.Add((0, new Edge(0, 0), 0));
        }

        public void AddUsedCapacityOfTruck(double usedCapacity)
        {
            this.UsedCapacitiesOfTrucks.Add(usedCapacity);
        }

        public void ResetSolution()
        {
            this.GiantItinerary = new List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)>();
            this.UsedCapacitiesOfTrucks = new List<double>();

            this.GiantTourDistance = 0;
        }

        public double GetGiantTourDistance()
        {
            return this.GiantTourDistance;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }

            var otherSolution = obj as GiantTourSolution;
            if (otherSolution != null)
            {
                return otherSolution.GetGiantTourDistance().CompareTo(this.GetGiantTourDistance());
            }

            throw new Exception("Porównywany obiekt jest niepoprawny!");
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

        public string GetItineraryAllApart()
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
                // str += "(" + partOfPath._Edge.PheromoneLevel + ")";

                if (partOfPath._EndCityIndex == 0)
                {
                    str += 0;
                    str +=
                        " | zapelnienie tira: " + this.UsedCapacitiesOfTrucks[itineraryNumber - 1];
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
