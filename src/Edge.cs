using System;

namespace AntColonyNamespace
{
    internal class Edge
    {
        public int StartVertex { get; }

        public int EndVertex { get; }

        public double Distance { get; set; }

        public double PheromoneLevel { get; set; }

        public Edge(int startVertex, int endVertex, double distance, double pheromoneLevel)
        {
            this.StartVertex = startVertex;
            this.EndVertex = endVertex;
            this.Distance = distance;
            this.PheromoneLevel = pheromoneLevel;
        }

        public Edge(int startVertex, int endVertex, double distance)
        {
            this.StartVertex = startVertex;
            this.EndVertex = endVertex;
            this.Distance = distance;
            this.PheromoneLevel = 0;
        }

        public void UpdatePheromoneLevel(double newPheromoneLevel)
        {
            this.PheromoneLevel = newPheromoneLevel;
        }
    }
}
