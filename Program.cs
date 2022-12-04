using AntColonyNamespace;

var benchamrkFIles = new List<string>()
{
    "A-n32-k5.txt",
    "A-n39-k5.txt",
    "A-n45-k5.txt",
    // "A-n48-k7.txt",
    // "B-n52-k7.txt",
    // "A-n48-k7.txt",
    // "A-n48-k7.txt",
    // "A-n48-k7.txt",
    // "A-n48-k7.txt",
    // "A-n48-k7.txt",
    // "A-n48-k7.txt",
    // "A-n48-k7.txt"
};

var coefsStruct = new AntParams(0.5, 2, 0.4, 0.01, 0.01, 1, 300, 30, 2000);

foreach (var benchmarkFile in benchamrkFIles)
{
    var ACSForCVRP = new ACS(benchmarkFile, 3, 4, "Solution.txt", "SolutionP.txt", coefsStruct);
    ACSForCVRP.SolveCVRP();
}

// foreach (var coef in coefBeta)
// {
//     double alpha = 0.7;
//     double beta = coef;
//     double q0 = 0.4;
//     double tau = 0.01;
//     double eta = 0.01;
//     double Q = 1;
//     int stagnation = 600;
//     int ants = 20;
//     int iterations = 2000;

//     if (numberOfThreads > ants)
//     {
//         throw new Exception("Niepoprawna ilość wątków!");
//     }

//     AntColony antColony = new AntColony(
//         alpha,
//         beta,
//         q0,
//         tau,
//         eta,
//         Q,
//         stagnation,
//         ants,
//         iterations,
//         numberOfThreads,
//         benchmarkFileName
//     );

//     var statistics = new Statistics(
//         numberOfProgramIterations,
//         antColony.GetOptimalDistanceFromBenchmarkFile(),
//         statisticSolution,
//         benchmarkFileName
//     );

//     var pythonStatistics = new PythonStatistics(pythonSolution, coef, "Beta");

//     statistics.AddUsedParameters(alpha, beta, q0, tau, eta, Q, ants, iterations);

//     for (int j = 0; j < numberOfProgramIterations; ++j)
//     {
//         try
//         {
//             var timeWatch = new Stopwatch();

//             timeWatch.Start();
//             //var solution = antColony.StartSolvingProblemInSeries();
//             var solution = antColony.StartSolvingProblemParallel();
//             timeWatch.Stop();

//             statistics.AddAndPresentNewFoundSolution(timeWatch.Elapsed, solution);
//             pythonStatistics.AddDataToSolution(solution.GetGiantTourDistance());

//             antColony.ResetColonyForNextRun();
//         }
//         catch
//         {
//             Console.WriteLine("Złapano wyjątek w trakcie rozwiązywania problemu!");
//         }
//     }
//     pythonStatistics.CountAverage();
// }

// foreach (var coef in coefQ0)
// {
//     double alpha = 0.7;
//     double beta = 2;
//     double q0 = coef;
//     double tau = 0.01;
//     double eta = 0.01;
//     double Q = 1;
//     int stagnation = 600;
//     int ants = 20;
//     int iterations = 2000;

//     if (numberOfThreads > ants)
//     {
//         throw new Exception("Niepoprawna ilość wątków!");
//     }

//     AntColony antColony = new AntColony(
//         alpha,
//         beta,
//         q0,
//         tau,
//         eta,
//         Q,
//         stagnation,
//         ants,
//         iterations,
//         numberOfThreads,
//         benchmarkFileName
//     );

//     var statistics = new Statistics(
//         numberOfProgramIterations,
//         antColony.GetOptimalDistanceFromBenchmarkFile(),
//         statisticSolution,
//         benchmarkFileName
//     );

//     var pythonStatistics = new PythonStatistics(pythonSolution, coef, "q0");

//     statistics.AddUsedParameters(alpha, beta, q0, tau, eta, Q, ants, iterations);

