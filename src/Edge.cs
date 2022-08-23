using System;

namespace AntColonyNamespace
{
    internal class Edge
    {
        //Po tym moze dziedziczyć zeby bylo wyrazniej
        public double Distance { get; set; }

        public double PheromoneLevel { get; set; }

        // public int StartVertex { get; }

        // public int EndVertex { get; }

        // public Edge(int startVertex, int endVertex)
        // {
        //     this.StartVertex = startVertex;
        //     this.EndVertex = endVertex;
        // }

        public Edge(double distance, double pheromoneLevel)
        {
            this.Distance = distance;
            this.PheromoneLevel = pheromoneLevel;
        }

        public void SetDistanceAndPheromoneLevel(double newDistance, double newPheromoneLevel)
        {
            this.Distance = newDistance;
            this.PheromoneLevel = newPheromoneLevel;
        }
    }
}
