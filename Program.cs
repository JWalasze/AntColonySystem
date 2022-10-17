using AntColonyNamespace;
using System.Diagnostics;

var coefAlpha = new List<double> { 0.01, 0.05, 0.1, 0.2, 0.3, 0.5, 0.75, 1, 2, 3, 5 };
var coefBeta = new List<double> { 0.5, 1, 1.5, 2, 3, 10, 30 };

var coefTau = new List<double> { 0.001, 0.005, 0.01, 0.05, 0.1, 0.2, 0.3, 0.5 };
var coefQ = new List<double> { 1, 100, 1000, 10000, 50000, 100000, 200000, 500000 };
var coefQ0 = new List<double> { 0.4, 0.5, 0.7, 0.75, 0.8, 0.9, 0.95 };
var coefAnts = new List<int> { 5, 10, 20, 30, 40 };
var coefIterat = new List<int> { 5, 50, 100, 500, 1000, 5000, 6000, 7000 };

List<double> foundDistances = new List<double>();
File.AppendAllText(
    "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/Solution1",
    "NOWE URUCHOMIENIE..." + Environment.NewLine + Environment.NewLine
);

foreach (var alpha in coefAlpha)
{
    foreach (var beta in coefBeta)
    {
        Console.WriteLine();
        Console.WriteLine("--------------Coef=" + alpha + ", " + beta + "---------------");
        File.AppendAllText(
            "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/Solution1",
            "--------------Coef="
                + alpha
                + ", "
                + beta
                + "---------------"
                + Environment.NewLine
                + Environment.NewLine
        );
        Console.WriteLine();
        for (int j = 0; j < 6; ++j)
        {
            AntColony antColony = new AntColony(
                alpha, //ALFA
                beta, //BETA
                0.9, //Q0
                0.2, //TAU
                1000000, //Q
                25, //ANTS
                5000, //ITERATIONS
                //"C:\\Users\\Kuba\\Desktop\\.NET_App\\AntColonySystem\\BenchmarkData\\A-n32-k5.txt"
                "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/A-n32-k5.txt"
            //"/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/E-n101-k8.txt"
            );

            try
            {
                var watch = new Stopwatch();
                watch.Start();
                antColony.StartSolvingProblemInSeries();
                //antColony.StartSolvingProblemParallel();
                watch.Stop();
                Console.WriteLine(
                    "Czas trwania: " + watch.Elapsed.Minutes + ":" + watch.Elapsed.Seconds
                );
            }
            catch
            {
                Console.WriteLine("Złapano wyjątek!!!!!!!!!!!!!");
            }
            //antColony.StartSolvingProblemParallel();
            foundDistances.Add(antColony.GetGiantTourSolutionDistance()); //Zrobic zeby start Solving zwrocil rozwiAZANIE
            if (antColony.GetGiantTourSolutionDistance() < 805)
            {
                Console.WriteLine(antColony.GetGiantTourSolution().GetItineraryAllApart());
                File.AppendAllText(
                    "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/Solution1",
                    antColony.GetGiantTourSolution().GetItineraryAllApart() + Environment.NewLine
                );
            }
            Console.WriteLine(
                "Blad: "
                    + 100
                        * (
                            antColony.GetGiantTourSolutionDistance()
                            - antColony.GetOptimalDistanceFromBenchmarkFile()
                        )
                        / antColony.GetOptimalDistanceFromBenchmarkFile()
            );
            File.AppendAllText(
                "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/Solution1",
                "Blad: "
                    + 100
                        * (
                            antColony.GetGiantTourSolutionDistance()
                            - antColony.GetOptimalDistanceFromBenchmarkFile()
                        )
                        / antColony.GetOptimalDistanceFromBenchmarkFile()
                    + Environment.NewLine
                    + Environment.NewLine
            );
        }
        Console.WriteLine("Srednia:" + foundDistances.Average());
        File.AppendAllText(
            "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/Solution1",
            foundDistances.Average().ToString() + Environment.NewLine
        );
        foundDistances.Clear();
    }
}

//Fajnie opisana matematyka
//https://sci-hub.se/https://www.tandfonline.com/doi/abs/10.1080/02522667.2005.10699639
