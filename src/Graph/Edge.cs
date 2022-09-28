using System;

namespace AntColonyNamespace
{
    internal class Edge
    {
        public double Distance { get; private set; }

        public double PheromoneLevel { get; set; }

        public Edge(double distance, double initialPheromoneLevel)
        {
            this.Distance = distance;
            this.PheromoneLevel = initialPheromoneLevel;
        }

        //Po to zeby dalo szanse ze sa rozne, po to jak jest do dziennika dodawane w possibilities
        //To juz daje ze moze beda rozne hashe a jak beda takie same to i tak program nie uzna ze to te same
        //edge i sprawdzi equals nizej
        public override int GetHashCode()
        {
            return (int)(this.Distance * this.PheromoneLevel);
        }

        public override bool Equals(object? obj)
        {
            if (obj != null && this.GetType() == obj.GetType() && Object.ReferenceEquals(this, obj))
            {
                Edge checkingEdge = (Edge)obj;
                return checkingEdge.PheromoneLevel == this.PheromoneLevel
                    && checkingEdge.Distance == this.Distance;
            }
            else
            {
                return false;
            }
        }
    }
}
