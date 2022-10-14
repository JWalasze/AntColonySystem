using AntColonyNamespace;

var coefAlpha = new List<double> { 0.1, 0.2, 0.3, 0.5, 0.75, 1, 1.5, 2 };
var coefBeta = new List<double> { 0.5, 0.8, 1, 1.5, 2, 2.3, 2.5, 3, 5, 10, 20, 30 };
var coefTau = new List<double> { 0.001, 0.005, 0.01, 0.05, 0.1, 0.2, 0.3, 0.5 };
var coefQ = new List<double> { 1, 100, 1000, 10000, 50000, 100000, 200000, 500000 };
var coefQ0 = new List<double> { 0.4, 0.5, 0.7, 0.75, 0.8, 0.9, 0.95 };
var coefAnts = new List<int> { 5, 10, 20, 30, 40 };
var coefIterat = new List<int> { 5, 50, 100, 500, 1000, 5000, 6000, 7000 };

List<double> foundDistances = new List<double>();

foreach (var item in coefTau)
{
    Console.WriteLine();
    Console.WriteLine("--------------Coef=" + item + "---------------");
    Console.WriteLine();
    for (int j = 0; j < 6; ++j)
    {
        AntColony antColony = new AntColony(
            0.3, //ALFA
            1.5, //BETA
            0.75, //Q0
            0.3, //TAU //POZNIEJ 0.3xD
            100000, //Q 100000
            20, //ANTS
            2000, //ITERATIONS
            "C:\\Users\\Kuba\\Desktop\\.NET_App\\AntColonySystem\\BenchmarkData\\A-n32-k5.txt"
        //"/home/kuba/Desktop/Praca_Inzynierska/Algorytm_Mrowkowy_App/AntColonySystem/BenchmarkData/A-n32-k5.txt"
        );

        //antColony.StartSolvingProblemInSeries();
        antColony.StartSolvingProblemParallel();
        foundDistances.Add(antColony.GetGiantTourDistance()); //Zrobic zeby start Solving zwrocil rozwiAZANIE
        if (antColony.GetGiantTourDistance() < 805)
        {
            Console.WriteLine(antColony.GetGiantTourSolution().GetItineraryAllApart());
        }
        Console.WriteLine(
            "Blad: "
                + 100
                    * (antColony.GetGiantTourDistance() - antColony.GetOptimalDistance())
                    / antColony.GetOptimalDistance()
        );
    }
    Console.WriteLine("Srednia:" + foundDistances.Average());
    foundDistances.Clear();
}

//Fajnie opisana matematyka
//https://sci-hub.se/https://www.tandfonline.com/doi/abs/10.1080/02522667.2005.10699639
