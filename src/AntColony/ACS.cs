using System.Diagnostics;

namespace AntColonyNamespace
{
    internal class ACS
    {
        private string _benchmarkFileName;

        private int _numberOfProgramIterations;

        private int _numberOfThreads;

        private string _generalStatisticSolutionFileName;

        private string _pythonSolutionFileName;

        private AntParams _params;

        public ACS(
            string benchmarkFileName,
            int numberOfProgramIterations,
            int numberOfThreads,
            string statisticSolution,
            string pythonSolution,
            AntParams colonyParams
        )
        {
            this._benchmarkFileName = benchmarkFileName;
            this._numberOfProgramIterations = numberOfProgramIterations;
            this._pythonSolutionFileName = pythonSolution;
            this._generalStatisticSolutionFileName = statisticSolution;
            this._numberOfThreads = numberOfThreads;
            this._params = colonyParams;
        }

        public void SolveCVRP()
        {
            var antColony = new AntColony(
                this._params._alpha,
                this._params._beta,
                this._params._q0,
                this._params._tau,
                this._params._eta,
                this._params._Q,
                this._params._stagnation,
                this._params._ants,
                this._params._iterations,
                this._numberOfThreads,
                this._benchmarkFileName
            );

            var statistics = new Statistics(
                this._numberOfProgramIterations,
                antColony.GetOptimalDistanceFromBenchmarkFile(),
                this._generalStatisticSolutionFileName,
                this._benchmarkFileName
            );
            statistics.AddUsedParameters(
                this._params._alpha,
                this._params._beta,
                this._params._q0,
                this._params._tau,
                this._params._eta,
                this._params._Q,
                this._params._stagnation,
                this._params._ants,
                this._params._iterations
            );

            var solutionStatistics = new SolutionStatistics(
                _pythonSolutionFileName,
                _benchmarkFileName,
                _params,
                antColony.GetOptimalDistanceFromBenchmarkFile()
            );

            for (int j = 0; j < _numberOfProgramIterations; ++j)
            {
                try
                {
                    var timeWatch = new Stopwatch();
                    GiantTourSolution solution;

                    if (this._numberOfThreads == 1)
                    {
                        timeWatch.Start();
                        solution = antColony.StartSolvingProblemInSeries();
                        timeWatch.Stop();
                    }
                    else if (this._numberOfThreads > 1)
                    {
                        timeWatch.Start();
                        solution = antColony.StartSolvingProblemParallel();
                        timeWatch.Stop();
                    }
                    else
                    {
                        throw new Exception("Niepoprawna ilość wątków!");
                    }

                    statistics.AddAndPresentNewFoundSolution(timeWatch.Elapsed, solution);

                    solutionStatistics.AddSolution(
                        solution.GetGiantTourDistance(),
                        timeWatch.Elapsed
                    );

                    antColony.ResetColonyForNextRun();
                }
                catch
                {
                    Console.WriteLine("Złapano wyjątek w trakcie rozwiązywania problemu!");
                }
            }
            solutionStatistics.PerformMeasurements();
        }
    }
}
