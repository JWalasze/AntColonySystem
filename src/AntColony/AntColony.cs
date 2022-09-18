using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace AntColonyNamespace
{
    internal class AntColony
    {
        //ALFA controls influence of TAU
        public readonly double _ALFA;

        //BETA controls influence of ETA
        public readonly double _BETA;

        //q0 determins the importance of exploration versus exploitation
        public readonly double _q0;

        private readonly double _NumberOfIterations;

        private readonly double _Capacity;

        //List of every ant in a colony
        private List<Ant> Ants;

        public Graph CitiesGraph;

        public AntColony(
            Graph _Graph,
            double ALFA,
            double BETA,
            double q0,
            int NumberOfAnts,
            int NumberOfIterations,
            string pathToBenchmarkData
        )
        {
            this.CitiesGraph = _Graph;
            this._ALFA = ALFA;
            this._BETA = BETA;
            this._q0 = q0;
            this._NumberOfIterations = NumberOfIterations;

            this.Ants = new List<Ant>();
            for (int i = 0; i < NumberOfAnts; ++i)
            {
                this.AddAntToTheColony();
            }

            var allBenchmarkLines = System.IO.File.ReadAllLines(pathToBenchmarkData);
            var lineCounter = 0;
            var dimension = -1; //ILOSC MIAST Z DEPOTEM
            var capacity = 0; //Pojemnosc mrowki
            foreach (var line in allBenchmarkLines) //DOKONCZYC...DODAC WIEZCHOLKI I KRAWEDZIE-WYLICZYC JE
            {
                ++lineCounter;
                Console.WriteLine(line);

                if (lineCounter == 1)
                {
                    dimension = int.Parse(Regex.Split(line, @"\D+")[1]);
                    Console.WriteLine(dimension);
                }
                else if (lineCounter == 2)
                {
                    capacity = int.Parse(Regex.Split(line, @"\D+")[1]);
                    Console.WriteLine(capacity);
                }
                else if (dimension != -1 && lineCounter > 3 && lineCounter <= dimension + 3)
                {
                    Console.WriteLine("->" + Regex.Split(line, @"\D+")[0]);
                    Console.WriteLine("->" + Regex.Split(line, @"\D+")[1]);
                    Console.WriteLine("->" + Regex.Split(line, @"\D+")[2]);
                }
                else if (
                    dimension != -1
                    && lineCounter > dimension + 4
                    && lineCounter < 2 * dimension + 4
                )
                {
                    Console.WriteLine("->" + Regex.Split(line, @"\D+")[0]);
                    Console.WriteLine("->" + Regex.Split(line, @"\D+")[1]);
                }
                else { }
            }
        }

        public void AddAntToTheColony()
        {
            this.Ants.Add(new Ant(this, 0));
        }

        public void StartLookingForPath()
        {
            //ITERACJE to nie to, pozniej zmienic
            for (int iteration = 0; iteration < this._NumberOfIterations; ++iteration)
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
            private static int AntsGlobalIndex = 0;
            private int AntIndex;

            //Obiekt do liczenia prawdopodobienstw wyboru krawedzi
            private Possibilities _Possibilities;

            //Wierzcholek z ktorego mrowka zaczyna podroz
            private int _InitialVertexIndex;

            //Zapisany index obecnego wierzcholka
            private int _CurrentVertexIndex;

            private List<EdgeWithDestinationCity> _CurrentPheromonePath;

            private AntColony _AntColony;

            public Ant(AntColony AntColony, int InitialVertexIndex)
            {
                this._AntColony = AntColony;
                this._Possibilities = new Possibilities(this._AntColony._BETA);
                this._InitialVertexIndex = InitialVertexIndex;
                this._CurrentVertexIndex = this._InitialVertexIndex;

                this._CurrentPheromonePath = new List<EdgeWithDestinationCity>();
                this.AntIndex = AntsGlobalIndex++;
            }

            public EdgeWithDestinationCity ChoosePath()
            {
                foreach (
                    var possibleEdge in this._AntColony.CitiesGraph.GetEdgesFromCity(
                        this._CurrentVertexIndex
                    )
                )
                {
                    //Nie bierzemy pod uwagę odwiedzonych wierzchołków
                    if (!this.DidAntVisitVertex(possibleEdge.DestinationCity))
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

            private EdgeWithDestinationCity ChoosePathBasedOnProb()
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
                if (vertex == 0)
                {
                    return true;
                }
                else
                {
                    return this._CurrentPheromonePath.Any(
                        partOfPath => partOfPath.DestinationCity == vertex
                    );
                }
            }

            public void MoveToTheNextEdge()
            {
                var choosenEdge = this.ChoosePath();
                this._CurrentPheromonePath.Add(choosenEdge);
                this._CurrentVertexIndex = choosenEdge.DestinationCity;
                Console.WriteLine();
                foreach (var item in _Possibilities.GetProbabilities())
                {
                    Console.WriteLine(item.Value);
                }
                this._Possibilities.RestartAllValues();

                //Lokalne aktualizowanie feromonów
                double updatedPheromoneLevel =
                    (1 - this._AntColony._ALFA) * choosenEdge.PheromoneLevel
                    + this._AntColony._ALFA * 0;
                //Na końcu powinna być initial value, ale nie wiadomo o co z tym chodzi
                choosenEdge.UpdatePheromoneLevel(updatedPheromoneLevel);
                //I jeszcze trzeba bedzie globalnie gdzie indziej,
                //ale globalne jeszcze trzeba dać gdzieś po skończonej iteracji,
                //że wszystkie mrówki znajdą swoje własne rozwiązanie...
                //Może dać klasę Solution, a wręcz napewno xD
                //Moze tez do macierzy feromonow xD
            }

            private double CalculateFindedPathDistance()
            {
                var distance = 0.0;
                this._CurrentPheromonePath.ForEach(edge =>
                {
                    distance += edge.Distance;
                });

                return distance;
            }

            public void PrintPath()
            {
                Console.WriteLine("Index mrowki: " + this.AntIndex);
                Console.Write(0);
                this._CurrentPheromonePath.ForEach(path =>
                {
                    Console.Write(" -> " + path.DestinationCity);
                });
                Console.WriteLine();
                Console.WriteLine("------------");
            }
        }
    }
}
