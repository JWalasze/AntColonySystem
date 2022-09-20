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
        0.1,
        2.3,
        0.2,
        0.1,
        3,
        4,
        //"C:\\Users\\Kuba\\Desktop\\.NET_App\\AntColonySystem\\BenchmarkData\\A-n32-k5.txt"
        "/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/A-n32-k5.txt"
    );

    antColony.StartSolvingProblemInSeries();

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
//EXTREMALY IMPORTANT!!!
//https://sci-hub.se/https://link.springer.com/chapter/10.1007/1-4020-3432-6_21
//https://d1wqtxts1xzle7.cloudfront.net/57723244/357_Solving_Vehicle_Routing_Problem_using_Ant_Colony_Optimisation_ACO_Algorithm_FINAL-with-cover-page-v2.pdf?Expires=1663669356&Signature=GbW1SpvllXcJTRD5MtEbzoOP04KqS1YGsYHFOj5VYg-~1-4u22fQ21rm56t9t4cJBDEP65xpwcU6KCb4V0kRJqDRhYTFneFGLIkTvd0pNV8wnRbJF5NVzAOcy8mxf0uIi3D5JVedbfzakkFq~L7eQKF0lUxA28W3WtWzNSUkkhIgXhpGIE~9Ng5Qh6fp0OiHMsjglwRmy9-i5EM804UL~BcgBHM9tcswKPaTdHnF4CP6CKqVzGku6~bbotnYLipZPHGUJqqE9c55PTLIk~WBwP9jmXWBJJJ5qhvDhwRSJbUU1BbQqszN4YxoVhI8-vqQDTgX1NEalRW4kBBcJaxIPg__&Key-Pair-Id=APKAJLOHF5GGSLRBV4ZA
