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
using System.Windows.Shapes;
using Чтение.Localization;
using Чтение.Managers;

namespace Чтение.Pages
{
    /// <summary>
    /// Логика взаимодействия для Level2.xaml
    /// </summary>
    public partial class Level2 : Page
    {
        public Level2()
        {
            InitializeComponent();
            LocalizationManager.Instance.CultureChanged += Instance_CultureChanged;
            Loaded += (sender, e) =>
                NavigationService.Navigating += NavigationService_Navigating;

            BackButton.Content = new Image()
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Чтение;component/Resources/Arrows/back.png"))
            };

            var temp = TypesClass.TypesLvl2.Where(t => t.Language == CurrentLanguage);
            first.Content = temp.First().Name;
            first.Tag = temp.First().Id;
            first.Click += (sender, e) =>
                new Show((sender as Button).Tag).ShowDialog();
            second.Content = temp.Skip(1).First().Name;
            second.Tag = temp.Skip(1).First().Id;
            second.Click += (sender, e) =>
                new Show((sender as Button).Tag).ShowDialog();
            third.Content = temp.Skip(2).First().Name;
            third.Tag = temp.Skip(2).First().Id;
            third.Click += (sender, e) =>
                new Show((sender as Button).Tag).ShowDialog();
        }

        private void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e) =>
            LocalizationManager.Instance.CultureChanged -= Instance_CultureChanged;

        private void Instance_CultureChanged(object sender, EventArgs e)
        {
            var temp = TypesClass.TypesLvl2.Where(t => t.Language == CurrentLanguage);
            first.Content = temp.First().Name;
            first.Tag = temp.First().Id;
            second.Content = temp.Skip(1).First().Name;
            second.Tag = temp.Skip(1).First().Id;
            third.Content = temp.Skip(2).First().Name;
            third.Tag = temp.Skip(2).First().Id;
        }

        private string CurrentLanguage { get => LocalizationManager.Instance.CurrentCulture.Name; }

        private void Button_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new LevelsPage());

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
                                    $"pack://application:,,,/Чтение;component/Resources/Numbers/two.png"))
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
                                    $"pack://application:,,,/Чтение;component/Resources/Numbers/two.png"))
                        });
                        break;
                    }
            }
        }
    }
}
