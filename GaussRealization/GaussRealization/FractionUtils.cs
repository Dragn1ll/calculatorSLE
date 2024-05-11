using Contract;

namespace GaussRealization;

public static class FractionUtils
{
    public static IFraction Add(IFraction f1, IFraction f2)
    {
        int numerator = f1.Numerator * f2.Denominator + f2.Numerator * f1.Denominator;
        int denominator = f1.Denominator * f2.Denominator;
        return new Fraction(numerator, denominator);
    }

    public static IFraction Substract(IFraction f1, IFraction f2)
    {
        return Add(f1, new Fraction(-f2.Numerator, f2.Denominator));
    }

    public static IFraction Multiply(IFraction f1, IFraction f2)
    {
        int numerator = f1.Numerator * f2.Numerator;
        int denominator = f1.Denominator * f2.Denominator;
        return new Fraction(numerator, denominator);
    }

    public static IFraction Divide(IFraction f1, IFraction f2)
    {
        return Multiply(f1, new Fraction(f2.Denominator, f2.Numerator));
    }
}
