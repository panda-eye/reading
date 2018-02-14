using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Чтение.Localization;
using Чтение.Managers;
using Чтение.Pages;

namespace Чтение
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ManageButton.Click += Button_Click;
            GoButton.Click += Button_Click;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            TitleGrid.Children.Clear();
            TitleGrid.ColumnDefinitions.Clear();
            DataGrid.Width = 0;

            RecreateDataGrid();
        }

        private void RecreateDataGrid()
        {
            char[] letters = LocalizationManager.Instance.CurrentCulture.Name
                .Equals("ru-RU") ? new[] { 'ч', 'т', 'е', 'н', 'и', 'е' } : new[] { 'ч', 'и', 'т', 'а', 'н', 'н', 'я' };
            for (int i = 0; i < letters.Length; i++)
            {
                DataGrid.Width += 200;
                TitleGrid.ColumnDefinitions.Add(new ColumnDefinition());

                var image = new Image()
                {
                    Source = new BitmapImage(new Uri($"pack://application:,,,/Чтение;component/Resources/Letters/{letters[i]}.png"))
                };
                Grid.SetColumn(image, i);
                TitleGrid.Children.Add(image);
            }
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            var im = (Button)sender;
            im.Background = new ImageBrush()
            {
                ImageSource = new Image()
                {
                    Source = new BitmapImage(
                            new Uri($"pack://application:,,,/Чтение;component/Resources/Flags/flag_{im.Tag}.png"))
                }.Source
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Tag)
            {
                case "go":
                    {
                        NavigationService.Navigate(new LevelsPage());
                        break;
                    }
                case "uk":
                    {
                        ApplyAnimation("uk-UA");
                        break;
                    }
                case "ru":
                    {
                        ApplyAnimation("ru-RU");
                        break;
                    }
                case "manage":
                    {
                        NavigationService.Navigate(new ManagePage());
                        break;
                    }
            }
        }

        private void ApplyAnimation(string cultureInfo)
        {
            if (!LocalizationManager.Instance.CurrentCulture.Name.Equals(cultureInfo))
                AnimationManager.ApplyOpacityAnimation(() =>
                    {
                        LocalizationManager.Instance.CurrentCulture = new CultureInfo(cultureInfo);
                        UpdateTitle();
                    }, true, 1, 0, DataGrid, ManageButton);
        }
    }
}
