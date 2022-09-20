using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace AntColonyNamespace
{
    internal class AntColony
    {
        //Parametr do kontroli wplywu feromonow
        private readonly double _ALFA;

        //Paramter do kontroli wplywu dlugosci krawedzi
        private readonly double _BETA;

        //Definiuje wplyw eksploracji vs eksploatacji
        private readonly double _q0;

        //Paramter kontrolujacy wyparowywanie feromonow
        private readonly double _TAU;

        //Liczba iteracji - ilosc cykli
        private readonly double _NumberOfIterations;

        //Pojemnosc ciezarowki/mrowki
        private readonly double _MaxCapacity;

        //Ilosc miast/wierzcholkow
        private readonly int _NumberOfCitiesWithDepot;

        //Ilosc ciezarowek/definiuje ile bedzie tras
        private readonly int _NumberOfTrucks;

        //Indeks depotu/startowego wierzcholka
        private readonly int _Depot;

        //Lista mrowek/posiada ilosc mrowek
        private List<Ant> Ants;

        public CompletedGraph CitiesGraph;

        public AntColony(
            double ALFA,
            double BETA,
            double q0,
            double TAU,
            int NumberOfAnts,
            int NumberOfIterations,
            string pathToBenchmarkData
        )
        {
            this.CitiesGraph = new CompletedGraph();
            this._ALFA = ALFA;
            this._BETA = BETA;
            this._q0 = q0;
            this._TAU = TAU;
            this._NumberOfIterations = NumberOfIterations;

            this.Ants = new List<Ant>(NumberOfAnts);
            for (int i = 0; i < NumberOfAnts; ++i)
            {
                this.AddAntToTheColony();
            }

            var allBenchmarkLines = System.IO.File.ReadAllLines(pathToBenchmarkData);
            var lineCounter = 0;
            foreach (var line in allBenchmarkLines)
            {
                ++lineCounter;
                if (lineCounter == 1)
                {
                    this._NumberOfCitiesWithDepot = int.Parse(Regex.Split(line, @"\D+")[1]);
                }
                else if (lineCounter == 2)
                {
                    this._MaxCapacity = int.Parse(Regex.Split(line, @"\D+")[1]);
                }
                else if (lineCounter == 3)
                {
                    this._NumberOfTrucks = int.Parse(Regex.Split(line, @"\D+")[1]);
                }
                else if (
                    this._NumberOfCitiesWithDepot != -1
                    && lineCounter > 4
                    && lineCounter <= this._NumberOfCitiesWithDepot + 4
                )
                {
                    var dataFromLine = Regex.Split(line, @"\D+");
                    var numbersFromLine = Array.ConvertAll(dataFromLine, str => int.Parse(str));
                    var capicityFromBenchmark = Regex.Split(
                        allBenchmarkLines[lineCounter + this._NumberOfCitiesWithDepot],
                        @"\D+"
                    );
                    this.CitiesGraph.AddCity(
                        numbersFromLine[0] - 1,
                        numbersFromLine[1],
                        numbersFromLine[2],
                        int.Parse(capicityFromBenchmark[1])
                    );
                }
                else if (
                    this._NumberOfCitiesWithDepot != 1
                    && lineCounter == 2 * this._NumberOfCitiesWithDepot + 7
                )
                {
                    this._Depot = int.Parse(Regex.Split(line, @"\D+")[0]) - 1;
                }
                else { }
            }
            this.CitiesGraph.CreateCompletedGraphBasedOnCityCoord();
        }

        public void AddAntToTheColony()
        {
            this.Ants.Add(new Ant(this, this.Ants.Count));
        }

        public void StartSolvingProblemInSeries() //Sekwencyjnie
        {
            for (int iteration = 0; iteration < this._NumberOfIterations; ++iteration)
            {
                this.Ants.ForEach(ant =>
                {
                    ant.StartCreatingItinerary();
                });
            }

            // this.Ants.ForEach(ant =>
            // {
            //     ant.PrintPath();
            // });
        }

        public void StartSolvingProblemParallel() { } //Rownolegle

        public class Ant
        {
            private int _AntIndex;

            private Possibilities _Possibilities;

            private int _CurrentCityIndex;

            private double _CurrentCapicity;

            private List<EdgeWithDestinationCity> _CurrentAntPath;

            private int _NumberOfUnvisitedCitiesWithoutDepot;

            private AntColony _AntColony;

            public Ant(AntColony AntColony, int Index)
            {
                this._AntColony = AntColony;
                this._Possibilities = new Possibilities(
                    this._AntColony._ALFA,
                    this._AntColony._BETA
                );
                this._CurrentCityIndex = this._AntColony._Depot;

                this._CurrentAntPath = new List<EdgeWithDestinationCity>();
                this._NumberOfUnvisitedCitiesWithoutDepot =
                    this._AntColony._NumberOfCitiesWithDepot - 1;
                this._AntIndex = Index;
                this._CurrentCapicity = 0;
            }

            public bool CanAntMoveToNextCity()
            {
                return this._CurrentCapicity >= this._AntColony._MaxCapacity;
            }

            public bool HasAntCreatedAllItinerary()
            {
                return this._NumberOfUnvisitedCitiesWithoutDepot == 0;
            }

            public void StartCreatingItinerary()
            {
                if (!this.HasAntCreatedAllItinerary())
                {
                    //Szukamy marszrut
                    //Szukamy wierzcholkow zeby do nich isc
                    //Mozemy tez wrocic do bazy
                }
                else
                {
                    //Marszruty znalezione
                    //Proces aktualizacji,liczenia, itp
                }
            }

            public EdgeWithDestinationCity ChoosePath()
            {
                foreach (
                    var possibleEdge in this._AntColony.CitiesGraph.GetEdgesFromCity(
                        this._CurrentCityIndex
                    )
                )
                {
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
                    return this._CurrentAntPath.Any(
                        partOfPath => partOfPath.DestinationCity == vertex
                    );
                }
            }

            public void MoveToTheNextEdge()
            {
                var choosenEdge = this.ChoosePath();
                this._CurrentAntPath.Add(choosenEdge);
                this._CurrentCityIndex = choosenEdge.DestinationCity;
                this._Possibilities.RestartAllValues();

                //Lokalne aktualizowanie feromonów
                double updatedPheromoneLevel =
                    (1 - this._AntColony._TAU) * choosenEdge.PheromoneLevel
                    + this._AntColony._TAU * 0;
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
                this._CurrentAntPath.ForEach(edge =>
                {
                    distance += edge.Distance;
                });

                return distance;
            }

            public void PrintPath()
            {
                Console.WriteLine("Index mrowki: " + this._AntIndex);
                Console.Write(0);
                this._CurrentAntPath.ForEach(path =>
                {
                    Console.Write(" -> " + path.DestinationCity);
                });
                Console.WriteLine();
                Console.WriteLine("------------");
            }
        }
    }
}
