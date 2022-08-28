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

        //q0 determins the importance of exploration versus exploitation
        public readonly double q0;

        //List of every ant in a colony
        public List<Ant> Ants;

        private Graph Graph;

        public AntColony(Graph _Graph, double _Q, double _ALFA, double _BETA, double _P, double _q0)
        {
            this.Graph = _Graph;
            this.Q = _Q;
            this.ALFA = _ALFA;
            this.BETA = _BETA;
            this.P = _P;
            this.q0 = _q0;

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

            private double Denominator = 0;

            private List<double> ListOfNominators = new List<double>();

            private List<double> ListOfPossibilities = new List<double>();

            private List<(Vertex vertex, Edge edge)> ListOfPossibileMoves = new List<(Vertex vertex, Edge edge)>();

            //DO zmiany
            private Vertex InitialVertex = new Vertex();

            private List<Tuple<Vertex, Edge>> CurrentPheromonePath;

            private AntColony AntColonyReference;

            public Ant(AntColony _antColonyReference)
            {
                //Reference to the outer class
                this.AntColonyReference = _antColonyReference;

                this.CurrentPheromonePath = new List<Tuple<Vertex, Edge>>();
            }

            
            //Liczymy mianownik wzoru i zliczamy liczniki
            private void CountDenominatorAndNominators()
            {
                this.AntColonyReference.Graph
                    .GetPossibleEdgesFromVertex(this.InitialVertex.VertexIndex)
                    .ForEach(edge =>
                    {
                        {
                            if (!this.IsAntVisitedVertex(new Vertex()))
                            {
                                double nominator = Math.Pow(edge.PheromoneLevel, this.AntColonyReference.ALFA)
                        * Math.Pow(1 / edge.Distance, this.AntColonyReference.BETA);
                            this.ListOfNominators.Add(nominator);
                            this.Denominator += nominator;
                            }
                            else{
                                this.ListOfNominators.Add(0);
                                this.Denominator += 0;
                            }
                        }
                    });
            }

            //Wybieramy kolejn krawedz do przejscia dalej
            public void CountPossibilities()
            {
                this.CountDenominatorAndNominators();

                this.AntColonyReference.Graph
                    .GetPossibleEdgesFromVertex(this.InitialVertex.VertexIndex)
                    .ForEach(edge => { 
                        double possibility = 
                    });
                {
                    countedPossiblities.Add(
                        this.CountPossibility(edge, totalValueOfPossibleStates)
                    );
                }
                return this.ChoosePath(countedPossiblities);
            }

            //Na podstawie prawdopodobienstw wybieramy sciezke
            private Edge ChoosePath(List<(double, double)> countedPossibilities)
            {
                double randomNumberFrom0To1 = (new Random()).NextDouble();
                if (randomNumberFrom0To1 <= this.AntColonyReference.q0) { }
                else
                {
                    return null;
                }
                return null;
            }

            private bool IsAntVisitedEdge(Edge edge)
            {
                return true;
            }

            private bool IsAntVisitedVertex(Vertex vertex)
            {
                return false;
            }

            public void MoveToTheNextEdge() { }
        }
    }
}
