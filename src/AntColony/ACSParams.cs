namespace AntColonyNamespace
{
    internal class AntParams
    {
        public double _alpha { get; private set; }
        public double _beta { get; private set; }
        public double _q0 { get; private set; }
        public double _tau { get; private set; }
        public double _eta { get; private set; }
        public double _Q { get; private set; }
        public int _stagnation { get; private set; }
        public int _ants { get; private set; }
        public int _iterations { get; private set; }

        public AntParams(
            double alpha,
            double beta,
            double q0,
            double tau,
            double eta,
            double Q,
            int stagnation,
            int ants,
            int iterations
        )
        {
            this._alpha = alpha;
            this._beta = beta;
            this._q0 = q0;
            this._tau = tau;
            this._eta = eta;
            this._Q = Q;
            this._stagnation = stagnation;
            this._ants = ants;
            this._iterations = iterations;
        }
    }
}
