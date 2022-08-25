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

        private Graph Graph;

        public AntColony(Graph _Graph, double _Q, double _ALFA, double _BETA, double _P)
        {
            this.Graph = _Graph;
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

            private Vertex InitialVertex = new Vertex();

            private List<Tuple<Vertex, Edge>> CurrentPheromonePath;

            private AntColony AntColonyReference;

            public Ant(AntColony _antColonyReference)
            {
                //Reference to the outer class
                this.AntColonyReference = _antColonyReference;

                this.CurrentPheromonePath = new List<Tuple<Vertex, Edge>>();
            }

            // public int GetPheromonePathLength()
            // {
            //     return this.CurrentPheromonePath.Count;
            // }

            // public void AddVertexToPheromonePath(int vertex)
            // {
            //     this.CurrentPheromonePath.Add(vertex);
            // }

            //Liczymy prawdopodobienstwo wyboru konkretnej krawedzi
            private double CountPossibility(Edge pickedEdge, double totalValueOfPossibleStates)
            {
                if (this.IsAntVisitedEdge(pickedEdge))
                {
                    return 0;
                }
                else
                {
                    double possibility =
                        Math.Pow(pickedEdge.PheromoneLevel, this.AntColonyReference.ALFA)
                        * Math.Pow(1 / pickedEdge.Distance, this.AntColonyReference.BETA)
                        / totalValueOfPossibleStates;

                    return possibility;
                }
            }

            //Liczymy mianownik wzoru
            private double CountValueOfPossibleStates()
            {
                double totalValueOfPossibleStates = 0;
                foreach (
                    Edge edge in this.AntColonyReference.Graph.GetPossibleEdgesFromVertex(
                        InitialVertex.VertexIndex
                    )
                )
                {
                    if (!this.IsAntVisitedEdge(edge))
                    {
                        totalValueOfPossibleStates +=
                            Math.Pow(edge.PheromoneLevel, this.AntColonyReference.ALFA)
                            * Math.Pow(1 / edge.Distance, this.AntColonyReference.BETA);
                    }
                }
                return totalValueOfPossibleStates;
            }

            //Wybieramy kolejn krawedz do przejscia dalej
            public Edge SelectPath()
            {
                List<double> countedPossiblities = new List<double>();
                double totalValueOfPossibleStates = this.CountValueOfPossibleStates();
                foreach (
                    Edge edge in this.AntColonyReference.Graph.GetPossibleEdgesFromVertex(
                        InitialVertex.VertexIndex
                    )
                )
                {
                    countedPossiblities.Add(
                        this.CountPossibility(edge, totalValueOfPossibleStates)
                    );
                }
                return this.ChoosePath(countedPossiblities);
            }

            //Na podstawie prawdopodobienstw wybieramy sciezke
            private Edge ChoosePath(List<double> countedPossibilities)
            {
                return null;
            }

            private bool IsAntVisitedEdge(Edge edge)
            {
                return true;
            }

            public void MoveToTheNextEdge() { }

            public double GetRandomNumberFrom0To1()
            {
                Random random = new Random();
                return random.NextDouble();
            }
        }
    }
}
