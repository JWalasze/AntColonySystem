using AntColonyNamespace;
using System.Collections.ObjectModel;

for (int j = 0; j < 1; ++j)
{
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

    AntColony antColony = new AntColony(graph, 0.1, 2.3, 0.1, 3, 2);
    antColony.AddAntToTheColony();
    antColony.AddAntToTheColony();
    antColony.AddAntToTheColony();
    antColony.StartLookingForPath();

    Console.WriteLine("...XDDDD");

    Console.WriteLine(graph.ToString());
}

// City city = new City(20);

//!!!!!!!!
//https://sci-hub.se/http://dx.doi.org/10.17535/crorr.2017.0029

//https://www.sciencedirect.com/topics/computer-science/pheromone-matrix

//https://en.wikipedia.org/wiki/Complete_graph
//https://sci-hub.se/https://link.springer.com/article/10.1007/s00500-019-04072-6
//https://www.google.com/search?client=firefox-b-d&q=a+giant+tour+representation#imgrc=IOSUiKddzLveiM
//https://cezarywalenciuk.pl/blog/programing/ienumerable-i-ienumerator-implementowanie-tych-interfejsow
// List<int> ll = new List<int> { 1, 2, 3, 4, 5 };
// Console.WriteLine(ll[3]);
// ReadOnlyCollection<int> k = new ReadOnlyCollection<int>(ll);
// Console.WriteLine(k[3]);
// bool mn = object.ReferenceEquals(ll, k);
// Console.WriteLine(mn);

CompletedGraph l = new CompletedGraph(5);
