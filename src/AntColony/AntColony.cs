using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace AntColonyNamespace
{
    /*Glowna klasa zawierajaca graf wraz z mrowkami
    i dokonujaca szukanai marszrut*/
    internal class AntColony
    {
        //Parametr do kontroli wplywu feromonow na wybor sciezki
        private readonly double _ALFA;

        //Paramter do kontroli wplywu dlugosci krawedzi na wybor krawedzi
        private readonly double _BETA;

        //Definiuje wplyw eksploracji vs eksploatacji
        private readonly double _q0;

        //Paramter kontrolujacy wyparowywanie feromonow
        private readonly double _TAU;

        //Poczatkowa wartosc feromonow na krawedziach
        private readonly double _InitialPheromoneLevel;

        private readonly double _Lmn;

        private readonly double _Q;

        private readonly double _ETA;

        /*Wspolczynnik definiujacy przy jakim stosunku pozostalych pojemnosci ciezarowek
         i pozostalych rzadan ciezarowka moze wrocic wczesniej do depotu*/
        private readonly double _LimitOfBaseReturning = 0.9;

        //Liczba iteracji - ilosc cykli
        private readonly double _NumberOfIterations;

        //Pojemnosc pojedynczej ciezarowki
        private readonly double _MaxCapacityOfTruck;

        //Ilosc miast
        private readonly int _NumberOfCitiesWithDepot;

        //Ilosc ciezarowek/definiuje ile bedzie tras
        private readonly int _NumberOfTrucks;

        private readonly double _NumberOfTotalDemand;

        private readonly double _OptimalDistance;

        //Lista mrowek
        private List<Ant> _Ants;

        //Budowane rozwiazanie w postaci Grand Tour Representation
        private GiantTourSolution? _BestFoundSolutionYet;

        //Graf z miastami i sciezkami
        public BenchmarkGraph _CitiesGraph;

        //Konstruktor
        public AntColony(
            double ALFA,
            double BETA,
            double q0,
            double TAU,
            double ETA,
            double Q,
            int NumberOfAnts,
            int NumberOfIterations,
            string pathToBenchmarkData
        )
        {
            this._CitiesGraph = new BenchmarkGraph();
            this._ALFA = ALFA;
            this._BETA = BETA;
            this._q0 = q0;
            this._TAU = TAU;
            this._ETA = ETA;
            this._Q = Q;
            this._NumberOfIterations = NumberOfIterations;

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
                    this._MaxCapacityOfTruck = int.Parse(Regex.Split(line, @"\D+")[1]);
                }
                else if (lineCounter == 3)
                {
                    this._NumberOfTrucks = int.Parse(Regex.Split(line, @"\D+")[1]);
                }
                else if (lineCounter == 4)
                {
                    this._OptimalDistance = int.Parse(Regex.Split(line, @"\D+")[1]);
                }
                else if (
                    this._NumberOfCitiesWithDepot != -1
                    && lineCounter > 5
                    && lineCounter <= this._NumberOfCitiesWithDepot + 5
                )
                {
                    var dataFromLine = Regex.Split(line, @"\D+");
                    var numbersFromLine = Array.ConvertAll(dataFromLine, str => int.Parse(str));
                    var capicityFromBenchmark = Regex.Split(
                        allBenchmarkLines[lineCounter + this._NumberOfCitiesWithDepot],
                        @"\D+"
                    );
                    this._CitiesGraph.AddCity(
                        numbersFromLine[0] - 1,
                        numbersFromLine[1],
                        numbersFromLine[2],
                        int.Parse(capicityFromBenchmark[1])
                    );
                }
                else { }
            }

            this._CitiesGraph.CreateCompletedGraphBasedOnCityCoord();

            this._Ants = new List<Ant>(NumberOfAnts);
            for (int i = 0; i < NumberOfAnts; ++i)
            {
                this.AddAntToTheColony();
            }

            this._BestFoundSolutionYet = null;

            this._Lmn = new Ant(this, 0).FindSolutionUsingNearestNeighbourTourAlgorithm();
            this._InitialPheromoneLevel = 1 / (this._NumberOfCitiesWithDepot * this._Lmn);

            this._CitiesGraph.SetInitialPheromoneValues(this._InitialPheromoneLevel);
            this._NumberOfTotalDemand = this._CitiesGraph.GetTotalDemandOfCities();
        }

        public void AddAntToTheColony()
        {
            this._Ants.Add(new Ant(this, this._Ants.Count));
        }

        public double GetGiantTourSolutionDistance()
        {
            if (this._BestFoundSolutionYet != null)
            {
                return this._BestFoundSolutionYet.GetGiantTourDistance();
            }

            throw new Exception("Nie znaleziono żadnego rozwiązania!!!");
        }

        public double GetOptimalDistanceFromBenchmarkFile()
        {
            return this._OptimalDistance;
        }

        public GiantTourSolution StartSolvingProblemInSeries()
        {
            for (int iteration = 0; iteration < this._NumberOfIterations; ++iteration)
            {
                this._Ants.ForEach(ant =>
                {
                    ant.StartCreatingItinerary();
                    ant.UpdateBestFoundSolutionYet();
                });

                // Console.WriteLine("Graf przed update wszystkie: " + this._CitiesGraph.ToString());
                // Console.WriteLine();

                this._Ants.ForEach(ant =>
                {
                    ant.UpdatePheromonesOnVisitedPaths();
                    //ant.PrintItinerariesAllApart();
                });

                // Console.WriteLine("Graf po update wszystkie" + this._CitiesGraph.ToString());
                // this._BestFoundSolutionInCurrentIteration.GetItineraryAllApart();
                // Console.WriteLine(
                //     "Sciezka dlugosc:"
                //         + this._BestFoundSolutionInCurrentIteration.GetGiantTourDistance()
                // );
                // Console.WriteLine();

                this.EvaporateAllPathsAndUpdateBestFoundSolution();

                // Console.WriteLine("Graf po akyualizacji najlepsze" + this._CitiesGraph.ToString());
                // Console.WriteLine();

                this._Ants.ForEach(ant =>
                {
                    ant.ResetAntForNextItinerary();
                });

                if (this._BestFoundSolutionYet == null)
                {
                    throw new Exception(
                        "Niepoprawnie znalezione najlepsze rozwiązanie do tej pory!!!"
                    );
                }
            }

            if (this._BestFoundSolutionYet == null)
            {
                throw new Exception("Niepoprawnie znalezione najlepsze rozwiązanie do tej pory!!!");
            }

            return this._BestFoundSolutionYet;
        }

        public void ResetColonyForNextRun()
        {
            this._BestFoundSolutionYet = null;
            this._CitiesGraph.SetInitialPheromoneValues(this._InitialPheromoneLevel);
        }

        public void StartSolvingProblemParallel()
        {
            // for (int iteration = 0; iteration < this._NumberOfIterations; ++iteration)
            // {
            //     var listOfThreads = new List<Thread>();
            //     foreach (var ant in this._Ants)
            //     {
            //         var newThread = new Thread(() => DelegateToSolveProblemParallel(ant));
            //         listOfThreads.Add(newThread);
            //     }

            //     listOfThreads.ForEach(thread => thread.Start());
            //     listOfThreads.ForEach(thread => thread.Join());
            //     //listOfThreads.ForEach(thread => Console.WriteLine(thread.IsAlive));

            //     this._Ants.ForEach(ant =>
            //     {
            //         ant.UpdateBestFoundSolutionYet();
            //     });

            //     if (this._BestFoundSolutionInCurrentIteration == null)
            //     {
            //         throw new Exception(
            //             "Niepoprawnie znalezione najlepsze rozwiązanie w iteracji!!!"
            //         );
            //     }

            //     this._Ants.ForEach(ant =>
            //     {
            //         ant.UpdatePheromonesOnVisitedPaths();
            //     });

            //     this.EvaporateAllPathsAndUpdateBestFoundSolution();

            //     if (
            //         this._BestFoundSolutionYet == null
            //         || this._BestFoundSolutionYet.GetGiantTourDistance()
            //             > this._BestFoundSolutionInCurrentIteration.GetGiantTourDistance()
            //     )
            //     {
            //         this._BestFoundSolutionYet = (GiantTourSolution)
            //             this._BestFoundSolutionInCurrentIteration.Clone();
            //     }

            //     this._Ants.ForEach(ant =>
            //     {
            //         ant.ResetAntForNextItinerary();
            //     });

            //     if (this._BestFoundSolutionYet == null)
            //     {
            //         throw new Exception(
            //             "1) Niepoprawnie znalezione najlepsze rozwiązanie do tej pory!!!"
            //         );
            //     }
            // }

            // if (this._BestFoundSolutionYet != null)
            // {
            //     Console.WriteLine("-------------------BEST SOLUTION------------------");
            //     Console.WriteLine(
            //         "Najlepsza znaleziona trasa: "
            //             + Math.Round(this._BestFoundSolutionYet.GetGiantTourDistance(), 2)
            //     );
            //     // File.AppendAllText(
            //     //     "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/Solution1",
            //     //     "Najlepsza znaleziona trasa: "
            //     //         + Math.Round(this._BestFoundSolutionYet.GetGiantTourDistance(), 2)
            //     //         + Environment.NewLine
            //     // );
            // }
            // else
            // {
            //     throw new Exception("Niepoprawnie znalezione najlepsze rozwiązanie do tej pory!!!");
            // }
        }

        private void DelegateToSolveProblemParallel(Ant ant)
        {
            ant.StartCreatingItinerary();
        }

        private void EvaporateAllPathsAndUpdateBestFoundSolution()
        {
            foreach (var tuple in this._CitiesGraph)
            {
                foreach (var path in tuple._Edges)
                {
                    if (tuple._City.Index < path.DestinationCity)
                    {
                        if (this._BestFoundSolutionYet != null)
                        {
                            path.EdgeToDestCity.PheromoneLevel *= (1 - this._TAU);
                            if (this._BestFoundSolutionYet.IsEdgeInSolution(path.EdgeToDestCity))
                            {
                                path.EdgeToDestCity.PheromoneLevel =
                                    this._ETA
                                    * (1 / this._BestFoundSolutionYet.GetGiantTourDistance());
                            }
                        }
                    }
                }
            }
        }

        public class Ant
        {
            private readonly int _AntIndex;

            private Possibilities _Possibilities;

            private int _CurrentCityIndex;

            private double _CurrentCapicityOfTruck;

            private double _CityDemandsToServe;

            private int _NumberOfRemainingTrucks;

            private int _NumberOfUnvisitedCustomers;

            private GiantTourSolution _GiantSolution;

            private AntColony _AntColony;

            public Ant(AntColony AntColony, int Index)
            {
                this._AntIndex = Index;
                this._AntColony = AntColony;
                this._Possibilities = new Possibilities(
                    this._AntColony._ALFA,
                    this._AntColony._BETA
                );
                this._CurrentCityIndex = 0;
                this._CurrentCapicityOfTruck = 0;

                this._NumberOfUnvisitedCustomers = this._AntColony._NumberOfCitiesWithDepot - 1;
                this._NumberOfRemainingTrucks = this._AntColony._NumberOfTrucks;
                this._GiantSolution = new GiantTourSolution();

                this._CityDemandsToServe = this._AntColony._CitiesGraph.GetTotalDemandOfCities();
            }

            //------------------------NearestNeighbourTourAlgorithm------------------------
            public double FindSolutionUsingNearestNeighbourTourAlgorithm()
            {
                while (!this.AreAllCitiesVisited() || !this.IsAntInDepot())
                {
                    this.MoveUpInNearestNeighbourTourAlgorithm();
                }

                while (this._NumberOfRemainingTrucks != 0)
                {
                    this._GiantSolution.AddNoDepotLivingPathToSolution();
                    this._GiantSolution.AddUsedCapacityOfTruck(0);
                    --this._NumberOfRemainingTrucks;
                }

                return this._GiantSolution.GetGiantTourDistance();
            }

            private void MoveUpInNearestNeighbourTourAlgorithm()
            {
                var edgesFromCity = this._AntColony._CitiesGraph.GetEdgesFromCity(
                    this._CurrentCityIndex
                );
                EdgeWithDestinationCity? theShortestPath = null;

                foreach (var possiblePath in edgesFromCity)
                {
                    if (possiblePath.DestinationCity == 0)
                    {
                        if (this.CanAntReturnToDepotEarlier())
                        {
                            if (
                                theShortestPath == null
                                || theShortestPath.EdgeToDestCity.Distance
                                    > possiblePath.EdgeToDestCity.Distance
                            )
                            {
                                theShortestPath = possiblePath;
                            }
                        }
                    }
                    else
                    {
                        if (
                            !this.DidAntVisitCity(possiblePath.DestinationCity)
                            && this.CanAntMoveToNextCity(possiblePath.DestinationCity)
                        )
                        {
                            if (
                                theShortestPath == null
                                || theShortestPath.EdgeToDestCity.Distance
                                    > possiblePath.EdgeToDestCity.Distance
                            )
                            {
                                theShortestPath = possiblePath;
                            }
                        }
                    }
                }

                if (theShortestPath != null)
                {
                    if (theShortestPath.DestinationCity == 0)
                    {
                        this.ReturnToDepot(theShortestPath);
                    }
                    else
                    {
                        this.MoveToTheNextCity(theShortestPath);
                    }
                }
                else
                {
                    throw new Exception("Błąd w znalezieniu kolejnej ścieżki w NNTA!!!");
                }
            }

            //-----------------------------------------------------------------------------

            public void StartCreatingItinerary()
            {
                while (!this.AreAllCitiesVisited() || !this.IsAntInDepot())
                {
                    this.MoveUp();
                }

                while (this._NumberOfRemainingTrucks != 0)
                {
                    this._GiantSolution.AddNoDepotLivingPathToSolution();
                    this._GiantSolution.AddUsedCapacityOfTruck(0);
                    --this._NumberOfRemainingTrucks;
                }
            }

            private void ReturnToDepot(EdgeWithDestinationCity pickedPath)
            {
                this._GiantSolution.AddPathToGiantSolution(
                    this._CurrentCityIndex,
                    pickedPath.EdgeToDestCity,
                    pickedPath.DestinationCity
                );

                this._GiantSolution.AddUsedCapacityOfTruck(this._CurrentCapicityOfTruck);
                this._CurrentCapicityOfTruck = 0;
                this._CurrentCityIndex = pickedPath.DestinationCity;
                --this._NumberOfRemainingTrucks;

                this._Possibilities.RestartAllValues();
            }

            private void MoveToTheNextCity(EdgeWithDestinationCity pickedPath)
            {
                this._GiantSolution.AddPathToGiantSolution(
                    this._CurrentCityIndex,
                    pickedPath.EdgeToDestCity,
                    pickedPath.DestinationCity
                );

                var visitedCityDemand = this._AntColony._CitiesGraph.GetDemandOfCity(
                    pickedPath.DestinationCity
                );
                this._CityDemandsToServe -= visitedCityDemand;
                this._CurrentCapicityOfTruck += visitedCityDemand;
                this._CurrentCityIndex = pickedPath.DestinationCity;
                --this._NumberOfUnvisitedCustomers;

                this._Possibilities.RestartAllValues();
            }

            private void MoveUp()
            {
                foreach (
                    var possiblePath in this._AntColony._CitiesGraph.GetEdgesFromCity(
                        this._CurrentCityIndex
                    )
                )
                {
                    if (possiblePath.DestinationCity == 0)
                    {
                        if (this.CanAntReturnToDepotEarlier())
                        {
                            this._Possibilities.CountNominatorAndUpdateDenominator(possiblePath);
                        }
                    }
                    else
                    {
                        if (
                            !this.DidAntVisitCity(possiblePath.DestinationCity)
                            && this.CanAntMoveToNextCity(possiblePath.DestinationCity)
                        )
                        {
                            this._Possibilities.CountNominatorAndUpdateDenominator(possiblePath);
                        }
                    }
                }

                EdgeWithDestinationCity pickedPath;
                double randomNumberFrom0To1 = (new Random()).NextDouble();

                if (!this._Possibilities.CheckIfNominatorsAreEmpty())
                {
                    this._Possibilities.CountProbabilities();

                    if (randomNumberFrom0To1 <= this._AntColony._q0)
                    {
                        pickedPath = this._Possibilities.GetMaxNominator().Key;
                    }
                    else
                    {
                        pickedPath = this.ChoosePathBasedOnProb();
                    }

                    if (pickedPath.DestinationCity == 0)
                    {
                        this.ReturnToDepot(pickedPath);
                    }
                    else
                    {
                        this.MoveToTheNextCity(pickedPath);
                    }
                }
                else
                {
                    this.ReturnToDepot(
                        this._AntColony._CitiesGraph.GetEdgeWithDestinationCityToDepot(
                            this._CurrentCityIndex
                        )
                    );
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

                throw new Exception(
                    "Niepoprawnie wybrana sciezka na podstawie prawdopodobienstw!!!"
                );
            }

            public void UpdatePheromonesOnVisitedPaths()
            {
                foreach (var tuple in this._AntColony._CitiesGraph)
                {
                    foreach (var path in tuple._Edges)
                    {
                        if (tuple._City.Index < path.DestinationCity)
                        {
                            if (this._GiantSolution.IsEdgeInSolution(path.EdgeToDestCity))
                            {
                                path.EdgeToDestCity.PheromoneLevel += (
                                    this._AntColony._TAU
                                    *
                                    //* (1 / this._GiantSolution.GetGiantTourDistance())
                                    this._AntColony._InitialPheromoneLevel
                                );
                                //Console.WriteLine(c - path.EdgeToDestCity.PheromoneLevel);
                            }
                        }
                    }
                }
            }

            public void ResetAntForNextItinerary()
            {
                this._CurrentCityIndex = 0;
                this._CurrentCapicityOfTruck = 0;

                this._CityDemandsToServe = this._AntColony._NumberOfTotalDemand;
                this._NumberOfRemainingTrucks = this._AntColony._NumberOfTrucks;
                this._NumberOfUnvisitedCustomers = this._AntColony._NumberOfCitiesWithDepot - 1;
                this._GiantSolution.ResetSolution();
            }

            public double GetGiantTourDistance()
            {
                return this._GiantSolution.GetGiantTourDistance();
            }

            private bool DidAntVisitCity(int cityIndex)
            {
                return this._GiantSolution.IsCityVisited(cityIndex);
            }

            public void UpdateBestFoundSolutionYet()
            {
                if (
                    this._AntColony._BestFoundSolutionYet == null
                    || this._AntColony._BestFoundSolutionYet.GetGiantTourDistance()
                        > this._GiantSolution.GetGiantTourDistance()
                )
                {
                    this._AntColony._BestFoundSolutionYet = (GiantTourSolution)
                        this._GiantSolution.Clone();
                }
            }

            private bool CanAntMoveToNextCity(int cityIndex)
            {
                return this._CurrentCapicityOfTruck
                        + this._AntColony._CitiesGraph.GetDemandOfCity(cityIndex)
                    <= this._AntColony._MaxCapacityOfTruck;
            }

            private bool CanAntReturnToDepotEarlier()
            {
                if (this._NumberOfRemainingTrucks == 1)
                {
                    return this._NumberOfUnvisitedCustomers == 0;
                }

                return this._CityDemandsToServe
                        / (
                            (this._NumberOfRemainingTrucks - 1)
                            * this._AntColony._MaxCapacityOfTruck
                        )
                        <= this._AntColony._LimitOfBaseReturning
                    || this._NumberOfUnvisitedCustomers == 0;
            }

            private bool AreAllCitiesVisited()
            {
                return this._NumberOfUnvisitedCustomers == 0;
            }

            private bool IsAntInDepot()
            {
                return this._CurrentCityIndex == 0;
            }

            public void PrintItinerariesAllApart()
            {
                Console.WriteLine("----------||----------" + Environment.NewLine);
                Console.WriteLine("Indeks mrówki: " + this._AntIndex);
                Console.WriteLine(this._GiantSolution.ToString());
                Console.WriteLine(
                    "Wartość funkcji celu: "
                        + Math.Round(this._GiantSolution.GetGiantTourDistance(), 3)
                        + Environment.NewLine
                );
            }
        }
    }
}
