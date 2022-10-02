using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntColonyNamespace
{
    //Klasa liczaca prawdopodobienstwa
    internal class Possibilities
    {
        //Wspolczynnik z AntColony
        private double _ALFA;

        //Wspolczynnik z AntColony
        private double _BETA;

        //Mianownik
        private double _Denominator;

        //Lista licznikow
        private Dictionary<EdgeWithDestinationCity, double> _Nominators;

        //Lista obliczonych prawdopodobienstw na podstawie licznikow i mianownika
        private Dictionary<EdgeWithDestinationCity, double> _Probabilities;

        //Konstruktor
        public Possibilities(double ALFA, double BETA)
        {
            this._Denominator = 0;
            this._Nominators = new Dictionary<EdgeWithDestinationCity, double>();
            this._Probabilities = new Dictionary<EdgeWithDestinationCity, double>();

            this._ALFA = ALFA;
            this._BETA = BETA;
        }

        //Metoda do zliczania licznika i aktualizacji mianownika
        public void CountNominatorAndUpdateDenominator(EdgeWithDestinationCity pickedPath)
        {
            double countedValue =
                Math.Pow(pickedPath.EdgeToDestCity.PheromoneLevel, this._ALFA)
                * Math.Pow(1 / pickedPath.EdgeToDestCity.Distance, this._BETA);
            this._Nominators.Add(pickedPath, countedValue);
            this._Denominator += countedValue;
        }

        //Metoda do ostatecznego zliczenia prawdopodobienstw
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

        //Zwraca niemodyfikowalny maksymalna wartosc licznika
        public KeyValuePair<EdgeWithDestinationCity, double> GetMaxNominator()
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

        //Zwraca zliczone prawdopodobienstwa
        public ReadOnlyDictionary<EdgeWithDestinationCity, double> GetProbabilities()
        {
            if (this._Probabilities.Count == 0)
            {
                throw new Exception("Probabilities are empty!!!");
            }

            return new ReadOnlyDictionary<EdgeWithDestinationCity, double>(this._Probabilities);
        }

        //Sprawdzamy czy zostaly dobrze zliczone prawdopodoienstwa
        public bool CheckIfProbabilitiesAreEmpty()
        {
            return this._Probabilities.Count == 0;
        }

        //Sprawdzamy czy zostaly dobrze zliczone liczniki
        public bool CheckIfNominatorsAreEmpty()
        {
            return this._Nominators.Count == 0;
        }

        //Resetujemy wartosci obiektu do ponownego uzytku
        public void RestartAllValues()
        {
            this._Denominator = 0;
            this._Nominators.Clear();
            this._Probabilities.Clear();
        }
    }
}
