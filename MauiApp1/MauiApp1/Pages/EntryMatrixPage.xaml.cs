using System.Reflection;
using GaussRealization;

namespace MauiApp1.Pages;

public partial class EntryMatrixPage : ContentPage
{
    private Assembly _assembly;
    private int _dimension;
    private Grid _mainGrid;
    private readonly List<Entry> _entries = new List<Entry>();
    private Fraction[][] _matrix;

    public EntryMatrixPage(int dimension, Assembly assembly)
    {
        _assembly = assembly;
        _dimension = dimension;
        InitializeComponent();
        InitializeGrid(dimension);
    }

    private void InitializeGrid(int n)
    {
        _matrix = new Fraction[n][];
        _mainGrid = new Grid
        {
            ColumnSpacing = 5,
            RowSpacing = 5,
            WidthRequest = (n + 2) * 85,
            HeightRequest = n * 60,
        };


        Frame frame = new Frame
        {
            Margin = 20,
            Content = _mainGrid,
            CornerRadius = 10,
            HasShadow = true,
            Padding = 15,
            WidthRequest = (n + 2) * 90,
            HeightRequest = n * 70,
            BackgroundColor = Colors.White,
        };

        EntryPageLayout.Add(frame);

        for (int i = 0; i < n; i++)
        {
            _mainGrid.RowDefinitions.Add(new RowDefinition());
            _mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        _mainGrid.ColumnDefinitions.Add(new ColumnDefinition());

        SetInitialContent(n);
        SetSymbolsContent(n);
        AddMainButton();
    }

    private void SetInitialContent(int n)
    {
        for (int row = 0; row < n; row++)
        {
            for (int column = 0; column < (n + 1) * 2; column += 2)
            {
                Entry entry = new Entry
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 18,
                    WidthRequest = 45,
                    HeightRequest = 40,
                    MaxLength = int.MaxValue,
                    BackgroundColor = Colors.GhostWhite,
                    TextColor = Colors.Black,
                };

                entry.TextChanged += EntryTextChanged!;

                _entries.Add(entry);
                _mainGrid.Children.Add(entry);

                Grid.SetRow(entry, row);
                Grid.SetColumn(entry, column);
            }
        }
    }

    private void EntryTextChanged(object sender, TextChangedEventArgs e)
    {
        Entry entry = (Entry)sender;
        string newText = e.NewTextValue;

        if (!string.IsNullOrEmpty(newText))
        {
            newText = string.Join("", newText);
            entry.Text = newText;
        }
    }

    private void SetSymbolsContent(int n)
    {
        for (int row = 0; row < n; ++row)
        {
            for (int column = 1; column < n * 2; column += 2)
            {
                Label text = new Label()
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 18,
                    WidthRequest = 50,
                    HeightRequest = 40,
                    TextColor = Colors.Black,
                };

                if (column < (n- 1) * 2)
                    text.Text = $"x{column / 2 + 1}  + ";
                else
                    text.Text = $"x{column / 2 + 1} = ";

                _mainGrid.Children.Add(text);

                Grid.SetRow(text, row);
                Grid.SetColumn(text, column);
            }
        }
    }

    private void AddMainButton()
    {
        Button mainButton = new Button()
        {
            Text = "Решить",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            WidthRequest = 200,
            HeightRequest = 60,
            FontSize = 18,
            Margin = 30,

        };

        mainButton.Clicked += (sender, e) =>
        {
            bool allFieldsFilled = _entries.All(entry => !string.IsNullOrEmpty(entry.Text));

            if (allFieldsFilled)
            {
                for (int i = 0; i < _dimension; i++)
                {
                    _matrix[i] = new Fraction[_dimension + 1];
                    for (int j = 0; j < (_dimension + 1); j++)
                    {
                        if (int.TryParse(_entries[i * (_dimension + 1) + j].Text, out int num))
                            _matrix[i][j] = new Fraction(num);
                        else
                            DisplayAlert("Ошибка", "Есть нечисловая константа", "OK");
                    }
                }
                ToPage();
            }
            else
            {
                DisplayAlert("Ошибка", "Не все поля заполнены", "OK");
            }
        };

        EntryPageLayout.Children.Add(mainButton);
    }

    private async void ToPage()
    {
        await Navigation.PushAsync(new VisualizationPage(_matrix, _assembly, _dimension));
    }
}