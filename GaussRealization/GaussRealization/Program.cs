using GaussRealization;

Fraction[][] matrix = new Fraction[][]
{
    new Fraction[] { new Fraction(2), new Fraction(1), new Fraction(-1), new Fraction(5) },
    new Fraction[] { new Fraction(3), new Fraction(-2), new Fraction(2), new Fraction(-4) },
    new Fraction[] { new Fraction(1), new Fraction(3), new Fraction(1), new Fraction(10) }
};

CalculatorSLE calculator = new CalculatorSLE();
var results = calculator.GausSolution(matrix);
calculator.PrintResults(results);