//     for (int j = 0; j < numberOfProgramIterations; ++j)
//     {
//         try
//         {
//             var timeWatch = new Stopwatch();

//             timeWatch.Start();
//             //var solution = antColony.StartSolvingProblemInSeries();
//             var solution = antColony.StartSolvingProblemParallel();
//             timeWatch.Stop();

//             statistics.AddAndPresentNewFoundSolution(timeWatch.Elapsed, solution);
//             pythonStatistics.AddDataToSolution(solution.GetGiantTourDistance());

//             antColony.ResetColonyForNextRun();
//         }
//         catch
//         {
//             Console.WriteLine("Złapano wyjątek w trakcie rozwiązywania problemu!");
//         }
//     }
//     pythonStatistics.CountAverage();
// }

// foreach (var coef in coefTau)
// {
//     double alpha = 0.7;
//     double beta = 2;
//     double q0 = 0.4;
//     double tau = coef;
//     double eta = 0.01;
//     double Q = 1;
//     int stagnation = 600;
//     int ants = 20;
//     int iterations = 2000;

//     if (numberOfThreads > ants)
//     {
//         throw new Exception("Niepoprawna ilość wątków!");
//     }

//     AntColony antColony = new AntColony(
//         alpha,
//         beta,
//         q0,
//         tau,
//         eta,
//         Q,
//         stagnation,
//         ants,
//         iterations,
//         numberOfThreads,
//         benchmarkFileName
//     );

//     var statistics = new Statistics(
//         numberOfProgramIterations,
//         antColony.GetOptimalDistanceFromBenchmarkFile(),
//         statisticSolution,
//         benchmarkFileName
//     );

//     var pythonStatistics = new PythonStatistics(pythonSolution, coef, "Tau");

//     statistics.AddUsedParameters(alpha, beta, q0, tau, eta, Q, ants, iterations);

//     for (int j = 0; j < numberOfProgramIterations; ++j)
//     {
//         try
//         {
//             var timeWatch = new Stopwatch();

//             timeWatch.Start();
//             //var solution = antColony.StartSolvingProblemInSeries();
//             var solution = antColony.StartSolvingProblemParallel();
//             timeWatch.Stop();

//             statistics.AddAndPresentNewFoundSolution(timeWatch.Elapsed, solution);
//             pythonStatistics.AddDataToSolution(solution.GetGiantTourDistance());

//             antColony.ResetColonyForNextRun();
//         }
//         catch
//         {
//             Console.WriteLine("Złapano wyjątek w trakcie rozwiązywania problemu!");
//         }
//     }
//     pythonStatistics.CountAverage();
// }

// foreach (var coef in coefEta)
// {
//     double alpha = 0.7;
//     double beta = 2;
//     double q0 = 0.4;
//     double tau = 0.01;
//     double eta = coef;
//     double Q = 1;
//     int stagnation = 600;
//     int ants = 20;
//     int iterations = 2000;

//     if (numberOfThreads > ants)
//     {
//         throw new Exception("Niepoprawna ilość wątków!");
//     }

//     AntColony antColony = new AntColony(
//         alpha,
//         beta,
//         q0,
//         tau,
//         eta,
//         Q,
//         stagnation,
//         ants,
//         iterations,
//         numberOfThreads,
//         benchmarkFileName
//     );

//     var statistics = new Statistics(
//         numberOfProgramIterations,
//         antColony.GetOptimalDistanceFromBenchmarkFile(),
//         statisticSolution,
//         benchmarkFileName
//     );

//     var pythonStatistics = new PythonStatistics(pythonSolution, coef, "Eta");

//     statistics.AddUsedParameters(alpha, beta, q0, tau, eta, Q, ants, iterations);

//     for (int j = 0; j < numberOfProgramIterations; ++j)
//     {
//         try
//         {
//             var timeWatch = new Stopwatch();

//             timeWatch.Start();
//             //var solution = antColony.StartSolvingProblemInSeries();
//             var solution = antColony.StartSolvingProblemParallel();
//             timeWatch.Stop();

