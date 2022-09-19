using AntColonyNamespace;

for (int j = 0; j < 1; ++j)
{
    // Possibilities possibilities = new Possibilities(2.3);
    // possibilities.CountNominatorAndUpdateDenominator(
    //     new EdgeWithDestinationCity(new Edge(10, 3), 1)
    // );
    // possibilities.CountNominatorAndUpdateDenominator(
    //     new EdgeWithDestinationCity(new Edge(12, 5), 1)
    // );
    // possibilities.CountNominatorAndUpdateDenominator(
    //     new EdgeWithDestinationCity(new Edge(8, 7), 1)
    // );
    // possibilities.CountNominatorAndUpdateDenominator(
    //     new EdgeWithDestinationCity(new Edge(4, 2), 1)
    // );
    // possibilities.CountNominatorAndUpdateDenominator(
    //     new EdgeWithDestinationCity(new Edge(15, 4.5), 1)
    // );
    // possibilities.CountNominatorAndUpdateDenominator(
    //     new EdgeWithDestinationCity(new Edge(5, 13), 1)
    // );
    // possibilities.CountProbabilities();
    // Console.WriteLine(
    //     possibilities.GetMaxNominator().Key.DestinationCity
    //         + "--"
    //         + +possibilities.GetMaxNominator().Key.DestinationCity
    //         + "--"
    //         + +possibilities.GetMaxNominator().Value
    // );
    // foreach (var item in possibilities.GetProbabilities())
    // {
    //     Console.WriteLine(item.Value);
    // }
    // possibilities.RestartAllValues();

    Console.WriteLine("-------------------------");

    AntColony antColony = new AntColony(
        new Graph(),
        0.1,
        2.3,
        0.1,
        3,
        4,
        "C:\\Users\\Kuba\\Desktop\\.NET_App\\AntColonySystem\\BenchmarkData\\A-n32-k5.txt"
    //"/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/src/BenchmarkData/A-n32-k5.txt"
    );
    antColony.StartLookingForPath();

    // Graph g = new Graph(
    //     "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/src/BenchmarkData/A-n32-k5.txt"
    // );

    //Console.WriteLine(antColony.Graph.ToString());
}

//https://sci-hub.se/http://dx.doi.org/10.17535/crorr.2017.0029
//https://www.sciencedirect.com/topics/computer-science/pheromone-matrix
//https://en.wikipedia.org/wiki/Complete_graph
//https://sci-hub.se/https://link.springer.com/article/10.1007/s00500-019-04072-6
//https://www.google.com/search?client=firefox-b-d&q=a+giant+tour+representation#imgrc=IOSUiKddzLveiM
//https://cezarywalenciuk.pl/blog/programing/ienumerable-i-ienumerator-implementowanie-tych-interfejsow
// var lista = new List<double>();
// var random = new Random();
// for (int i = 0; i < 10; ++i)
// {
//     lista.Add(random.NextDouble());
// }
// lista.ForEach(m =>
// {
//     Console.WriteLine(m);
// });
