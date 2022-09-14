namespace AntColonyNamespace
{
    internal class City : Vertex
    {
        private readonly double _Demand;

        public double Demand
        {
            get { return this._Demand; }
        }

        public City(double demand)
        {
            this._Demand = demand;
        }
    }
}
