namespace AntColonyNamespace
{
    //Klasa krawędź pomiędzy miastami/klientami
    internal class Edge
    {
        //Odległość drogi pomiędzy klientami - niezmienne w trakcie działania programu
        public double Distance { get; private set; }

        //Ilość feromonów na odcinku - zmieniane w trakcie działania programu
        public double PheromoneLevel { get; set; }

        /*Konstruktor - musimy podać również wstępną wartość feromonów na odcinku
        Powinno się ją uzyskiwać za pomocą strategii Nearest Neighbour*/
        public Edge(double distance, double initialPheromoneLevel)
        {
            this.Distance = distance;
            this.PheromoneLevel = initialPheromoneLevel;
        }

        /*Obydwie nadpisane metody służą do sprawdzenia, czy 2 krawędzie są sobie równe. Jest to potrzebne
        przy dodawaniu nowych elementów do Dictionary, ponieważ edge są kluczami i muszą być unikalne.
        GetHashCode daje duże prawdopodobieństwo, że zwróci różne hashe co jest warunkiem koniecznym
        żeby stwierdzić, że 2 edge są różne. Jeśli zwróci takie same hashe to to jeszcze nie oznacza, że
        edge są identyczne - metoda Equals ostatecznie sprawdzi, czy są to takie same obiekty*/
        public override int GetHashCode()
        {
            return (int)(this.Distance * this.PheromoneLevel);
        }

        public override bool Equals(object? obj)
        {
            if (obj != null && this.GetType() == obj.GetType() && Object.ReferenceEquals(this, obj))
            {
                Edge checkedEdge = (Edge)obj;
                return checkedEdge.PheromoneLevel == this.PheromoneLevel
                    && checkedEdge.Distance == this.Distance;
            }
            else
            {
                return false;
            }
        }
    }
}
