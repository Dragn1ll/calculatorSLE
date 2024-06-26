﻿using GaussRealization;
using System.Diagnostics;


Semaphore semaphore = new Semaphore(10, 10);
Fraction[][] matrix = new Fraction[][]
{
    new Fraction[] { new Fraction(1), new Fraction(2), new Fraction(3) },
    new Fraction[] { new Fraction(4), new Fraction(5), new Fraction(6) },
    
};

Stopwatch sw = new Stopwatch();
sw.Start();

CalculatorSLE calculator = new CalculatorSLE();
var results = calculator.GausSolution(matrix);

sw.Stop();

calculator.PrintResults(results);
Console.WriteLine(sw.ElapsedMilliseconds);