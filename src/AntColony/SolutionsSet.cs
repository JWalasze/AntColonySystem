namespace AntColonyNamespace
{
    internal class SolutionSet
    {
        private GiantTourSolution? BestFoundSolution;

        private GiantTourSolution? SecondBestFoundSolution;

        private GiantTourSolution? ThirdBestFoundSolution;

        private List<GiantTourSolution> _SolutionSet;

        public SolutionSet()
        {
            this._SolutionSet = new List<GiantTourSolution>(3);
        }

        public void AddSolutionIfNeeded(GiantTourSolution newSolution)
        {
            this._SolutionSet.Add(newSolution);
            this._SolutionSet.Sort();
            if (this._SolutionSet.Count > 3)
            {
                this._SolutionSet.Remove(this._SolutionSet[^1]);
            }
        }

        public override string ToString() { }

        public void ResetSolutionSet()
        {
            this.BestFoundSolution = null;
            this.SecondBestFoundSolution = null;
            this.ThirdBestFoundSolution = null;
        }
    }
}
