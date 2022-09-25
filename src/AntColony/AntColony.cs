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

        private readonly double _LimitOfBaseReturning = 0.9;

        //Liczba iteracji - ilosc cykli
        private readonly double _NumberOfIterations;

        //Pojemnosc ciezarowki/mrowki
        private readonly double _MaxCapacityOfTruck;

        //Ilosc miast/wierzcholkow
        private readonly int _NumberOfCitiesWithDepot;

        //Ilosc ciezarowek/definiuje ile bedzie tras
        private readonly int _NumberOfTrucks;

        //Indeks depotu/startowego wierzcholka
        private readonly int _Depot;

        //Lista mrowek/posiada ilosc mrowek
        private List<Ant> _Ants;

        public BenchmarkGraph _CitiesGraph;

        public PheromoneMatrix _PheromoneMatrix;

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
            this._CitiesGraph = new BenchmarkGraph();
            this._ALFA = ALFA;
            this._BETA = BETA;
            this._q0 = q0;
            this._TAU = TAU;
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
                else if (
                    this._NumberOfCitiesWithDepot != 1
                    && lineCounter == 2 * this._NumberOfCitiesWithDepot + 7
                )
                {
                    this._Depot = int.Parse(Regex.Split(line, @"\D+")[0]) - 1;
                }
                else { }
            }
            this._CitiesGraph.CreateCompletedGraphBasedOnCityCoord();
            this._PheromoneMatrix = new PheromoneMatrix();

            this._Ants = new List<Ant>(NumberOfAnts);
            for (int i = 0; i < NumberOfAnts; ++i)
            {
                this.AddAntToTheColony();
            }
        }

        public void AddAntToTheColony()
        {
            this._Ants.Add(new Ant(this, this._Ants.Count));
        }

        public void StartSolvingProblemInSeries() //Sekwencyjnie
        {
            for (int iteration = 0; iteration < this._NumberOfIterations; ++iteration)
            {
                this._Ants.ForEach(ant =>
                {
                    ant.StartCreatingItinerary();
                });
            }

            this._Ants.ForEach(ant =>
            {
                ant.PrintPath();
            });
        }

        public void StartSolvingProblemParallel() { } //Rownolegle

        public class Ant
        {
            private readonly int _AntIndex;

            private Possibilities _Possibilities;

            private double _PossibilityToReturnToDepot;

            private int _CurrentCityIndex;

            private double _CurrentCapicityOfTruck;

            private int _NumberOfDoneItinerary;

            private double _CityDemandsToServe;

            private int _NumberOfRemainingTrucks;

            private GiantTourSolution _GiantSolution;

            private List<double> _CapacitiesOfDoneTrucks;

            //DO TEGO MOZE NA MACIERZ FEROMONOW xD OSOBNA KLASA

            private int _NumberOfUnvisitedCustomers;

            private AntColony _AntColony;

            public Ant(AntColony AntColony, int Index)
            {
                this._AntColony = AntColony;
                this._Possibilities = new Possibilities(
                    this._AntColony._ALFA,
                    this._AntColony._BETA
                );
                this._CurrentCityIndex = this._AntColony._Depot;

                this._NumberOfUnvisitedCustomers = this._AntColony._NumberOfCitiesWithDepot - 1;
                this._AntIndex = Index;
                this._CurrentCapicityOfTruck = 0;
                this._PossibilityToReturnToDepot = 0.1;
                this._NumberOfRemainingTrucks = this._AntColony._NumberOfTrucks;
                this._GiantSolution = new GiantTourSolution();
                this._NumberOfDoneItinerary = 0;

                this._CapacitiesOfDoneTrucks = new List<double>();

                this._CityDemandsToServe = this._AntColony._CitiesGraph.GetTotalDemandOfCities();
            }

            public void StartCreatingItinerary()
            {
                while (!this.AreAllCitiesVisited() || !this.IsAntInDepot())
                {
                    //Szukamy marszrut
                    //Szukamy wierzcholkow zeby do nich isc
                    //Mozemy tez wrocic do bazy

                    /*Sprawdzamy, czy:
                    - nie jestesmy wlasnie w depocie
                    - wracamy jesli zmiescimy sie w prawdopodobienstwie od 0 do PossibilityToReturnToDepot
                    - musimy sie upewnic ze wracajac szybciej nie spowodujemy ze reszta
                    ciezarowek nie sprosta wymaganiom klientow*/

                    //Jesli tu jestemsy to znaczy ze nie jestesmy w depocie
                    //ale nie ma juz customerow wiec musimy wrocic

                    // else if (
                    //     !this.IsAntInDepot() &&
                    //     this._CurrentCityIndex != 0
                    //     && this._CityDemandsToServe
                    //         / (this._NumberOfRemainingTrucks * this._AntColony._MaxCapacityOfTruck)
                    //         < this._AntColony._LimitOfBaseReturning
                    //     && this._NumberOfRemainingTrucks > 1
                    // )
                    // {
                    //     this.ReturnToDepot();
                    // }
                    this.MoveUp();
                }

                while (this._NumberOfRemainingTrucks != 0)
                {
                    this._GiantSolution.AddNoDepotLeavingPathOfSolution();
                    this._CapacitiesOfDoneTrucks.Add(0);
                    this.DecreaseNumberOfRemainingTracks();
                }

                this.PrintPath();
                Console.WriteLine("-----------------||------------------");
                this._GiantSolution.PrintItineraryAllApart(this._CapacitiesOfDoneTrucks); //MOZE DAC ZEBY SOLUTION TRZYMALO DANE O ITINERARIES
                Console.WriteLine(Math.Round(this._GiantSolution.GetGiantIteneraryDistance(), 2));

                //Marszruty znalezione
                //Proces aktualizacji,liczenia, itp
                //Aktualizacji calej marszruty...czyli ewaporacja i tak dalej
            }

            private void ReturnToDepot(EdgeWithDestinationCity pickedPath)
            {
                this._GiantSolution.AddPathOfSolution(
                    this._CurrentCityIndex,
                    pickedPath.EdgeToDestCity,
                    pickedPath.DestinationCity
                );

                this._CapacitiesOfDoneTrucks.Add(this._CurrentCapicityOfTruck);
                this._CurrentCapicityOfTruck = 0;
                this._CurrentCityIndex = pickedPath.DestinationCity;
                ++this._NumberOfDoneItinerary;
                --this._NumberOfRemainingTrucks;

                this._Possibilities.RestartAllValues();
            }

            private void MoveToTheNextCity(EdgeWithDestinationCity pickedPath)
            {
                this._GiantSolution.AddPathOfSolution(
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

                this._Possibilities.CountProbabilities();

                EdgeWithDestinationCity pickedPath;
                double randomNumberFrom0To1 = (new Random()).NextDouble();

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

            private EdgeWithDestinationCity GetEdgeToDepot()
            {
                return this._AntColony._CitiesGraph.GetEdgeToDepot(this._CurrentCityIndex);
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

            private bool DidAntVisitCity(int cityIndex)
            {
                return this._GiantSolution.IsCityVisited(cityIndex);
            }

            private void UpdateItineraryAfterMovingToNextCity() { }

            private void UpdateItineraryAfterReturningToDepot() { }

            //SPORNE
            private void DecreaseTotalDemandToServe(EdgeWithDestinationCity chosenEdge)
            {
                this._CityDemandsToServe -= this._AntColony._CitiesGraph.GetDemandOfCity(
                    chosenEdge.DestinationCity
                );
            }

            private void DecreaseNumberOfUnvisitedCustomers()
            {
                --this._NumberOfUnvisitedCustomers;
            }

            private void DecreaseNumberOfRemainingTracks()
            {
                --this._NumberOfRemainingTrucks;
            }

            private void ResetCapacityOfCurrentTruck()
            {
                this._CurrentCapicityOfTruck = 0;
            }

            private void IncreaseDoneItinerary()
            {
                ++this._NumberOfDoneItinerary;
            }

            private void AddCapacityOfDoneItinerary(double capacity)
            {
                this._CapacitiesOfDoneTrucks.Add(this._CurrentCapicityOfTruck);
            }

            private void AddCapacityToCurrentTruck(int cityIndex)
            {
                this._CurrentCapicityOfTruck += this._AntColony._CitiesGraph.GetDemandOfCity(
                    cityIndex
                );
            }

            private void ChangeCurrentCity(int newCity)
            {
                this._CurrentCityIndex = newCity;
            }

            public void UpdatePheromonesOnEdges() { }

            public void EvaporatePheromonesOnEdges() { }

            private bool CanAntMoveToNextCity(int cityIndex)
            {
                return this._CurrentCapicityOfTruck
                        + this._AntColony._CitiesGraph.GetDemandOfCity(cityIndex)
                    <= this._AntColony._MaxCapacityOfTruck;
            }

            private bool CanAntReturnToDepotEarlier()
            {
                //Czy pozostale trucki razy ich pojemnosc
                //dadza rade z pozostalymi demands
                return this._CityDemandsToServe
                        / (
                            (this._NumberOfRemainingTrucks - 1)
                            * this._AntColony._MaxCapacityOfTruck
                        )
                        < this._AntColony._LimitOfBaseReturning
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

            public void PrintPath()
            {
                Console.WriteLine("Index mrowki: " + this._AntIndex);
                Console.WriteLine(this._GiantSolution.ToString());
                Console.WriteLine("------------");
            }
        }
    }
}
