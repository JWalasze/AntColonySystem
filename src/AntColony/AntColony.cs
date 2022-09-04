using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class AntColony
    {
        //Coefficients

        //Q is unknow???
        public readonly double _Q;

        //ALFA controls influence of TAU
        public readonly double _ALFA;

        //BETA controls influence of ETA
        public readonly double _BETA;

        //P is the pheromone evaporation coefficient
        public readonly double _P;

        //q0 determins the importance of exploration versus exploitation
        public readonly double _q0;

        //ro defines evaporation
        public readonly double ro;

        //List of every ant in a colony
        public List<Ant> Ants;

        private Graph Graph;

        public AntColony(
            Graph _Graph,
            double Q,
            double ALFA,
            double BETA,
            double P,
            double q0,
            double ro
        )
        {
            this.Graph = _Graph;
            this._Q = Q;
            this._ALFA = ALFA;
            this._BETA = BETA;
            this._P = P;
            this._q0 = q0;
            this.ro = ro;

            this.Ants = new List<Ant>();
        }

        public void AddAntToTheColony()
        {
            this.Ants.Add(new Ant(this, 0));
        }

        public void StartLookingForPath()
        {
            for (int i = 0; i < 2; ++i)
            {
                this.Ants.ForEach(ant =>
                {
                    ant.MoveToTheNextEdge();
                });
            }
            this.Ants.ForEach(ant =>
            {
                ant.PrintPath();
            });
        }

        public class Ant
        {
            private int AntIndex;

            private static int AntsGlobalIndex = 0;

            //Obiekt do liczenia prawdopodobienstw wyboru krawedzi
            private Possibilities _Possibilities;

            //Wierzcholek z ktorego mrowka zaczyna podroz
            private int _InitialVertexIndex;

            //Zapisany index obecnego wierzcholka
            private int _CurrentVertexIndex;

            private List<Edge> _CurrentPheromonePath;

            private AntColony _AntColony;

            public Ant(AntColony AntColony, int InitialVertexIndex)
            {
                this._AntColony = AntColony;
                this._Possibilities = new Possibilities(this._AntColony._BETA);
                this._InitialVertexIndex = InitialVertexIndex;
                this._CurrentVertexIndex = this._InitialVertexIndex;

                this._CurrentPheromonePath = new List<Edge>();
                this.AntIndex = AntsGlobalIndex++;
            }

            public Edge ChoosePath()
            {
                foreach (
                    var possibleEdge in this._AntColony.Graph.GetEdgesFromVertex(
                        this._CurrentVertexIndex
                    )
                )
                {
                    //Nie bierzemy pod uwagę odwiedzonych wierzchołków
                    if (!this.DidAntVisitVertex(possibleEdge.EndVertex))
                    {
                        this._Possibilities.CountNominatorAndUpdateDenominator(possibleEdge);
                    }
                }
                this._Possibilities.CountProbabilities();

                double randomNumberFrom0To1 = (new Random()).NextDouble();
                if (randomNumberFrom0To1 <= this._AntColony._q0)
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
                return this._CurrentPheromonePath.Any(
                    partOfPath => partOfPath.EndVertex == vertex || partOfPath.StartVertex == vertex
                );
            }

            public void MoveToTheNextEdge()
            {
                var choosenEdge = this.ChoosePath();
                this._CurrentPheromonePath.Add(choosenEdge);
                this._CurrentVertexIndex = choosenEdge.EndVertex;
                Console.WriteLine();
                foreach (var item in _Possibilities.GetProbabilities())
                {
                    Console.WriteLine(item.Value);
                }
                this._Possibilities.RestartAllValues();

                //Update feromony --- TO POWINNO BYC PO ZROBIENIU CALEJ SCIEZKI, TRZEBA DODAC POJEMNOSC MROWKI
                double updatedPheromoneLevel =
                    (1 - this._AntColony._ALFA) * choosenEdge.PheromoneLevel; //+ ALFA * najlepsza ostatnia sciezka
                choosenEdge.UpdatePheromoneLevel(updatedPheromoneLevel);
                //I jeszcze trzeba bedzie globalnie gdzie indziej
            }

            public void PrintPath()
            {
                Console.WriteLine(this.AntIndex);
                this._CurrentPheromonePath.ForEach(path =>
                {
                    Console.WriteLine("od: " + path.StartVertex + " do " + path.EndVertex + " -> ");
                });
            }
        }
    }
}
