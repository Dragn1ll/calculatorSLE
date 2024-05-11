using Contract;

namespace MauiApp1.Pages;

public partial class VisualizationPage : ContentPage
{
    private readonly IFraction[][] _inputMatrix;
    private Label[,] _matrix;
    private readonly int _dimension;
    private IFraction[] _resultMatrix;
    private int _timeOfSolution = 5;
    private readonly Type _solution;
    private readonly Button _backButton = new()
    {
        Text = "Назад",
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.CenterAndExpand,
        WidthRequest = 200,
        HeightRequest = 60,
        FontSize = 18,
        Margin = 30,
        Background = Colors.SkyBlue
    };
    private Button _changeButton = new Button()
    {
        Text = "Изменить",
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.CenterAndExpand,
        WidthRequest = 200,
        HeightRequest = 60,
        FontSize = 18,
        Margin = 10,
        Background = Colors.SkyBlue
    };
    private readonly Entry _entryTime = new Entry
    {
        Placeholder = "5",
        HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center,
        FontSize = 18,
        WidthRequest = 45,
        HeightRequest = 40,
        MaxLength = int.MaxValue,
        BackgroundColor = Colors.GhostWhite,
        TextColor = Colors.Black,
    };
    

    private readonly ActivityIndicator _activityIndicator = new ActivityIndicator()
    {
        IsRunning = true,
        HeightRequest = 100,
        WidthRequest = 100,
    };

    public VisualizationPage(IFraction[][] matrix, int dimension, Type solution)
    {
        _inputMatrix = matrix;
        _dimension = dimension;
        _solution = solution;
        InitializeComponent();
        AddActivityCircle();
        StartVisualization();
    }

    private void AddActivityCircle()
    {
        VisualizationPageLayout.Children.Add(_activityIndicator);
    }

    private async void StartVisualization()
    {
        var solution = Activator.CreateInstance(_solution);
        var gaussSolution = _solution.GetMethod("GausSolution");
        var getSteps = _solution.GetMethod("GetSteps");

        Grid grid = new Grid
        {
            ColumnSpacing = 5,
            RowSpacing = 5,
            WidthRequest = _dimension * 70,
            HeightRequest = _dimension * 60,
        };

        Frame frame = new Frame
        {
            Margin = 20,
            Content = grid,
            CornerRadius = 10,
            HasShadow = true,
            Padding = 15,
            WidthRequest = _dimension * 90,
            HeightRequest = _dimension * 80,
            BackgroundColor = Colors.White,
        };

        VisualizationPageLayout.Add(frame);
        
        AddSpeedEntry();
        AddGoBackButton();

        VisualizationPageLayout.Children.Remove(_activityIndicator);

        for (int i = 0; i < _dimension; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        grid.ColumnDefinitions.Add(new ColumnDefinition());

        _matrix = new Label[_dimension, _dimension + 1];

        for (int row = 0; row < _dimension; row++)
        {
            for (int column = 0; column < (_dimension + 1); column++)
            {
                Label label = new Label
                {
                    Text = _inputMatrix[row][column].ToString(),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 20,
                    BackgroundColor = Colors.White,
                    TextColor = Colors.Black,
                };

                grid.Add(label, column, row);
                _matrix[row, column] = label;
            } 
        }

        _resultMatrix = await Task.Run(() => (IFraction[])gaussSolution.Invoke(solution, new object[] { _inputMatrix })!);

        (int Row, IFraction[] Nums)[] steps = ((int, IFraction[])[])getSteps.Invoke(solution, null)!;

        await Task.Delay(2000);
        foreach (var step in steps)
        {
            for (int column = 0; column < (_dimension + 1); column++)
            {
                _matrix[step.Row, column].Text = step.Nums[column].ToString();
                
                await Task.Delay(_timeOfSolution * 500);
            }
        }

        VisualizationPageLayout.Remove(frame);

        VisualizationPageLayout.Remove(_backButton);
        VisualizationPageLayout.Remove(_entryTime);
        VisualizationPageLayout.Remove(_changeButton);


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
            WidthRequest =  1000,
            HeightRequest = _dimension * 90,
            BackgroundColor = Colors.LightGray,
            BorderColor = Colors.LightGray,
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
                WidthRequest = 500,
                HeightRequest = 80,
                TextColor = Colors.Black,
            };

            grid.Children.Add(answer);
            grid.SetRow(answer, i);
            grid.SetColumn(answer, 0);
        }

        VisualizationPageLayout.Children.Add(frame);
        VisualizationPageLayout.Children.Add(_backButton);
    }

    private void AddGoBackButton()
    {
        _backButton.Clicked += ToMainPage;

        VisualizationPageLayout.Children.Add(_backButton);
    }

    private async void ToMainPage(object? sender, EventArgs e)
    {
        await Task.Delay(100);
        await Navigation.PopAsync();
    }

    private void AddSpeedEntry()
    {
        _entryTime.TextChanged += EntryTextChanged!;

        _changeButton.Clicked += SpeedChanged!;

        VisualizationPageLayout.Children.Add(_entryTime);
        VisualizationPageLayout.Children.Add(_changeButton);
    }


    private async void EntryTextChanged(object sender, TextChangedEventArgs e)
    {
        await Task.Run(() =>
        {
            Entry entry = (Entry)sender;
            string newText = e.NewTextValue;

            if (!string.IsNullOrEmpty(newText))
            {
                newText = string.Join("", newText);
                entry.Text = newText;
            }
        });
    }

    private async void SpeedChanged(object sender, EventArgs e)
    {
        await Task.Run(() =>
        {
            if (_entryTime != null!)
                if (int.TryParse(_entryTime.Text, out int num))
                    _timeOfSolution = num;
                else
                    DisplayAlert("Ошибка", "Введите число", "ОК");
        });
    }
}