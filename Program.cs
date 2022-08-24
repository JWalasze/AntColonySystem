﻿using AntColonyNamespace;

// Ant ant = new Ant(new AntColony());
//Console.WriteLine(ant.getPheromonePathLength());
// for (int i = 0; i < 20; ++i)
// {
//     Console.WriteLine(new AntColony(1, 1, 1, 1).Ants[0].GetRandomNumberFrom0To1());
// }

Graph graph = new Graph(5);
graph.AddDirectedEdge(0, 1);
graph.AddUndirectedEdge(0, 2);
graph.AddDirectedEdge(3, 1);

graph.AddUndirectedEdge(2, 3);
Console.WriteLine(graph.ToString());
Console.WriteLine("Liczba wierzchołków: " + graph.GetNumberOfVertexes());

AdjacencyList a = new AdjacencyList();
a.AddVertex();
a.AddVertex();
a.AddVertex();
a.AddVertex();
a.AddDirectedEdge(new Edge(1, 2, 1, 1));
a.AddDirectedEdge(new Edge(0, 2, 1, 1));
a.AddDirectedEdge(new Edge(3, 2, 1, 1));
a.AddDirectedEdge(new Edge(1, 0, 1, 1));
Console.WriteLine(a.ToString());
