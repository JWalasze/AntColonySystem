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

        //ro defines evaporation
        public readonly double ro;

        //List of every ant in a colony
        public List<Ant> Ants;

        private Graph Graph;

        public AntColony(
            Graph _Graph,
            double _Q,
            double _ALFA,
            double _BETA,
            double _P,
            double _q0,
            double _ro
        )
        {
            this.Graph = _Graph;
            this.Q = _Q;
            this.ALFA = _ALFA;
            this.BETA = _BETA;
            this.P = _P;
            this.q0 = _q0;
            this.ro = _ro;

            this.Ants = new List<Ant>();
        }

        private void AddAntToTheColony()
        {
            this.Ants.Add(new Ant(this, 0));
        }

        public void StartLookingForPath() { }

        public class Ant
        {
            //Index mrowki
            private int AntIndex;

            //Obiekt do liczenia prawdopodobienstw wyboru krawedzi
            private Possibilities _Possibilities;

            //Wierzcholek z ktorego mrowka zaczyna podroz
            private int InitialVertex;

            //Zapisany index obecnego wierzcholka
            private int CurrentVertexIndex;

            private List<(Edge _Edge, int _VertexIndex)> CurrentPheromonePath;

            private AntColony _AntColony;

            public Ant(AntColony _antColony, int initialVertexIndex)
            {
                this._AntColony = _antColony;
                this._Possibilities = new Possibilities(this._AntColony.ALFA, this._AntColony.BETA);
                this.InitialVertex = initialVertexIndex;
                this.CurrentVertexIndex = this.InitialVertex;

                this.CurrentPheromonePath = new List<(Edge _Edge, int _VertexIndex)>();
            }

            public Edge ChoosePath()
            {
                foreach (
                    var edge in this._AntColony.Graph.GetEdgesFromVertex(this.CurrentVertexIndex)
                )
                {
                    if (!this.DidAntVisitVertex(edge.EndVertex))
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
                double randomNumberFrom0To1 = new Random().NextDouble();
                double lowerLimit = 0;
                double upperLimit = 0;
                foreach (var edge in this._Possibilities.GetProbabilities())
                {
                    upperLimit += edge.Value;
                    if (randomNumberFrom0To1 < upperLimit && randomNumberFrom0To1 > lowerLimit)
                    {
                        return edge.Key;
                    }
                    lowerLimit += edge.Value;
                }

                return this._Possibilities.GetProbabilities().First().Key;
            }

            private bool DidAntVisitVertex(int vertex)
            {
                return this.CurrentPheromonePath.Any(tuple => tuple._VertexIndex == vertex);
            }

            public void MoveToTheNextEdge()
            {
                var pickedEdge = this.ChoosePath();
                this.CurrentPheromonePath.Add((pickedEdge, pickedEdge.EndVertex));
                this.CurrentVertexIndex = pickedEdge.EndVertex;
                this._Possibilities.RestartAllValues();

                //Update feromony
                double updatedPheromoneLevel = (1 - this._AntColony.ro) * pickedEdge.PheromoneLevel;
                pickedEdge.UpdatePheromoneLevel(updatedPheromoneLevel);
            }
        }
    }
}
