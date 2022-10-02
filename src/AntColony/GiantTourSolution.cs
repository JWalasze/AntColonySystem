namespace AntColonyNamespace
{
    //Klasa przechowujaca rozwiazanie, czyli stworzone marszruty
    internal class GiantTourSolution : ICloneable
    {
        //Lista przechowujaca krotki zawierajace sciezke i dwa miasta pomiedzy nia
        public List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)> GiantItinerary;

        //Lista przechowujaca wypelnienia kolejnych ciezarowek na trasie
        public List<double> UsedCapacitiesOfTrucks;

        //Konstruktor
        public GiantTourSolution()
        {
            this.GiantItinerary = new List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)>();
            this.UsedCapacitiesOfTrucks = new List<double>();
        }

        //Dodanie kolejnego przejscia pomiedzy miastami
        public void AddPathToGiantSolution(int startCityIndex, Edge edgeBetween, int endCityIndex)
        {
            this.GiantItinerary.Add((startCityIndex, edgeBetween, endCityIndex));
        }

        //Dodanie przejscia gdzie ciezarowka nie wyjezdza z depotu
        public void AddNoDepotLivingPathToSolution()
        {
            this.GiantItinerary.Add((0, new Edge(0, 0), 0));
        }

        //Dodanie wypelnienia ciezarowki w skonczonej marszrucie
        public void AddUsedCapacityOfTruck(double usedCapacity)
        {
            this.UsedCapacitiesOfTrucks.Add(usedCapacity);
        }

        //Resetujemy wartosci w obydwu listach - nie tracimy obiektow na stercie, jedynie zmienia sie referencja
        public void ResetSolution()
        {
            this.GiantItinerary = new List<(int _StartCityIndex, Edge _Edge, int _EndCityIndex)>();
            this.UsedCapacitiesOfTrucks = new List<double>();
        }

        //Zwraca dlugosc calej trasy
        public double GetGiantTourDistance()
        {
            var giantDistance = 0.0;
            this.GiantItinerary.ForEach(partOfPath =>
            {
                giantDistance += partOfPath._Edge.Distance;
            });

            return giantDistance;
        }

        //Wykonuje plytka kopie calego rozwiazania
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        //Sprawdzamy czy miasto zostalo odwiedzone - depot zawsze jest jako nieodwiedzony
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

        //Sprawdzamy czy konkretna krawedz jest w rozwiazaniu
        public bool IsEdgeInSolution(Edge checkingEdge)
        {
            return this.GiantItinerary.Any(partOfPath => partOfPath._Edge == checkingEdge);
        }

        //Zwraca rozwiazanie wielkiej trasy z rozlamem na marszruty jako string
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

        //Zwraca rozwiazanie wielkiej trasy jako string
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
