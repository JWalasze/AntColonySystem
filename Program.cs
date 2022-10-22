using AntColonyNamespace;
using System.Diagnostics;

var coefAlpha = new List<double> { 0.75, 0.8, 0.85, 0.9, 0.95, 1 };
var coefBeta = new List<double> { 0.5, 1, 1.5, 2, 3, 10, 30 };

var coefTau = new List<double> { 0.001, 0.005, 0.01, 0.05, 0.1, 0.2, 0.3, 0.5 };
var coefQ = new List<double> { 1, 100, 1000, 10000, 50000, 100000, 200000, 500000 };
var coefQ0 = new List<double> { 0.4, 0.5, 0.7, 0.75, 0.8, 0.9, 0.95 };
var coefAnts = new List<int> { 5, 10, 20, 30, 40 };
var coefIterat = new List<int> { 5, 50, 100, 500, 1000, 5000, 6000, 7000 };

var alphaBetaCoef = new List<(double, double)>
{
    (0.5, 0.5),
    (0.5, 1),
    (0.5, 2),
    (1, 0.5),
    (1, 1),
    (1, 2),
    (2, 0.5),
    (2, 1),
    (2, 2)
};

var numberOfProgramIterations = 6;


//foreach (var alpha in coefAlpha)
{
    foreach (var coef in coefAlpha)
    {
        double alpha = coef;
        double beta = 1;
        double q0 = 0.5;
        double tau = 0.01;
        double eta = 0.3;
        double Q = 1000000;
        int ants = 20;
        int iterations = 4000;

        AntColony antColony = new AntColony(
            alpha,
            beta,
            q0,
            tau,
            eta,
            Q,
            ants,
            iterations,
            //"C:\\Users\\Kuba\\Desktop\\.NET_App\\AntColonySystem\\BenchmarkData\\A-n32-k5.txt"
            //"/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/A-n32-k5.txt"
            "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/A-n39-k5.txt"
        //"/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/E-n101-k8.txt"
        );

        var statistics = new Statistics(
            numberOfProgramIterations,
            antColony.GetOptimalDistanceFromBenchmarkFile(),
            "Solution.txt",
            "BenchmarkData/A-n39-k5.txt"
        );

        statistics.AddUsedParameters(alpha, beta, q0, tau, Q, ants, iterations);

        for (int j = 0; j < numberOfProgramIterations; ++j)
        {
            try
            {
                var timeWatch = new Stopwatch();

                timeWatch.Start();
                var solution = antColony.StartSolvingProblemInSeries();
                //var solution = antColony.StartSolvingProblemParallel();
                timeWatch.Stop();

                if (
                    solution.GetGiantTourDistance()
                    < antColony.GetOptimalDistanceFromBenchmarkFile() + 2
                )
                {
                    Console.WriteLine("Znaleziono optymalne rozwiązanie!!!!!!");
                }
                else
                {
                    Console.WriteLine("Znaleziono rozwiązanie!");
                }

                statistics.AddAndPresentNewFoundSolution(timeWatch.Elapsed, solution);
                antColony.ResetColonyForNextRun();
            }
            catch
            {
                Console.WriteLine("Złapano wyjątek w trakcie rozwiązywania problemu!");
            }
        }
    }
}
