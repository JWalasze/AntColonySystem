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

        //Lista mrowek
        private List<Ant> _Ants;

        //Budowane rozwiazanie w postaci Grand Tour Representation
        private GiantTourSolution? BestFoundSolutionYet;

        //Graf z miastami i sciezkami
        public BenchmarkGraph _CitiesGraph;

        //Konstruktor
        public AntColony(
            double ALFA,
            double BETA,
            double q0,
            double TAU,
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

            this.BestFoundSolutionYet = null;

            this._Lmn = new Ant(this, 0).FindSolutionUsingNearestNeighbourTourAlgorithm();
            this._InitialPheromoneLevel = this._Q / (this._NumberOfCitiesWithDepot * this._Lmn);

            this._CitiesGraph.SetInitialPheromoneValues(this._InitialPheromoneLevel);
        }

        //Dodanie mrowki do kolonii
        public void AddAntToTheColony()
        {
            this._Ants.Add(new Ant(this, this._Ants.Count));
        }

        public double GetGiantTourDistance()
        {
            if (this.BestFoundSolutionYet != null)
            {
                return this.BestFoundSolutionYet.GetGiantTourDistance();
            }
            throw new Exception("No zleee");
        }

        //Glowna metoda rozpoczynajaca szukanie rozwiazania sekwencyjnie
        public void StartSolvingProblemInSeries()
        {
            for (int iteration = 0; iteration < this._NumberOfIterations; ++iteration)
            {
                this._Ants.ForEach(ant =>
                {
                    ant.StartCreatingItinerary();
                    //ant.PrintItineraryAllApart();
                    ant.UpdateBestFoundSolutionSoFar();
                });

                this._Ants.ForEach(ant =>
                {
                    ant.UpdatePheromonesTrials();
                    ant.ResetAntForNextItinerary();
                });

                this.EvaporateAllPathsAndUpdateBestFoundSolution();

                if (this.BestFoundSolutionYet != null)
                {
                    // Console.WriteLine(
                    //     "Najlepsza trasa jak dotad: "
                    //         + this.BestFoundSolutionYet.GetGiantTourDistance()
                    // );
                    // //Console.WriteLine(this.BestFoundSolutionYet.PrintItineraryAllApart());
                }
            }

            if (this.BestFoundSolutionYet != null)
            {
                Console.WriteLine("-------------------BEST SOLUTION------------------");
                Console.WriteLine(
                    "Najlepsza znaleziona trasa: "
                        + Math.Round(this.BestFoundSolutionYet.GetGiantTourDistance(), 2)
                );
                //Console.WriteLine(this.BestFoundSolutionYet.PrintItineraryAllApart());
            }

            //Console.WriteLine("-------------------CITY GRAPH AFTER ALOGRITHM------------------");
            //Console.WriteLine(this._CitiesGraph.ToString());
        }

        //Aktualizacja najlepiej znaleznionej trasy i wyparowanie feromonow ze wszystkich krawedzi
        private void EvaporateAllPathsAndUpdateBestFoundSolution()
        {
            if (this.BestFoundSolutionYet != null)
            {
                foreach (var tuple in this._CitiesGraph)
                {
                    foreach (var edge in tuple._Edges)
                    {
                        edge.EdgeToDestCity.PheromoneLevel =
                            (1 - this._TAU) * edge.EdgeToDestCity.PheromoneLevel;

                        if (this.BestFoundSolutionYet.IsEdgeInSolution(edge.EdgeToDestCity))
                        {
                            edge.EdgeToDestCity.PheromoneLevel +=
                                this._TAU
                                * (this._Q / this.BestFoundSolutionYet.GetGiantTourDistance());
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Niepopawnie zaktualizowane najlepsze rozwiazanie!!!");
            }
        }

        //Glowna metoda rozpoczynajaca szukanie rozwiazania rownolegle
        public void StartSolvingProblemParallel() { }

        public GiantTourSolution GetGiantTourSolution()
        {
            if (this.BestFoundSolutionYet != null)
            {
                return this.BestFoundSolutionYet;
            }
            throw new Exception("Kolejny blad");
        }

        public class Ant
        {
            private readonly int _AntIndex;

            private Possibilities _Possibilities;

            private int _CurrentCityIndex;

            private double _CurrentCapicityOfTruck;

            private double _CityDemandsToServe;

            private int _NumberOfRemainingTrucks;

            private int _NumberOfDoneItinerary;

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

                this._NumberOfUnvisitedCustomers = this._AntColony._NumberOfCitiesWithDepot - 1;

                this._CurrentCapicityOfTruck = 0;
                this._NumberOfRemainingTrucks = this._AntColony._NumberOfTrucks;
                this._GiantSolution = new GiantTourSolution();
                this._NumberOfDoneItinerary = 0;

                this._CityDemandsToServe = this._AntColony._CitiesGraph.GetTotalDemandOfCities();
            }

            public double FindSolutionUsingNearestNeighbourTourAlgorithm()
            {
                while (!this.AreAllCitiesVisited() || !this.IsAntInDepot())
                {
                    this.MoveUpNNTA();
                }

                while (this._NumberOfRemainingTrucks != 0)
                {
                    this._GiantSolution.AddNoDepotLivingPathToSolution();
                    this._GiantSolution.AddUsedCapacityOfTruck(0);
                    --this._NumberOfRemainingTrucks;
                    ++this._NumberOfDoneItinerary;
                }

                return this._GiantSolution.GetGiantTourDistance();
            }

            private void MoveUpNNTA()
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
                    throw new Exception("Zle znaleziona kolejna krawedz!!!");
                }
            }

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
                    ++this._NumberOfDoneItinerary;
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
                ++this._NumberOfDoneItinerary;
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

                this._CityDemandsToServe -= this._AntColony._CitiesGraph.GetDemandOfCity(
                    pickedPath.DestinationCity
                );
                this._CurrentCapicityOfTruck += this._AntColony._CitiesGraph.GetDemandOfCity(
                    pickedPath.DestinationCity
                );
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

            //Odparowywanie feromonow oraz pozostawienie czesci na sciezkach
            public void UpdatePheromonesTrials()
            {
                foreach (var tuple in this._AntColony._CitiesGraph)
                {
                    foreach (var path in tuple._Edges)
                    {
                        if (this._GiantSolution.IsEdgeInSolution(path.EdgeToDestCity))
                        {
                            //-------DO ZMIANY ZAPEWNE-------------
                            path.EdgeToDestCity.PheromoneLevel +=
                                this._AntColony._TAU * this._AntColony._InitialPheromoneLevel;
                        }
                    }
                }
            }

            public void ResetAntForNextItinerary()
            {
                this._CurrentCityIndex = 0;
                this._CurrentCapicityOfTruck = 0;

                this._CityDemandsToServe = this._AntColony._CitiesGraph.GetTotalDemandOfCities();
                this._NumberOfRemainingTrucks = this._AntColony._NumberOfTrucks;
                this._NumberOfDoneItinerary = 0;
                this._NumberOfUnvisitedCustomers = this._AntColony._NumberOfCitiesWithDepot - 1;
                this._GiantSolution.ResetSolution();
            }

            public double GetObjectiveFunction()
            {
                return this._GiantSolution.GetGiantTourDistance();
            }

            private bool DidAntVisitCity(int cityIndex)
            {
                return this._GiantSolution.IsCityVisited(cityIndex);
            }

            public void UpdateBestFoundSolutionSoFar()
            {
                if (
                    this._AntColony.BestFoundSolutionYet != null
                        && this._AntColony.BestFoundSolutionYet.GetGiantTourDistance()
                            > this._GiantSolution.GetGiantTourDistance()
                    || this._AntColony.BestFoundSolutionYet == null
                )
                {
                    this._AntColony.BestFoundSolutionYet = (GiantTourSolution)
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

            public void PrintGiantPath()
            {
                Console.WriteLine("Index mrowki: " + this._AntIndex);
                Console.WriteLine(this._GiantSolution.ToString());
                Console.WriteLine("------------");
            }

            public void PrintItineraryAllApart()
            {
                Console.WriteLine("----------||----------");
                Console.WriteLine();
                Console.WriteLine("Indeks mrówki: " + this._AntIndex);
                Console.WriteLine(this._GiantSolution.ToString());
                Console.WriteLine(
                    "Wartość funkcji celu: "
                        + Math.Round(this._GiantSolution.GetGiantTourDistance(), 2)
                );
                Console.WriteLine();
            }
        }
    }
}
