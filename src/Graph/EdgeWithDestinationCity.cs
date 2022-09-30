namespace AntColonyNamespace
{
    //Klasa, która przechowuje odcinek wraz z indeksem docelowego miasta
    internal class EdgeWithDestinationCity
    {
        //Indeks docelowego miasta
        public int DestinationCity { get; private set; }

        //Krawędź, która prowadzi do docelowego miasta
        public Edge EdgeToDestCity { get; private set; }

        //Konstruktor
        public EdgeWithDestinationCity(Edge edgeToDestCity, int destinationCity)
        {
            this.EdgeToDestCity = edgeToDestCity;
            this.DestinationCity = destinationCity;
        }
    }
}
