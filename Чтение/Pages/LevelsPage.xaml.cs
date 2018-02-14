using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;

namespace Чтение.Pages
{
    /// <summary>
    /// Логика взаимодействия для LevelsPage.xaml
    /// </summary>
    public partial class LevelsPage : Page
    {
        public LevelsPage()
        {
            InitializeComponent();

            BackButton.Content = new Image()
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Чтение;component/Resources/Arrows/back.png"))
            };

            Builder(3, 2, 3, 3);
            //UnderLevels.Width = LevelsData.Width + 150;
            //UnderLevels.Height = LevelsData.Height + 150;
        }

        //public void Update(int levels)
        //{
        //    if (levels > 0)
        //        switch (levels)
        //        {
        //            case 1:
        //                {
        //                    Builder(1, 1, 1);
        //                    break;
        //                }
        //            case 2:
        //                {
        //                    Builder(2, 1, 2);
        //                    break;
        //                }
        //            case 3:
        //                {
        //                    Builder(3, 1, 3);
        //                    break;
        //                }
        //            case 4:
        //                {
        //                    Builder(3, 2, 3, 1);
        //                    break;
        //                }
        //            case 5:
        //                {
        //                    Builder(3, 2, 3, 2);
        //                    break;
        //                }
        //            case 6:
        //                {
        //                    Builder(3, 2, 3, 3);
        //                    break;
        //                }
        //        }
        //    UnderLevels.Width = LevelsData.Width + 150;
        //    UnderLevels.Height = LevelsData.Height + 150;
        //}

        private string[] Names => new[] { "one", "two", "three", "four", "five", "six" };

        private void Builder(int columns, int rows, int row_1, int row_2 = 0, int row_3 = 0)
        {
            LevelsData.Height = 200;
            for (int vl = 0; vl < columns; vl++)
            {
                LevelsData.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int vl = 0; vl < rows; vl++)
            {
                LevelsData.RowDefinitions.Add(new RowDefinition());
            }
            for (int vl = 0; vl < row_1; vl++)
            {
                var image = NewImage(vl);
                if (image != null)
                {
                    Grid.SetColumn(image, vl);
                    LevelsData.Children.Add(image);
                }
            }
            LevelsData.Width = row_1 * 200;
            if (row_2 == 0)
                return;
            LevelsData.Height += 200;
            for (int vl = 0; vl < row_2; vl++)
            {
                var image = NewImage(vl + 3);
                Grid.SetColumn(image, vl);
                Grid.SetRow(image, 1);
                LevelsData.Children.Add(image);
            }
            if (row_3 == 0)
                return;
            LevelsData.Height += 200;
            for (int vl = 0; vl < row_3; vl++)
            {
                var image = NewImage(vl + 6);
                Grid.SetColumn(image, vl);
                Grid.SetRow(image, 2);
                LevelsData.Children.Add(image);
            }
        }

        private Button NewImage(int ind)
        {
            Uri path = new Uri($"pack://application:,,,/Чтение;component/Resources/Numbers/{Names[ind]}.png");
            Button but = new Button()
            {
                Content = new Image()
                {
                    Source = new BitmapImage(path),
                    Width = 190,
                    Height = 190
                },
                Tag = ind + 1,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0)
            };
            but.Click += (sender, e) =>
            {
                switch ((sender as Button).Tag as int?)
                {
                    case 1:
                        {
                            NavigationService.Navigate(new Level1());
                            break;
                        }
                    case 2:
                        {
                            NavigationService.Navigate(new Level2());
                            break;
                        }
                    case 3:
                        {
                            NavigationService.Navigate(new Level3());
                            break;
                        }
                    case 4:
                        {
                            NavigationService.Navigate(new Level4());
                            break;
                        }
                    case 5:
                        {
                            NavigationService.Navigate(new Level5());
                            break;
                        }
                    case 6:
                        {
                            NavigationService.Navigate(new Level6());
                            break;
                        }
                }
            };
            return but;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Mouse_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as Image).Source = new BitmapImage(
                new Uri("pack://application:,,,/Чтение;component/Resources/mouse.png"));
        }

        private void Kids_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as Image).Source = new BitmapImage(
                new Uri("pack://application:,,,/Чтение;component/Resources/kids.png"));
        }
    }
}
