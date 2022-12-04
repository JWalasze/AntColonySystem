namespace AntColonyNamespace
{
    internal class SolutionStatistics
    {
        private string _ImportantSolutionFileName;

        private string _BenchmarkFileName;

        private AntParams _AntParams;

        private double _OptimalSolutionDistance;

        private double _BestFoundDistance;

        private double _AverageFoundDistance;

        private double _AverageTimeElapsed;

        private double _WorstFoundDistance;

        private List<double> _ListOfFoundDistances;

        private List<System.TimeSpan> _ListOfTimeElapsed;

        public SolutionStatistics(
            string pythonSolutionPath,
            string benchmarkFileName,
            AntParams antParams,
            double optimalSOlutionDistance
        )
        {
            this._ImportantSolutionFileName =
                "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/Solutions/"
                + pythonSolutionPath;
            this._BenchmarkFileName = benchmarkFileName;

            this._ListOfFoundDistances = new List<double>();
            this._ListOfTimeElapsed = new List<TimeSpan>();

            this._BestFoundDistance = 0;
            this._AverageFoundDistance = 0;
            this._AverageTimeElapsed = 0;
            this._WorstFoundDistance = 0;

            this._OptimalSolutionDistance = optimalSOlutionDistance;

            this._AntParams = antParams;
        }

        public void AddSolution(double solutionDistance, System.TimeSpan timeElapsed)
        {
            this._ListOfFoundDistances.Add(solutionDistance);
            this._ListOfTimeElapsed.Add(timeElapsed);

            if (_BestFoundDistance == 0 || solutionDistance < _BestFoundDistance)
            {
                _BestFoundDistance = solutionDistance;
            }

            if (_WorstFoundDistance == 0 || solutionDistance > _WorstFoundDistance)
            {
                _WorstFoundDistance = solutionDistance;
            }
        }

        private void CountAverageTime()
        {
            var averageValue = this._ListOfTimeElapsed.Average(
                time =>
                    time.Minutes * 60 + time.Seconds + Convert.ToDouble(time.Milliseconds) / 1000
            );

            this._AverageTimeElapsed = averageValue;
        }

        private void CountAverageSolutionsDistances()
        {
            this._AverageFoundDistance = this._ListOfFoundDistances.Average();
        }

        private double CountRelativeError(double solutionDistances)
        {
            return 100
                * (solutionDistances - this._OptimalSolutionDistance)
                / this._OptimalSolutionDistance;
        }

        public void PerformMeasurements()
        {
            this.CountAverageSolutionsDistances();
            this.CountAverageTime();

            File.AppendAllText(
                this._ImportantSolutionFileName,
                this._BenchmarkFileName
                    + " | "
                    + this._BestFoundDistance
                    + "/"
                    + this.CountRelativeError(this._BestFoundDistance)
                    + " | "
                    + this._AverageFoundDistance
                    + "/"
                    + this.CountRelativeError(this._AverageFoundDistance)
                    + " | "
                    + this._WorstFoundDistance
                    + "/"
                    + this.CountRelativeError(this._WorstFoundDistance)
                    + " | "
                    + this._AverageTimeElapsed
                    + Environment.NewLine
                    + Environment.NewLine
            );
        }
    }
}
