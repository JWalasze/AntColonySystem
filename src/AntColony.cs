using System;

namespace AntColonyNamespace
{
    internal class AntColony
    {
        //Coefficients

        //Q is unknow???
        public readonly double Q;

        //ALFA controls influence of TAU
        public readonly double ALFA;

        //BETA controls influence of ETA
        public readonly double BETA;

        //P is the pheromone evaporation coefficient
        public readonly double P;

        //List of every ant in a colony
        public List<Ant> Ants;

        public AntColony(double _Q, double _ALFA, double _BETA, double _P)
        {
            this.Q = _Q;
            this.ALFA = _ALFA;
            this.BETA = _BETA;
            this.P = _P;

            this.Ants = new List<Ant>();
            this.AddAntToTheColony();
        }

        private void AddAntToTheColony()
        {
            this.Ants.Add(new Ant(this));
        }

        public class Ant
        {
            private int AntIndex;
            private List<int> CurrentPheromonePath;

            private AntColony AntColonyReference;

            public Ant(AntColony _antColonyReference)
            {
                //Reference to the outer class
                this.AntColonyReference = _antColonyReference;

                this.CurrentPheromonePath = new List<int>();
            }

            public int GetPheromonePathLength()
            {
                return this.CurrentPheromonePath.Count;
            }

            public void AddVertexToPheromonePath(int vertex)
            {
                this.CurrentPheromonePath.Add(vertex);
            }

            public void CountPossibility() { }

            public void MoveToTheNextEdge() { }

            public double GetRandomNumberFrom0To1()
            {
                Random random = new Random();
                return random.NextDouble();
            }
        }
    }
}