//             statistics.AddAndPresentNewFoundSolution(timeWatch.Elapsed, solution);
//             pythonStatistics.AddDataToSolution(solution.GetGiantTourDistance());

//             antColony.ResetColonyForNextRun();
//         }
//         catch
//         {
//             Console.WriteLine("Złapano wyjątek w trakcie rozwiązywania problemu!");
//         }
//     }
//     pythonStatistics.CountAverage();
// }

// foreach (var coef in coefQ)
// {
//     double alpha = 0.7;
//     double beta = 2;
//     double q0 = 0.4;
//     double tau = 0.01;
//     double eta = 0.01;
//     double Q = coef;
//     int stagnation = 600;
//     int ants = 20;
//     int iterations = 2000;

//     if (numberOfThreads > ants)
//     {
//         throw new Exception("Niepoprawna ilość wątków!");
//     }

//     AntColony antColony = new AntColony(
//         alpha,
//         beta,
//         q0,
//         tau,
//         eta,
//         Q,
//         stagnation,
//         ants,
//         iterations,
//         numberOfThreads,
//         benchmarkFileName
//     );

//     var statistics = new Statistics(
//         numberOfProgramIterations,
//         antColony.GetOptimalDistanceFromBenchmarkFile(),
//         statisticSolution,
//         benchmarkFileName
//     );

//     var pythonStatistics = new PythonStatistics(pythonSolution, coef, "Q");

//     statistics.AddUsedParameters(alpha, beta, q0, tau, eta, Q, ants, iterations);

//     for (int j = 0; j < numberOfProgramIterations; ++j)
//     {
//         try
//         {
//             var timeWatch = new Stopwatch();

//             timeWatch.Start();
//             //var solution = antColony.StartSolvingProblemInSeries();
//             var solution = antColony.StartSolvingProblemParallel();
//             timeWatch.Stop();

//             statistics.AddAndPresentNewFoundSolution(timeWatch.Elapsed, solution);
//             pythonStatistics.AddDataToSolution(solution.GetGiantTourDistance());

//             antColony.ResetColonyForNextRun();
//         }
//         catch
//         {
//             Console.WriteLine("Złapano wyjątek w trakcie rozwiązywania problemu!");
//         }
//     }
//     pythonStatistics.CountAverage();
// }

// foreach (var coef in coefStagnation)
// {
//     double alpha = 0.7;
//     double beta = 2;
//     double q0 = 0.4;
//     double tau = 0.01;
//     double eta = 0.01;
//     double Q = 1;
//     int stagnation = coef;
//     int ants = 20;
//     int iterations = 2000;

//     if (numberOfThreads > ants)
//     {
//         throw new Exception("Niepoprawna ilość wątków!");
//     }

//     AntColony antColony = new AntColony(
//         alpha,
//         beta,
//         q0,
//         tau,
//         eta,
//         Q,
//         stagnation,
//         ants,
//         iterations,
//         numberOfThreads,
//         benchmarkFileName
//     );

//     var statistics = new Statistics(
//         numberOfProgramIterations,
//         antColony.GetOptimalDistanceFromBenchmarkFile(),
//         statisticSolution,
//         benchmarkFileName
//     );

//     var pythonStatistics = new PythonStatistics(pythonSolution, coef, "Stagnation");

//     statistics.AddUsedParameters(alpha, beta, q0, tau, eta, Q, ants, iterations);

//     for (int j = 0; j < numberOfProgramIterations; ++j)
//     {
//         try
//         {
//             var timeWatch = new Stopwatch();

//             timeWatch.Start();
//             //var solution = antColony.StartSolvingProblemInSeries();
//             var solution = antColony.StartSolvingProblemParallel();
//             timeWatch.Stop();

//             statistics.AddAndPresentNewFoundSolution(timeWatch.Elapsed, solution);
//             pythonStatistics.AddDataToSolution(solution.GetGiantTourDistance());

