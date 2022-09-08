using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    internal class Possibilities
    {
        private double _BETA;

        private double _Denominator;

        private Dictionary<Edge, double> _Nominators;

        private Dictionary<Edge, double> _Probabilities;

        public Possibilities(double BETA)
        {
            this._Denominator = 0;
            this._Nominators = new Dictionary<Edge, double>();
            this._Probabilities = new Dictionary<Edge, double>();
            this._BETA = BETA;
        }

        public void CountNominatorAndUpdateDenominator(Edge pickedEdge)
        {
            double countedValue =
                pickedEdge.PheromoneLevel * Math.Pow(1 / pickedEdge.Distance, this._BETA);
            this._Nominators.Add(pickedEdge, countedValue);
            this._Denominator += countedValue;
        }

        public void CountProbabilities()
        {
            foreach (var nomiantor in this._Nominators)
            {
                this._Probabilities.Add(
                    nomiantor.Key,
                    this._Nominators[nomiantor.Key] / this._Denominator
                );
            }
        }

        public KeyValuePair<Edge, double> GetMaxNominator()
        {
            if (this._Nominators.Count == 0)
            {
                throw new Exception("Nominators are empty!!!");
            }

            var maxNominator = this._Nominators.First();
            foreach (var nominator in this._Nominators)
            {
                if (nominator.Value > maxNominator.Value)
                {
                    maxNominator = nominator;
                }
            }

            return maxNominator;
        }

        public ReadOnlyDictionary<Edge, double> GetProbabilities()
        {
            if (this._Probabilities.Count == 0)
            {
                throw new Exception("Probabilities are empty!!!");
            }

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
