namespace AntColonyNamespace
{
    internal class SolutionStatistics
    {
        private string PythonSolutionPath;

        private double Parameter;

        private string ParameterName;

        private List<double> FoundDistancesInSeries;

        public SolutionStatistics(string pythonSolutionPath, double parameter, string parameterName)
        {
            this.PythonSolutionPath =
                "C:\\Users\\Kuba Walaszek\\Desktop\\.NET_App\\Solutions\\" + pythonSolutionPath;
            this.Parameter = parameter;
            this.ParameterName = parameterName;

            this.FoundDistancesInSeries = new List<double>();
        }

        public void ChangeExaminedParameter(double newParameter, string newParameterName)
        {
            this.Parameter = newParameter;
            this.ParameterName = newParameterName;
        }

        public void AddDataToSolution(double newObjectiveFunction)
        {
            this.FoundDistancesInSeries.Add(newObjectiveFunction);
        }

        public void CountAverage()
        {
            File.AppendAllText(
                this.PythonSolutionPath,
                this.ParameterName
                    + " "
                    + this.Parameter
                    + " "
                    + this.FoundDistancesInSeries.Average()
                    + Environment.NewLine
            );
        }
    }
}
