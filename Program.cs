using AntColonyNamespace;
using System.Collections.ObjectModel;

// Ant ant = new Ant(new AntColony());
//Console.WriteLine(ant.getPheromonePathLength());
// for (int i = 0; i < 20; ++i)
// {
//     Console.WriteLine(new AntColony(1, 1, 1, 1).Ants[0].GetRandomNumberFrom0To1());
// }

// Graph graph = new Graph(5);
// graph.AddDirectedEdge(0, 1);
// graph.AddUndirectedEdge(0, 2);
// graph.AddDirectedEdge(3, 1);

// graph.AddUndirectedEdge(2, 3);
// Console.WriteLine(graph.ToString());
// Console.WriteLine("Liczba wierzchołków: " + graph.GetNumberOfVertexes());


Graph graph = new Graph();
for (int i = 0; i < 6; ++i)
{
    graph.AddVertex();
}
graph.AddDirectedEdge(new Edge(0, 1, 10, 5));
graph.AddDirectedEdge(new Edge(1, 0, 10, 5));
graph.AddDirectedEdge(new Edge(2, 1, 1, 2));
graph.AddDirectedEdge(new Edge(1, 2, 9, 3));
graph.AddDirectedEdge(new Edge(1, 3, 4, 6));
graph.AddDirectedEdge(new Edge(3, 4, 4, 6));
graph.AddDirectedEdge(new Edge(1, 5, 2, 3));
graph.AddDirectedEdge(new Edge(2, 4, 12, 4));
graph.AddDirectedEdge(new Edge(4, 2, 13, 7));
graph.AddDirectedEdge(new Edge(4, 5, 13, 7));
graph.AddDirectedEdge(new Edge(5, 1, 10, 1));
Console.WriteLine(graph.ToString());

Possibilities possibilities = new Possibilities(2.3);
possibilities.CountNominatorAndUpdateDenominator(new Edge(0, 1, 10, 3));
possibilities.CountNominatorAndUpdateDenominator(new Edge(0, 2, 12, 5));
possibilities.CountNominatorAndUpdateDenominator(new Edge(1, 2, 8, 7));
possibilities.CountNominatorAndUpdateDenominator(new Edge(1, 3, 4, 2));
possibilities.CountNominatorAndUpdateDenominator(new Edge(3, 0, 15, 4.5));
possibilities.CountNominatorAndUpdateDenominator(new Edge(2, 3, 5, 13));
possibilities.CountProbabilities();
Console.WriteLine(
    possibilities.GetMaxNominator().Key.StartVertex
        + "--"
        + +possibilities.GetMaxNominator().Key.EndVertex
        + "--"
        + +possibilities.GetMaxNominator().Value
);
foreach (var item in possibilities.GetProbabilities())
{
    Console.WriteLine(item.Value);
}
possibilities.RestartAllValues();

Console.WriteLine("-------------------------");

AntColony antColony = new AntColony(graph, 100, 0.1, 2.3, 100, 0.1, 100);
antColony.AddAntToTheColony();
antColony.AddAntToTheColony();
antColony.AddAntToTheColony();
antColony.StartLookingForPath();

//https://www.sciencedirect.com/topics/computer-science/pheromone-matrix
