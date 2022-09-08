using System;

namespace AntColonyNamespace
{
    internal class Vertex
    {
        //Statyczna zmienna zeby kazdy vertex byl +1
        private static int VertexIndexCounter = 0;

        //Property zeby zwrocic index
        public int VertexIndex { get; }

        //Konstruktor
        public Vertex()
        {
            this.VertexIndex = VertexIndexCounter++;
        }
    }
}
