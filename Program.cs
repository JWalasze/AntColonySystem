using AntColonyNamespace;

for (int j = 0; j < 10; ++j)
{
    AntColony antColony = new AntColony(
        0.8,
        2,
        0.9,
        0.2,
        4,
        10,
        "C:\\Users\\Kuba\\Desktop\\.NET_App\\AntColonySystem\\BenchmarkData\\A-n32-k5.txt"
    //"/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/A-n32-k5.txt"
    );
    antColony.StartSolvingProblemInSeries();
}

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


//To jest mega wazne, mamy tuatj jak aktualizowac feromony i jaka powinna byc ih inicjalizacyjna wartosc xDD
//https://sci-hub.se/https://www.tandfonline.com/doi/abs/10.1080/10170660609509001
//--------------------------------------------------------------------------------

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
//EXTREMALY IMPORTANT!!! WE WILL SEE..
//https://sci-hub.se/https://link.springer.com/chapter/10.1007/1-4020-3432-6_21
//https://d1wqtxts1xzle7.cloudfront.net/57723244/357_Solving_Vehicle_Routing_Problem_using_Ant_Colony_Optimisation_ACO_Algorithm_FINAL-with-cover-page-v2.pdf?Expires=1663669356&Signature=GbW1SpvllXcJTRD5MtEbzoOP04KqS1YGsYHFOj5VYg-~1-4u22fQ21rm56t9t4cJBDEP65xpwcU6KCb4V0kRJqDRhYTFneFGLIkTvd0pNV8wnRbJF5NVzAOcy8mxf0uIi3D5JVedbfzakkFq~L7eQKF0lUxA28W3WtWzNSUkkhIgXhpGIE~9Ng5Qh6fp0OiHMsjglwRmy9-i5EM804UL~BcgBHM9tcswKPaTdHnF4CP6CKqVzGku6~bbotnYLipZPHGUJqqE9c55PTLIk~WBwP9jmXWBJJJ5qhvDhwRSJbUU1BbQqszN4YxoVhI8-vqQDTgX1NEalRW4kBBcJaxIPg__&Key-Pair-Id=APKAJLOHF5GGSLRBV4ZA

//NEW ONES
//https://sci-hub.se/https://www.tandfonline.com/doi/abs/10.1080/02522667.2005.10699639 NICE DESCRIPTIONS
//https://sci-hub.se/https://link.springer.com/chapter/10.1007/1-4020-3432-6_21 EQUATIONS BUT NOTHING BRILLIANT
//https://sci-hub.se/https://link.springer.com/chapter/10.1007/11839088_30 NOT GOOD
//https://sci-hub.se/https://ieeexplore.ieee.org/abstract/document/7018311 NICE PSEUDO CODE
//https://sci-hub.se/https://link.springer.com/chapter/10.1007/978-981-10-1837-4_96 NICE ALGORITHM CODE AND SOME EQUATIONS...VERY GOOOOD
//https://sci-hub.se/https://www.sciencedirect.com/science/article/abs/pii/S0305054804003223 NOT REALLY

//https://ttpsc.com/pl/blog/jak-zaczac-przygode-z-azure-i-przygotowac-sie-do-certyfikacji-az-900/ AZURE...GREAT

// var myEdge = new Edge(1, 1);
// var e1 = new EdgeWithDestinationCity(myEdge, 5);
// var e2 = new EdgeWithDestinationCity(myEdge, 6);
// Console.WriteLine("e1: " + e1.EdgeToDestinationCity.PheromoneLevel);
// Console.WriteLine("e2: " + e2.EdgeToDestinationCity.PheromoneLevel);

// e1.EdgeToDestinationCity.UpdatePheromoneLevel(90);
// Console.WriteLine("e1: " + e1.EdgeToDestinationCity.PheromoneLevel);
// Console.WriteLine("e2: " + e2.EdgeToDestinationCity.PheromoneLevel);

// var c = new GiantTourSolution();
// c.AddPathToNextCity(1, new Edge(2, 5));
// c.AddPathToNextCity(10, new Edge(2, 5));
// c.AddPathToNextCity(100, new Edge(2, 5));
// c.ForEach(m =>
// {
//     Console.WriteLine(m._CityIndex);
// });
