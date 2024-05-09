namespace GaussRealization;

public static class FractionUtils
{
    public static Fraction Add(Fraction f1, Fraction f2)
    {
        int numerator = f1.Numerator * f2.Denominator + f2.Numerator * f1.Denominator;
        int denominator = f1.Denominator * f2.Denominator;
        return new Fraction(numerator, denominator);
    }

    public static Fraction Substract(Fraction f1, Fraction f2)
    {
        return Add(f1, new Fraction(-f2.Numerator, f2.Denominator));
    }

    public static Fraction Multiply(Fraction f1, Fraction f2)
    {
        int numerator = f1.Numerator * f2.Numerator;
        int denominator = f1.Denominator * f2.Denominator;
        return new Fraction(numerator, denominator);
    }

    public static Fraction Divide(Fraction f1, Fraction f2)
    {
        return Multiply(f1, new Fraction(f2.Denominator, f2.Numerator));
    }
}
