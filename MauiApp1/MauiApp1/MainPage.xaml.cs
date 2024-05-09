using MauiApp1.Pages;
using System.Reflection;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private int _dimension;
        private string _realizationPath;
        private Assembly _assembly;
        private bool _assemblyStatus;
        private Type[] _types;
        private Type[] _interfaces;
        private Dictionary<Type, Type> _typesDictionary = new();
        private string _contractPath = @"C:\Users\abdul\tren\CalculatorSLE\Contract\Contract\bin\Debug\net8.0\Contract.dll";
        private Button _dllButton = new()
        {
            Text = "Загрузить сборку",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            WidthRequest = 200,
            HeightRequest = 60,
            FontSize = 20,
        };

        public MainPage()
        {
            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            CreateEntry();
            AddMainButton();
            AddGetDllButton();
        }

        private void CreateEntry()
        {
            Entry entry = new Entry()
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 18,
                WidthRequest = 50,
                HeightRequest = 50,
                MaxLength = 100,
                TextColor = Colors.Black,
            };
            entry.TextChanged += EntryTextChanged!;

            MainLayout.Children.Add(entry);
        }

        private void EntryTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)sender;
            if (int.TryParse(e.NewTextValue, out int result))
            {
                _dimension = result;
            }
        }

        private void AddMainButton()
        {
            Button mainButton = new Button()
            {
                Text = "Далее",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 200,
                HeightRequest = 60,
                FontSize = 18,
            };
            
            mainButton.Clicked += (sender, e) =>
            {
                ToPage();
            };

            MainLayout.Children.Add(mainButton);
        }

        private async void ToPage()
        {
            if (_dimension > 10)
                await DisplayAlert("Соси бибу", "СЛАУ слишком велика. Введите количество неизвестных максимум: 10", "OK");
            else if (_dimension > 1)
            {
                if (_assemblyStatus)
                {
                    await Navigation.PushAsync(new EntryMatrixPage(_dimension, _assembly), true);
                }
                else
                {
                    await DisplayAlert("Ошибка", "Не удалось найти реализацию контракта. Загрузите правильную сборку.", "OK");
                }
            }
            else
                await DisplayAlert("Ошибка", "СЛАУ слишком мала. Введите количество неизвестных минимум: 2", "OK");
        }

        private void AddGetDllButton()
        {
            _dllButton.BackgroundColor = Colors.Red;

            _dllButton.Clicked += async (sender, e) =>
            {
                await PickDllFile();
            };

            MainLayout.Children.Add(_dllButton);
        }

        private async Task PickDllFile()
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".dll" } },
                });

            try
            {
                var result = await FilePicker.PickAsync(new PickOptions()
                {
                    PickerTitle = "Выберете сборку",
                    FileTypes = customFileType,
                });

                _realizationPath = result.FullPath;
                await UploadNewDll();
            }
            catch
            {
                Console.WriteLine("Ошибка");
            }
        }

        private async Task UploadNewDll()
        {
            _assembly = Assembly.LoadFrom(_realizationPath);
            await CheckContractRealization();
        }

        private async Task CheckContractRealization()
        {
            _assemblyStatus = true;

            _types = this._assembly.GetTypes();
            _interfaces = Assembly.LoadFrom(_contractPath).GetTypes().Where(type => type.IsInterface).ToArray();

            foreach (var interfaceType in _interfaces)
            {
                bool isImplemented = false;
                foreach (var type in _types)
                {
                    foreach (var face in type.GetInterfaces())
                    {
                        if (face.IsGenericType && face.GetGenericTypeDefinition() == interfaceType)
                        {
                            isImplemented = true;
                            _typesDictionary.Add(face.GetGenericTypeDefinition(), type);

                            break;
                        }
                        else if (face == interfaceType)
                        {
                            isImplemented = true;
                            _typesDictionary.Add(face, type);

                            break;
                        }
                    }
                    if (isImplemented) break;
                }

                if (!isImplemented)
                {
                    _assemblyStatus = false;
                    break;
                }
            }

            if (_assemblyStatus)
            {
                _dllButton.BackgroundColor = Colors.Green;
            }
            else
            {
                await DisplayAlert("Ошибка", "Реализация не соответсвует контракту", "OK");
            }
        }
    }
}
