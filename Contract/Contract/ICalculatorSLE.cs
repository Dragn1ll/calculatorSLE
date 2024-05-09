namespace Contract;

public interface ICalculatorSLE<T> where T : IFraction
{
    /// <summary>
    /// Метод для решения СЛАУ методом Гаусса
    /// </summary>
    /// <param name="matrix">СЛАУ, которое надо решить</param>
    /// <returns>корни уравнений в виде массива</returns>
    T[] GausSolution(T[][] matrix);
}