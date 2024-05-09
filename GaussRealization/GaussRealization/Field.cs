using Contract;

namespace GaussRealization;

public class Field : IField<Fraction>
{
    public Fraction[][] Matrix { get; set; }

    public Field(Fraction[][] matrix)
    {
        Matrix = matrix;
    }

    public void Print()
    {
        for (int row = 0; row < Matrix.GetLength(0); row++)
        {
            for (int col = 0; col < Matrix.GetLength(1); col++)
                Console.Write($"{Matrix[row][col]}");
            Console.WriteLine();
        }
    }
}
