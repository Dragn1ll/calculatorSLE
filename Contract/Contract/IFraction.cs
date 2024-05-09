namespace Contract;

public interface IFraction
{
    /// <summary>
    /// Числитель дроби
    /// </summary>
    int Numerator { get; set; }
    /// <summary>
    /// Знаменатель дроби
    /// </summary>
    int Denominator { get; set; }
    /// <summary>
    /// Сокращение дроби
    /// </summary>
    void Reduce();
    /// <summary>
    /// Перевод дроби в строку
    /// </summary>
    /// <returns>строчная запись дроби</returns>
    string ToString();
}
