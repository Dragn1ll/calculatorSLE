namespace Contract;

public interface IField<T> where T : IFraction
{
    /// <summary>
    /// Двумерный массив с введёнными значениями
    /// </summary>
    T[][] Matrix { get; }
    /// <summary>
    /// Вывод поля
    /// </summary>
    void Print();
}

