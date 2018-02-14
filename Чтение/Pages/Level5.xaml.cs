using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Чтение.Localization;
using Чтение.Managers;

namespace Чтение.Pages
{
    /// <summary>
    /// Логика взаимодействия для Level5.xaml
    /// </summary>
    public partial class Level5 : Page
    {
        public Level5()
        {
            InitializeComponent();
            //LocalizationManager.Instance.CultureChanged += Instance_CultureChanged;
            //Loaded += (sender, e) =>
            //    NavigationService.Navigating += NavigationService_Navigating;

            BackButton.Content = new Image()
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Чтение;component/Resources/Arrows/back.png"))
            };

            var temp = TypesClass.TypesLvl5.Where(t => t.Language == CurrentLanguage);
            first.Content = temp.First().Name;
            first.Tag = temp.First().Id;
            first.Click += (sender, e) =>
                new Show((sender as Button).Tag).ShowDialog();
            second.Content = new TextBlock
            {
                Text = temp.Skip(1).First().Name,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };
            second.Tag = temp.Skip(1).First().Id;
            second.Click += (sender, e) =>
                new Show((sender as Button).Tag).ShowDialog();
            third.Content = temp.Skip(2).First().Name;
            third.Tag = temp.Skip(2).First().Id;
            third.Click += (sender, e) =>
                new Show((sender as Button).Tag).ShowDialog();
            fourth.Content = temp.Skip(3).First().Name;
            fourth.Tag = temp.Skip(3).First().Id;
            fourth.Click += (sender, e) =>
                new Show((sender as Button).Tag).ShowDialog();
            fifth.Content = new TextBlock
            {
                Text = temp.Skip(4).First().Name,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };
            fifth.Tag = "base4";
            fifth.Click += (sender, e) =>
                new Show1((sender as Button).Tag).ShowDialog();
            sixth.Content = new TextBlock
            {
                Text = temp.Skip(5).First().Name,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };
            sixth.Tag = "base5";
            sixth.Click += (sender, e) =>
                new Show1((sender as Button).Tag).ShowDialog();
        }

        private string CurrentLanguage { get => LocalizationManager.Instance.CurrentCulture.Name; }

        private void Button_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new LevelsPage());

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var obj = (StackPanel)sender;
            var ar = LocalizationManager.Instance.CurrentCulture.Name == "ru-RU" ? new[] { "у", "р", "о", "в", "е2", "н", "ь" } :
                new[] { "р", "і", "в", "е2", "н", "ь" };
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
                        $"pack://application:,,,/Чтение;component/Resources/Numbers/five.png"))
            });
        }
    }
}
