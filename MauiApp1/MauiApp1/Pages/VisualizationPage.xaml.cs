using System.Reflection;
using GaussRealization;

namespace MauiApp1.Pages;

public partial class VisualizationPage : ContentPage
{
    private Fraction[][] _matrix;
    private Assembly _assembly;
    private int _dimension;
    private Fraction[] _resultMatrix;

    private ActivityIndicator activityIndicator = new ActivityIndicator()
    {
        IsRunning = true,
        HeightRequest = 100,
        WidthRequest = 100,
    };

    public VisualizationPage(Fraction[][] matrix, Assembly assembly, int dimension)
    {
        _matrix = matrix;
        _assembly = assembly;
        _dimension = dimension;
        InitializeComponent();
        AddActivityCircle();
        GetDataFromDll();
    }

    private void AddActivityCircle()
    {
        VisualizationPageLayout.Children.Add(activityIndicator);
    }

    private void GetDataFromDll()
    {
        Type? calculatorSLEType = _assembly.GetTypes().FirstOrDefault(t => t.Name == "CalculatorSLE");

        var calculatorSLEInstance = Activator.CreateInstance(calculatorSLEType!);

        MethodInfo? method = calculatorSLEType!.GetMethod("GausSolution");

        var result = (Fraction[])method.Invoke(calculatorSLEInstance, new object[] { _matrix })!;

        _resultMatrix = result;

        MainThread.BeginInvokeOnMainThread(InitializeGrid);

    }

    private void InitializeGrid()
    {
        if (_resultMatrix.Length == 1)
        {
            if (_resultMatrix[0].Numerator == 0)
                DisplayAlert("Ошибка", "СЛАУ имеет бесконечное количество решений", "ОК");
            else
                DisplayAlert("Ошибка", "У СЛАУ нет корней", "ОК");

            return;
        }

        Grid grid = new Grid();

        Frame frame = new Frame()
        {
            Content = grid,
            CornerRadius = 10,
            HasShadow = true,
            Padding = new Thickness(15),
            WidthRequest =  500,
            HeightRequest = _dimension * 90,
            BackgroundColor = Colors.White,
            BorderColor = Colors.White,
        };

        for (int i = 0; i < _dimension; i++)
            grid.RowDefinitions.Add(new RowDefinition());
        grid.ColumnDefinitions.Add(new ColumnDefinition());

        for (int i = 0; i < _dimension; i++)
        {
            _resultMatrix[i].Reduce();
            Label answer = new Label()
            {
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = $"x{i + 1} = {_resultMatrix[i].ToString()}",
                FontSize = 30,
                WidthRequest = 190,
                HeightRequest = 80,
                TextColor = Colors.Black,
            };

            grid.Children.Add(answer);
            grid.SetRow(answer, i);
            grid.SetColumn(answer, 0);
        }

        VisualizationPageLayout.Children.Add(frame);

        VisualizationPageLayout.Children.Remove(activityIndicator);
    }
}