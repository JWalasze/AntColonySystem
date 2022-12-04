using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;

namespace AntColonyNamespace
{
    internal class AntColony
    {
        private readonly double _ALFA;

        private readonly double _BETA;

        private readonly double _q0;

        private readonly double _TAU;

        private readonly double _InitialPheromoneLevel;

        private readonly double _Lnn;

        private readonly double _Q;

        private readonly double _ETA;

        private readonly double _LimitOfBaseReturning = 0.9;

        private readonly double _NumberOfIterations;

        private readonly double _MaxCapacityOfTruck;

        private readonly int _NumberOfCitiesWithDepot;

        private readonly int _NumberOfTrucks;

        private readonly double _NumberOfTotalDemand;

        private readonly double _OptimalDistance;

        private readonly int _NumberOfThreads;

        private List<Ant> _Ants;

        private GiantTourSolution? _BestFoundSolutionYet;

        public BenchmarkGraph _CitiesGraph;

        public int _StagnationCounter;

        public int _StagnationnParameter;

        public bool HasSolutionChanged;

        public AntColony(
            double ALFA,
            double BETA,
            double q0,
            double TAU,
            double ETA,
            double Q,
            int StagnationCounter,
            int NumberOfAnts,
            int NumberOfIterations,
            int NumberOfThreads,
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
            this._StagnationnParameter = StagnationCounter;
            this._NumberOfIterations = NumberOfIterations;
            this._NumberOfThreads = NumberOfThreads;

            this._StagnationCounter = 0;

            var allBenchmarkLines = System.IO.File.ReadAllLines(
                // "C:\\Users\\Kuba Walaszek\\Desktop\\.NET_App\\BenchmarkData\\" + pathToBenchmarkData
                "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/"
                    + pathToBenchmarkData
            );
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
            this.HasSolutionChanged = false;

            this._Lnn = new Ant(this, 0).FindSolutionUsingNearestNeighbourTourAlgorithm();
            this._InitialPheromoneLevel = 1 / (this._NumberOfCitiesWithDepot * this._Lnn);

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
            var timeWatch = new Stopwatch();
            var timeWatch2 = new Stopwatch();

            for (int iteration = 0; iteration < this._NumberOfIterations; ++iteration)
            {
                timeWatch.Start();
                this._Ants.ForEach(ant =>
                {
                    ant.StartCreatingItinerary();
                });
                timeWatch.Stop();

                timeWatch2.Start();

                this._Ants.ForEach(ant =>
                {
                    if (ant.IsSolutionCorrect == true)
                    {
                        ant.UpdateBestFoundSolutionYet();
                    }
                });

                this.EvaporateAllPathsAndUpdateBestFoundSolution();

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

                this.PerformStagnationOperation();

                timeWatch2.Stop();
            }

            Console.WriteLine(
                "Stop(szukanie): " + timeWatch.Elapsed.Minutes + ":" + timeWatch.Elapsed.Seconds
            );
            Console.WriteLine(
                "Stop(aktualizacja): "
                    + timeWatch2.Elapsed.Minutes
                    + ":"
                    + timeWatch2.Elapsed.Seconds
            );

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

        private void PerformStagnationOperation()
        {
            if (this.HasSolutionChanged == true)
            {
                this._StagnationCounter = 0;
            }
            else
            {
                ++this._StagnationCounter;
                if (this._StagnationCounter == this._StagnationnParameter)
                {
                    this._StagnationCounter = 0;
                    this._CitiesGraph.SetInitialPheromoneValues(this._InitialPheromoneLevel);
                }
            }
            this.HasSolutionChanged = false;
        }

        public GiantTourSolution StartSolvingProblemParallel()
        {
            int numberOfAntsForOneThread = this._Ants.Count / this._NumberOfThreads;
            int moduloOfDivision = this._Ants.Count % this._NumberOfThreads;

            var timeWatch = new Stopwatch();
            var timeWatch2 = new Stopwatch();

            var listOfThreads = new List<int[]>();

            int firstAntForThread = 0;
            int lastAntForThread = numberOfAntsForOneThread - 1;
            int otherAntsToAdd = moduloOfDivision;

            if (this._Ants.Count <= this._NumberOfThreads)
            {
                for (int thread = 0; thread < this._NumberOfThreads; ++thread)
                {
                    listOfThreads.Add(new int[] { thread, thread });
                }
            }
            else
            {
                for (int thread = 0; thread < this._NumberOfThreads; ++thread)
                {
                    if (otherAntsToAdd != 0)
                    {
                        lastAntForThread += 1;
                        --otherAntsToAdd;
                    }

                    int firstAnt = firstAntForThread;
                    int lastAnt = lastAntForThread;
                    // var newThread = new Thread(
                    //     () => DelegateToSolveProblemParallel(firstAnt, lastAnt)
                    // );
                    listOfThreads.Add(new int[] { firstAnt, lastAnt });

                    firstAntForThread = lastAntForThread + 1;
                    lastAntForThread += numberOfAntsForOneThread;
                }
            }

            for (int iteration = 0; iteration < this._NumberOfIterations; ++iteration)
            {
                timeWatch.Start();
                // listOfThreads.ForEach(thread => thread.Start());
                // listOfThreads.ForEach(thread => thread.Join());
                // timeWatch.Stop();

                using (CountdownEvent counter = new CountdownEvent(this._NumberOfThreads))
                {
                    listOfThreads.ForEach(
                        thread =>
                            ThreadPool.QueueUserWorkItem(
                                _ =>
                                    this.DelegateToSolveProblemParallel(
                                        thread[0],
                                        thread[1],
                                        counter
                                    )
                            )
                    );

                    // for (int i = 0; i < this._NumberOfThreads; i++)
                    // {
                    //     ThreadPool.QueueUserWorkItem(
                    //         _ => this.DelegateToSolveProblemParallel(firstAnt, lastAnt)
                    //     );
                    // }
                    counter.Wait();
                }
                timeWatch.Stop();

                timeWatch2.Start();

                this._Ants.ForEach(ant =>
                {
                    if (ant.IsSolutionCorrect == true)
                    {
                        ant.UpdateBestFoundSolutionYet();
                    }
                });

                this.EvaporateAllPathsAndUpdateBestFoundSolution();

                this._Ants.ForEach(ant =>
                {
                    ant.ResetAntForNextItinerary();
                });

                this.PerformStagnationOperation();

                if (this._BestFoundSolutionYet == null)
                {
                    throw new Exception(
                        "Niepoprawnie znalezione najlepsze rozwiązanie do tej pory!!!"
                    );
                }

                timeWatch2.Stop();
            }

            Console.WriteLine(
                "Stop(szukanie): " + timeWatch.Elapsed.Minutes + ":" + timeWatch.Elapsed.Seconds
            );
            Console.WriteLine(
                "Stop(aktualizacja): "
                    + timeWatch2.Elapsed.Minutes
                    + ":"
                    + timeWatch2.Elapsed.Seconds
            );

            if (this._BestFoundSolutionYet == null)
            {
                throw new Exception("Niepoprawnie znalezione najlepsze rozwiązanie do tej pory!!!");
            }

            return this._BestFoundSolutionYet;
        }

        private void DelegateToSolveProblemParallel(
            int firstAntForThread,
            int lastAntForThread,
            CountdownEvent evt
        )
        {
            var ant = firstAntForThread;
            for (; ant <= lastAntForThread; ++ant)
            {
                //Console.WriteLine("M: " + ant);
                this._Ants[ant].StartCreatingItinerary();
            }
            evt.Signal();
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
                                path.EdgeToDestCity.PheromoneLevel +=
                                    this._ETA
                                    * (this._Q / this._BestFoundSolutionYet.GetGiantTourDistance());
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

            public bool IsSolutionCorrect;

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

                this.IsSolutionCorrect = false;

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
                        theShortestPath = possiblePath;
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

                if (this._NumberOfRemainingTrucks < 0)
                {
                    this.IsSolutionCorrect = false;
                }
                else
                {
                    this.IsSolutionCorrect = true;
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

            public void ResetAntForNextItinerary()
            {
                this._CurrentCityIndex = 0;
                this._CurrentCapicityOfTruck = 0;

                this._CityDemandsToServe = this._AntColony._NumberOfTotalDemand;
                this._NumberOfRemainingTrucks = this._AntColony._NumberOfTrucks;
                this._NumberOfUnvisitedCustomers = this._AntColony._NumberOfCitiesWithDepot - 1;
                this._GiantSolution.ResetSolution();

                this.IsSolutionCorrect = true;
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
                    this._AntColony.HasSolutionChanged = true;
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
