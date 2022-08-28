using System;

namespace AntColonyNamespace
{
    internal class Edge
    {
        //Index startowego wierzcholka
        public int StartVertex { get; }

        //Index koncowego wierzcholka
        public int EndVertex { get; }

        //Dystans, czyli dlugosc krawedzi
        public double Distance { get; set; }

        //Poziom feromonow na krawedzi
        public double PheromoneLevel { get; set; }

        //Konstruktor z wyborem wstepnego poziomu feromonow
        public Edge(int startVertex, int endVertex, double distance, double pheromoneLevel)
        {
            this.StartVertex = startVertex;
            this.EndVertex = endVertex;
            this.Distance = distance;
            this.PheromoneLevel = pheromoneLevel;
        }

        //Konstruktor bez wstepnego wyboru feromonow
        public Edge(int startVertex, int endVertex, double distance)
        {
            this.StartVertex = startVertex;
            this.EndVertex = endVertex;
            this.Distance = distance;
            this.PheromoneLevel = 0;
        }

        //Aktualizowanie wartosci feromonow na krawedzi
        public void UpdatePheromoneLevel(double newPheromoneLevel)
        {
            this.PheromoneLevel = newPheromoneLevel;
        }
    }
}
