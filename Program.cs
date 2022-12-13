﻿using AntColonyNamespace;

var benchamrkFIles = new List<string>()
{
    "P-n20-k2.txt", //1
    "A-n32-k5.txt", //2
    "A-n39-k5.txt", //3
    "A-n45-k5.txt", //4
    "A-n48-k7.txt", //5
    "P-n50-k10.txt", //6
    "P-n55-k10.txt", //7
    "P-n60-k10.txt", //8
    "P-n65-k10.txt", //9
    "P-n70-k10.txt", //10
    "P-n76-k5.txt", //11
    "E-n101-k8.txt", //12
};

var coefsStruct = new AntParams(0.5, 5, 0.4, 0.01, 0.005, 1, 300, 30, 4000);

foreach (var benchmarkFile in benchamrkFIles)
{
    var ACSForCVRP = new ACS(benchmarkFile, 1, 4, "Solution.txt", "SolutionP.txt", coefsStruct);
    ACSForCVRP.SolveCVRP();
}
