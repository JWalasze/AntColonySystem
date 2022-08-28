using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class Possibilities
    {
        private double _Denominator;

        private double _ALFA;

        private double _BETA;

        private Dictionary<Edge, double> _Nominators;

        private Dictionary<Edge, double> _Probabilities;

        public Possibilities(double ALFA, double BETA)
        {
            this._Denominator = 0;
            this._Nominators = new Dictionary<Edge, double>();
            this._Probabilities = new Dictionary<Edge, double>();
            this._ALFA = ALFA;
            this._BETA = BETA;
        }

        public void CountNominatorAndUpdateDenominator(Edge edge)
        {
            double value =
                Math.Pow(edge.PheromoneLevel, this._ALFA) * Math.Pow(1 / edge.Distance, this._BETA);
            this._Nominators.Add(edge, value);
            this._Denominator += value;
        }

        public void CountProbabilities()
        {
            foreach (var edge in this._Nominators)
            {
                this._Probabilities.Add(edge.Key, this._Nominators[edge.Key] / this._Denominator);
            }
        }

        public KeyValuePair<Edge, double> GetMaxNominator()
        {
            return this._Nominators.Max();
        }

        public ReadOnlyDictionary<Edge, double> GetProbabilities()
        {
            return new ReadOnlyDictionary<Edge, double>(this._Probabilities);
        }

        public void RestartAllValues()
        {
            this._Denominator = 0;
            this._Nominators.Clear();
            this._Probabilities.Clear();
        }
    }
}
