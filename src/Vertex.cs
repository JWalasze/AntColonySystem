using System;

namespace AntColonyNamespace
{
    internal class Vertex
    {
        private static int VertexIndexCounter = 0;
        public int VertexIndex { get; }

        public Vertex()
        {
            this.VertexIndex = VertexIndexCounter++;
            Console.WriteLine(this.VertexIndex);
        }
    }
}
