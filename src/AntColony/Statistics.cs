namespace AntColonyNamespace
{
    internal class Statistics
    {
        private readonly double OptimalSolutionDistance;

        private string SoutionFileName;

        private string BenchmarkFileName;

        private int NumberOfSolutionsInSeries;

        private List<double> FoundDistancesInSeries;

        public Statistics(
            int numberOfSOlutionsInSeries,
            double optimalSolutionDistance,
            string solutionFileName,
            string benchmarkFileName
        )
        {
            this.FoundDistancesInSeries = new List<double>(NumberOfSolutionsInSeries);
            this.NumberOfSolutionsInSeries = numberOfSOlutionsInSeries;
            this.SoutionFileName =
                "C:\\Users\\Kuba Walaszek\\Desktop\\.NET_App\\Solutions\\" + solutionFileName;
            this.OptimalSolutionDistance = optimalSolutionDistance;
            this.BenchmarkFileName = benchmarkFileName;
        }

        public void AddUsedParameters(
            double alpha,
            double beta,
            double q0,
            double tau,
            double eta,
            double Q,
            int ants,
            int iterations
        )
        {
            File.AppendAllText(
                this.SoutionFileName,
                "---------------------||---------------------"
                    + Environment.NewLine
                    + "Problem z pliku: "
                    + this.BenchmarkFileName
                    + Environment.NewLine
                    + "Współczynniki: "
                    + Environment.NewLine
                    + "Alpha: "
                    + alpha
                    + Environment.NewLine
                    + "Beta: "
                    + beta
                    + Environment.NewLine
                    + "Q0: "
                    + q0
                    + Environment.NewLine
                    + "Tau: "
                    + tau
                    + Environment.NewLine
                    + "Eta: "
                    + eta
                    + Environment.NewLine
                    + "Q: "
                    + Q
                    + Environment.NewLine
                    + "Ants: "
                    + ants
                    + Environment.NewLine
                    + "Iterations: "
                    + iterations
                    + Environment.NewLine
                    + Environment.NewLine
            );
        }

        public void AddAndPresentNewFoundSolution(
            TimeSpan timeSpan,
            GiantTourSolution giantTourSolution
        )
        {
            this.FoundDistancesInSeries.Add(giantTourSolution.GetGiantTourDistance());

            File.AppendAllText(
                this.SoutionFileName,
                "-------------------BEST SOLUTION------------------"
                    + Environment.NewLine
                    + "Najlepsza znaleziona trasa: "
                    + Math.Round(this.FoundDistancesInSeries.Last(), 3)
                    + Environment.NewLine
                    + "Czas trwania: "
                    + timeSpan.Minutes
                    + ":"
                    + timeSpan.Seconds
                    + ":"
                    + timeSpan.Milliseconds
                    + Environment.NewLine
                    + "Błąd względny dla optymalnego rozwiązania: "
                    + 100
                        * (this.FoundDistancesInSeries.Last() - this.OptimalSolutionDistance)
                        / this.OptimalSolutionDistance
                    + Environment.NewLine
                    + "Prezentacja rozwiązania (marszruty): "
                    + Environment.NewLine
                    + giantTourSolution.GetItineraryAllApart()
                    + Environment.NewLine
            );

            if (this.NumberOfSolutionsInSeries == this.FoundDistancesInSeries.Count)
            {
                File.AppendAllText(
                    this.SoutionFileName,
                    "Średnia z serii: "
                        + this.FoundDistancesInSeries.Average()
                        + Environment.NewLine
                        + Environment.NewLine
                        + Environment.NewLine
                );

                this.FoundDistancesInSeries.Clear();
            }
        }
    }
}
