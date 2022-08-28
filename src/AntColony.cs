using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            //Index mrowki
            private int AntIndex;

            private Possibilities _Possibilities;

            private ReadOnlyCollection<Edge> ListOfEdges;

            //DO zmiany
            private Vertex InitialVertex = new Vertex();

            private List<Tuple<Vertex, Edge>> CurrentPheromonePath;

            private AntColony _AntColony;

            public Ant(AntColony _antColony)
            {
                this._AntColony = _antColony;
                this._Possibilities = new Possibilities(this._AntColony.ALFA, this._AntColony.BETA);
                this.ListOfEdges = new ReadOnlyCollection<Edge>(
                    this._AntColony.Graph.GetEdgesFromVertex(this.InitialVertex.VertexIndex)
                );

                this.CurrentPheromonePath = new List<Tuple<Vertex, Edge>>();
            }

            //Chyba niepotrzebne bo w zwracanych edgach bedzie info skad dokoad jest krawedz...UZYC ANY PRZY PATRZENIU CZY VERTEX JEST ODWIEDZONY

            public Edge ChoosePath()
            {
                this.ListOfEdges = this._AntColony.Graph.GetEdgesFromVertex(
                    this.InitialVertex.VertexIndex
                );
                foreach (var edge in this.ListOfEdges)
                {
                    if (!this.IsAntVisitedVertex(edge.EndVertex))
                    {
                        this._Possibilities.CountNominatorAndUpdateDenominator(edge);
                    }
                }
                this._Possibilities.CountProbabilities();

                double randomNumberFrom0To1 = (new Random()).NextDouble();
                if (randomNumberFrom0To1 <= this._AntColony.q0)
                {
                    return this._Possibilities.GetMaxNominator().Key;
                }
                else
                {
                    return this.ChoosePathBasedOnProb();
                }
            }

            private Edge ChoosePathBasedOnProb()
            {
                //Tu trzeba dac losowanie na podstawie tych prawdopodobienstw...
                return new Edge(1, 1, 1, 1);
            }

            private bool IsAntVisitedEdge(int edge)
            {
                //Tez trzeba zrobic
                return true;
            }

            private bool IsAntVisitedVertex(int vertex)
            {
                //Trzeba zrobic
                return false;
            }

            public void MoveToTheNextEdge()
            {
                //Tu bedziemy wszystko aktualizowac ruszac i takie tam
            }
        }
    }
}
