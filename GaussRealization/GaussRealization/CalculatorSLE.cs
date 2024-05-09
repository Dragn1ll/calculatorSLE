using Contract;

namespace GaussRealization;

public class CalculatorSLE : ICalculatorSLE<Fraction>
{
    public Fraction[] GausSolution(Fraction[][] matrix)
    {
        if (matrix == null || matrix.Length < 2)
            throw new ArgumentException("СЛАУ не подходит под условия решения методом Гаусса");

        Fraction[][] tmp = MoveForward(matrix);
        tmp = MoveBackward(tmp);

        var checker = Checker(tmp);

        if (checker == 1)
        {
            Fraction[] solution = new Fraction[matrix.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                solution[i] = Roots(tmp, i);
                solution[i].Reduce();
            }
            return solution;
        }
        else
        {
            var tmpFraction = new Fraction(checker);
            return new Fraction[] { tmpFraction };
        }
    }

    private static Fraction[][] MoveForward(Fraction[][] matrix) 
    {
        for (int i = 0; i < matrix.Length - 1; i++)
        {
            Parallel.For(i + 1, matrix.Length, j =>
            {
                if (matrix[i][i].Numerator == 0)
                {
                    int index = FindNonZero(matrix, i, i);
                    if (index != -1)
                        Swap(matrix, i, index);
                }
                if (matrix[i][i].Numerator == 0) return;
                Fraction k = FractionUtils.Divide(new Fraction(-matrix[j][i].Numerator, matrix[j][i].Denominator), matrix[i][i]);
                for (int l = 0; l < matrix[0].Length; l++)
                {
                    matrix[j][l] = FractionUtils.Add(matrix[j][l], FractionUtils.Multiply(k, matrix[i][l]));
                    matrix[j][l].Reduce();
                }
            });
        }

        return matrix;
    }

    private static Fraction[][] MoveBackward(Fraction[][] matrix)
    {
        for (int i = matrix.Length - 1; i > 0; i--)
        {
            Parallel.For(0, i, j =>
            {
                if (matrix[i][i].Numerator == 0) return;
                Fraction k = FractionUtils.Divide(new Fraction(-matrix[j][i].Numerator, matrix[j][i].Denominator), matrix[i][i]);
                int[] indexes = new int[] { matrix[0].Length - 1, i };
                foreach (int l in indexes)
                {
                    matrix[j][l] = FractionUtils.Add(matrix[j][l], FractionUtils.Multiply(k, matrix[i][l]));
                    matrix[j][l].Reduce();
                }
            });
        }

        return matrix;
    }

    private static int FindNonZero(Fraction[][] matrix, int i, int j)
    {
        for (int k = j; k < matrix.Length; k++)
            if (matrix[k][i].Numerator != 0)
                return k;
        return -1;
    }
    private static void Swap(Fraction[][] matrix, int i, int i2)
    {
        Fraction[] buffer = matrix[i];
        matrix[i] = matrix[i2];
        matrix[i2] = buffer;
    }

    private static int Checker(Fraction[][] matrix)
    {
        int zeroLineindex = IndexOfZeroLine(matrix);
        if (zeroLineindex != -1)
        {
            if (matrix[zeroLineindex][matrix[0].Length - 1].Numerator == 0)
                return 0; // бесконечное количество решений
            else
                return -1; // нет корней
        }

        return 1; // есть решение
    }

    private static int IndexOfZeroLine(Fraction[][] matrix)
    {
        for (int line = 0; line < matrix.Length; line++)
        {
            bool use = true;
            for (int i = 0; i < matrix[line].Length - 1; i++)
            {
                if (matrix[line][i].Numerator != 0)
                {
                    use = false; 
                    break;
                }
            }
            if (use)
                return line;
        }

        return -1;
    }

    private static Fraction Roots(Fraction[][] matrix, int i)
    {
        for (int j = 0; j < matrix.Length; j++)
        {
            if (matrix[i][j].Numerator != 0)
                return FractionUtils.Divide(matrix[i][matrix[0].Length - 1], matrix[i][j]);
        }

        return null;
    }

    public void PrintResults(Fraction[] results)
    {
        if (results.Length > 1)
            for (int i = 0; i < results.Length; i++)
                Console.WriteLine($"x{i + 1} = {results[i].ToString()}");
        else if (results[0].Numerator == 0)
            Console.WriteLine("Бесконечное количество решений");
        else
            Console.WriteLine("Нет корней");
    }
}
