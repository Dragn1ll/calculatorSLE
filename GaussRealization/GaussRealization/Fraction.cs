using Contract;

namespace GaussRealization;

public class Fraction : IFraction
{
    public int Numerator { get; set; }
    public int Denominator {  get; set; }

    public Fraction(int numerator, int denominator = 1)
    {
        int sign = numerator < 0 != denominator < 0 ? -1 : 1;
        Numerator = sign * Math.Abs(numerator);
        Denominator = Math.Abs(denominator);
    }

    public override string ToString()
    {
        if (Numerator == 0)
            return "0";
        if (Denominator == 1)
            return Numerator.ToString();
        return $"{Numerator} / {Denominator}";
    }

    public void Reduce()
    {
        int nod = GCD(Math.Abs(Numerator), Denominator);
        Numerator /= nod;
        Denominator /= nod;
    }

    private int GCD(int a, int b)
    {
        while (b != 0)
        {
            int t = a;
            a = b;
            b = t % b;
        }

        return a;
    }
}