namespace AntColonyNamespace
{
    //Klasa miasta pomiędzy którymi podróżują ciężarówki
    internal class City
    {
        //Indeks miasta
        public double Index { get; protected set; }

        //Zapotrzebowanie klienta w tej lokalizacji
        public double Demand { get; protected set; }

        //Szerokość geograficzna
        public double Latitude { get; protected set; }

        //Długość geograficzna
        public double Longitude { get; protected set; }

        /*Konstruktor - współrzędne geograficzne będą wykorzystane do obliczenia dystansów
        pomiędzy miastami*/
        public City(int cityIndex, double latitude, double longitude, double demand)
        {
            this.Index = cityIndex;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Demand = demand;
        }
    }
}
