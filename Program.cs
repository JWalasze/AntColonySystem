using AntColonyNamespace;
using System.Diagnostics;

var coefAlpha = new List<double> { 0.2, 0.5, 0.7, 0.8, 0.9, 1, 2, 5 };
var coefBeta = new List<double> { 0.1, 0.5, 1, 1.5, 2, 3, 10, 30 };
var coefTau = new List<double> { 0.005, 0.01, 0.05, 0.1, 0.2, 0.3, 0.5 };
var coefEta = new List<double> { 0.1, 0.2, 0.5, 0.75, 0.9 };
var coefQ = new List<double> { 1, 100, 1000, 10000, 50000, 100000, 200000, 500000 };
var coefQ0 = new List<double> { 0.2, 0.4, 0.5, 0.7, 0.75, 0.8, 0.9, 0.95 };
var coefAnts = new List<double> { 5, 10, 20, 30, 40 };
var coefIterat = new List<double> { 5, 50, 100, 500, 1000, 5000, 6000, 7000 };

var numberOfProgramIterations = 5;
var numberOfThreads = 3;
var statisticSolution = "Solution.txt";
var pythonSolution = "SolutionP.txt";
var benchmarkFileName = "A-n39-k5.txt";

string _alpha = "Alpha";
string _beta = "Beta";
string _q0 = "q0";
string _tau = "Tau";
string _eta = "Eta";
string _Q = "Q";
string _ants = "Ants";
string _iterations = "Iterations";

var listOfCoefs = new Dictionary<string, List<double>>();
listOfCoefs.Add(_alpha, coefAlpha);
listOfCoefs.Add(_beta, coefBeta);
listOfCoefs.Add(_q0, coefQ0);
listOfCoefs.Add(_tau, coefTau);
listOfCoefs.Add(_eta, coefEta);
listOfCoefs.Add(_Q, coefQ);
listOfCoefs.Add(_ants, coefAnts);
listOfCoefs.Add(_iterations, coefIterat);

foreach (var coef in coefQ0)
{
    double alpha = 2;
    double beta = 2;
    double q0 = 0.5;
    double tau = 0.2;
    double eta = 0.2;
    double Q = 1;
    int ants = 20;
    int iterations = 4000;

    if (numberOfThreads > ants)
    {
        throw new Exception("Niepoprawna ilość wątków!");
    }

    AntColony antColony = new AntColony(
        alpha,
        beta,
        q0,
        tau,
        eta,
        Q,
        ants,
        iterations,
        numberOfThreads,
        benchmarkFileName
    );

    var statistics = new Statistics(
        numberOfProgramIterations,
        antColony.GetOptimalDistanceFromBenchmarkFile(),
        statisticSolution,
        benchmarkFileName
    );

    var pythonStatistics = new PythonStatistics(pythonSolution, coef, _alpha);

    statistics.AddUsedParameters(alpha, beta, q0, tau, eta, Q, ants, iterations);

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
            pythonStatistics.AddDataToSolution(solution.GetGiantTourDistance());

            antColony.ResetColonyForNextRun();
        }
        catch
        {
            Console.WriteLine("Złapano wyjątek w trakcie rozwiązywania problemu!");
        }
    }
    pythonStatistics.CountAverage();
}