//             antColony.ResetColonyForNextRun();
//         }
//         catch
//         {
//             Console.WriteLine("Złapano wyjątek w trakcie rozwiązywania problemu!");
//         }
//     }
//     pythonStatistics.CountAverage();
// }

// foreach (var coef in coefAnts)
// {
//     double alpha = 0.7;
//     double beta = 2;
//     double q0 = 0.4;
//     double tau = 0.01;
//     double eta = 0.01;
//     double Q = 1;
//     int stagnation = 600;
//     int ants = coef;
//     int iterations = 2000;

//     if (numberOfThreads > ants)
//     {
//         throw new Exception("Niepoprawna ilość wątków!");
//     }

//     AntColony antColony = new AntColony(
//         alpha,
//         beta,
//         q0,
//         tau,
//         eta,
//         Q,
//         stagnation,
//         ants,
//         iterations,
//         numberOfThreads,
//         benchmarkFileName
//     );

//     var statistics = new Statistics(
//         numberOfProgramIterations,
//         antColony.GetOptimalDistanceFromBenchmarkFile(),
//         statisticSolution,
//         benchmarkFileName
//     );

//     var pythonStatistics = new PythonStatistics(pythonSolution, coef, "Ants");

//     statistics.AddUsedParameters(alpha, beta, q0, tau, eta, Q, ants, iterations);

//     for (int j = 0; j < numberOfProgramIterations; ++j)
//     {
//         try
//         {
//             var timeWatch = new Stopwatch();

//             timeWatch.Start();
//             //var solution = antColony.StartSolvingProblemInSeries();
//             var solution = antColony.StartSolvingProblemParallel();
//             timeWatch.Stop();

//             statistics.AddAndPresentNewFoundSolution(timeWatch.Elapsed, solution);
//             pythonStatistics.AddDataToSolution(solution.GetGiantTourDistance());

//             antColony.ResetColonyForNextRun();
//         }
//         catch
//         {
//             Console.WriteLine("Złapano wyjątek w trakcie rozwiązywania problemu!");
//         }
//     }
//     pythonStatistics.CountAverage();
// }

// foreach (var coef in coefIterat)
// {
//     double alpha = 0.7;
//     double beta = 2;
//     double q0 = 0.4;
//     double tau = 0.01;
//     double eta = 0.01;
//     double Q = 1;
//     int stagnation = 600;
//     int ants = 20;
//     int iterations = coef;

//     if (numberOfThreads > ants)
//     {
//         throw new Exception("Niepoprawna ilość wątków!");
//     }

//     AntColony antColony = new AntColony(
//         alpha,
//         beta,
//         q0,
//         tau,
//         eta,
//         Q,
//         stagnation,
//         ants,
//         iterations,
//         numberOfThreads,
//         benchmarkFileName
//     );

//     var statistics = new Statistics(
//         numberOfProgramIterations,
//         antColony.GetOptimalDistanceFromBenchmarkFile(),
//         statisticSolution,
//         benchmarkFileName
//     );

//     var pythonStatistics = new PythonStatistics(pythonSolution, coef, "Iterations");

//     statistics.AddUsedParameters(alpha, beta, q0, tau, eta, Q, ants, iterations);

//     for (int j = 0; j < numberOfProgramIterations; ++j)
//     {
//         try
//         {
//             var timeWatch = new Stopwatch();

//             timeWatch.Start();
//             //var solution = antColony.StartSolvingProblemInSeries();
//             var solution = antColony.StartSolvingProblemParallel();
//             timeWatch.Stop();

//             statistics.AddAndPresentNewFoundSolution(timeWatch.Elapsed, solution);
//             pythonStatistics.AddDataToSolution(solution.GetGiantTourDistance());

//             antColony.ResetColonyForNextRun();
//         }
//         catch
//         {
//             Console.WriteLine("Złapano wyjątek w trakcie rozwiązywania problemu!");
//         }
//     }
//     pythonStatistics.CountAverage();
// }
