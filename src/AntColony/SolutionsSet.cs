namespace AntColonyNamespace
{
    internal class SolutionSet
    {
        private List<GiantTourSolution> _SolutionSet;

        public SolutionSet()
        {
            this._SolutionSet = new List<GiantTourSolution>(3);
        }

        public void AddSolutionIfNeeded(GiantTourSolution newSolution)
        {
            this._SolutionSet.Add(newSolution);
            this._SolutionSet.Sort();
            this._SolutionSet.Reverse();
            if (this._SolutionSet.Count > 3)
            {
                this._SolutionSet.Remove(this._SolutionSet[^1]);
            }
        }

        public double GetBestSolutionDistance()
        {
            return this._SolutionSet[0].GetGiantTourDistance();
        }

        public double GetSecondBestSolutionDistance()
        {
            return this._SolutionSet[1].GetGiantTourDistance();
        }

        public double GetThirdBestSolutionDistance()
        {
            return this._SolutionSet[2].GetGiantTourDistance();
        }

        public bool IsEdgeInBestSolutionSet(Edge checkedEdge)
        {
            return this._SolutionSet[0].IsEdgeInSolution(checkedEdge);
        }

        public bool CheckReferenceWithBestSolution(GiantTourSolution solutionToCheck)
        {
            return ReferenceEquals(solutionToCheck, this._SolutionSet[0]);
        }

        public override string ToString()
        {
            var str = string.Empty;
            this._SolutionSet.ForEach(solution =>
            {
                str += "Rozwiązanie: ";
                str += solution.GetGiantTourDistance();
            });

            return str;
        }

        public void ResetSolutionSet()
        {
            this._SolutionSet.Clear();
        }
    }
}
