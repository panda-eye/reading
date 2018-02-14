using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Чтение.Localization;

namespace Чтение.Pages
{
    public partial class Level1 : Page
    {
        private Managers.Type[] Types = Managers.TypesClass.TypesLvl1.OrderBy(t => t.Name).ToArray();
        private List<Managers.Type> TempTypes = new List<Managers.Type>();

        public Level1()
        {
            InitializeComponent();

            BackButton.Content = new Image()
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Чтение;component/Resources/Arrows/back.png"))
            };

            Build();
            BuildElse();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (tb.Text.Length > 0)
                {
                    Base2Grid.Visibility = Visibility.Hidden;
                    BaseElse.Visibility = Visibility.Hidden;
                    TempTypes = Search(tb.Text);
                    BuildTemp();
                }
                else
                {
                    Base2Grid.Visibility = Visibility.Visible;
                    BaseElse.Visibility = Visibility.Visible;
                    TempTypes.Clear();
                    Build();
                }
            }
        }

        private List<Managers.Type> Search(string keywords)
        {
            string lower = keywords.ToLower();
            var res = new List<Managers.Type>();
            foreach (var type in Types.Where(t => t.Language == LocalizationManager.Instance.CurrentCulture.Name && (t.Name.StartsWith("Слоги") || t.Name.StartsWith("Склади"))))
                if (type.Name.ToLower().Split(new[] { ' ' })[2].IndexOf(keywords) >= 0)
                    res.Add(type);
            return res;
        }

        private void Build()
        {
            //Base1Buttons.RowDefinitions.Clear();
            //Base1Buttons.ColumnDefinitions.Clear();
            Base1Buttons.Children.Clear();

            var temp = Types.Where(t => t.Language == LocalizationManager.Instance.CurrentCulture.Name && t.BaseId == 0);
            //int count = temp.Count();
            //bool add = false;
            //Base1Buttons.RowDefinitions.Add(new RowDefinition());
            //for (int vl = 1; vl < count + 1; vl++)
            //{
            //    if (add)
            //        Base1Buttons.RowDefinitions.Add(new RowDefinition());
            //    if (vl % 4 == 0)
            //        add = true;
            //}
            //for (int vl = 0; vl < 4; vl++)
            //    Base1Buttons.ColumnDefinitions.Add(new ColumnDefinition());
            //int row = 0;
            //int column = 0;
            foreach (var t in temp)
            {
                var btn = new Button
                {
                    Style = Application.Current.Resources["ButtonStyle"] as Style,
                    Height = 40,
                    Width = 160,
                    Margin = new Thickness(3),
                    FontSize = 28,
                    Content = t.Name
                };
                btn.Click += (sender, e) =>
                    new Show1(t.Id).ShowDialog();
                //if (column == 4)
                //{
                //    column = 0;
                //    ++row;
                //}
                //Grid.SetColumn(btn, column++);
                //Grid.SetRow(btn, row);
                Base1Buttons.Children.Add(btn);
            }
        }

        private void BuildElse()
        {
            var temp = Types.Where(t => t.Language == LocalizationManager.Instance.CurrentCulture.Name && t.BaseId == 1);
            //int count = temp.Count();
            //bool add = false;
            //Base2Buttons.RowDefinitions.Add(new RowDefinition());
            //for (int vl = 1; vl < count + 1; vl++)
            //{
            //    if (add)
            //        Base2Buttons.RowDefinitions.Add(new RowDefinition());
            //    if (vl % 4 == 0)
            //        add = true;
            //}
            //for (int vl = 0; vl < 4; vl++)
            //    Base2Buttons.ColumnDefinitions.Add(new ColumnDefinition());
            //int row = 0;
            //int column = 0;
            foreach (var t in temp)
            {
                var btn = new Button
                {
                    Style = Application.Current.Resources["ButtonStyle"] as Style,
                    Height = 40,
                    Width = 160,
                    Margin = new Thickness(3),
                    FontSize = 28,
                    Content = t.Name
                };
                btn.Click += (sender, e) =>
                    new Show1(t.Id).ShowDialog();
                //if (column == 4)
                //{
                //    column = 0;
                //    row++;
                //}
                //Grid.SetColumn(btn, column++);
                //Grid.SetRow(btn, row);
                Base2Buttons.Children.Add(btn);
            }
        }

        private void BuildTemp()
        {
            //Base1Buttons.RowDefinitions.Clear();
            //Base1Buttons.ColumnDefinitions.Clear();
            Base1Buttons.Children.Clear();

            //int count = TempTypes.Count;
            //bool add = false;
            //Base1Buttons.RowDefinitions.Add(new RowDefinition());
            //for (int vl = 1; vl < count + 1; vl++)
            //{
            //    if (add)
            //        Base1Buttons.RowDefinitions.Add(new RowDefinition());
            //    if (vl % 4 == 0)
            //        add = true;
            //}
            //for (int vl = 0; vl < 4; vl++)
            //    Base1Buttons.ColumnDefinitions.Add(new ColumnDefinition());
            //int row = 0;
            //int column = 0;
            foreach (var t in TempTypes)
            {
                var btn = new Button
                {
                    Style = Application.Current.Resources["ButtonStyle"] as Style,
                    Height = 40,
                    Width = 180,
                    Margin = new Thickness(5),
                    FontSize = 28,
                    Content = t.Name
                };
                btn.Click += (sender, e) =>
                    new Show1(t.Id).ShowDialog();
                //if (column == 4)
                //{
                //    column = 0;
                //    row++;
                //}
                //Grid.SetColumn(btn, column++);
                //Grid.SetRow(btn, row);
                Base1Buttons.Children.Add(btn);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new LevelsPage());

        private void Base3Button_Click(object sender, RoutedEventArgs e)
        {
            new Show1("base1").ShowDialog();
        }

        private void Base4Button_Click(object sender, RoutedEventArgs e)
        {
            new Show1("base2").ShowDialog();
        }

        private void Base5Button_Click(object sender, RoutedEventArgs e)
        {
            new Show1("base3").ShowDialog();
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var obj = (StackPanel)sender;
            switch (LocalizationManager.Instance.CurrentCulture.Name)
            {
                case "ru-RU":
                    {
                        var ar = new[] { "у", "р", "о", "в", "е2", "н", "ь" };
                        foreach (var s in ar)
                        {
                            obj.Children.Add(new Image()
                            {
                                Source = new BitmapImage(new Uri(
                                    $"pack://application:,,,/Чтение;component/Resources/Letters/{s}.png"))
                            });
                        }
                        obj.Children.Add(new Image() { MinWidth = 70 });
                        obj.Children.Add(new Image()
                        {
                            Source = new BitmapImage(new Uri(
                                    $"pack://application:,,,/Чтение;component/Resources/Numbers/one.png"))
                        });
                        break;
                    }
                case "uk-UA":
                    {
                        var ar = new[] { "р", "і", "в", "е2", "н", "ь" };
                        foreach (var s in ar)
                        {
                            obj.Children.Add(new Image()
                            {
                                Source = new BitmapImage(new Uri(
                                    $"pack://application:,,,/Чтение;component/Resources/Letters/{s}.png"))
                            });
                        }
                        obj.Children.Add(new Image() { MinWidth = 70 });
                        obj.Children.Add(new Image()
                        {
                            Source = new BitmapImage(new Uri(
                                    $"pack://application:,,,/Чтение;component/Resources/Numbers/one.png"))
                        });
                        break;
                    }
            }
        }
    }
}
