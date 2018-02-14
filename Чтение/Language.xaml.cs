using MahApps.Metro;
using MahApps.Metro.Controls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Чтение.Localization;
using Чтение.Managers;

namespace Чтение
{
    /// <summary>
    /// Логика взаимодействия для Language.xaml
    /// </summary>
    public partial class Language : Window
    {
        public Language()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = (Grid)sender;
            var height = (ActualHeight - grid.ActualHeight) / 2;
            ThicknessAnimation margin = new ThicknessAnimation(new Thickness(0, height - 50, 0, height + 50), new Thickness(0), new Duration(new TimeSpan(0, 0, 3)), FillBehavior.HoldEnd);
            DoubleAnimation opacity = new DoubleAnimation(1, new Duration(new TimeSpan(0, 0, 3)), FillBehavior.HoldEnd);
            grid.BeginAnimation(MarginProperty, margin);
            grid.BeginAnimation(OpacityProperty, opacity);
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            var grid = (Grid)sender;
            switch (grid.Tag)
            {
                case "1":
                    {
                        if (LocalizationManager.Instance.CurrentCulture.Name.Equals("ru-RU"))
                            break;
                        ApplyAnimation("ru-RU");
                        break;
                    }
                case "2":
                    {
                        if (LocalizationManager.Instance.CurrentCulture.Name.Equals("uk-UA"))
                            break;
                        ApplyAnimation("uk-UA");
                        break;
                    }
            }
        }

        private void ApplyAnimation(string cultureInfo)
        {
            DoubleAnimation opacity = new DoubleAnimation(0, new Duration(new TimeSpan(0, 0, 0, 0, 300)), FillBehavior.HoldEnd);
            opacity.Completed += (send, ev) =>
            {
                LocalizationManager.Instance.CurrentCulture = new CultureInfo(cultureInfo);
                var backOpacity = new DoubleAnimation(1, new Duration(new TimeSpan(0, 0, 0, 0, 300)), FillBehavior.HoldEnd);
                LanguageText.BeginAnimation(OpacityProperty, backOpacity);
            };
            LanguageText.BeginAnimation(OpacityProperty, opacity);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var set = new Properties.Settings();
            switch (((Button)sender).Tag)
            {
                case "ru":
                    {
                        set.DefaultLocalization = "ru-RU";
                        break;
                    }
                case "uk":
                    {
                        set.DefaultLocalization = "uk-UA";
                        break;
                    }
            }
            set.Save();
            var wind = new Nav
            {
                Style = Application.Current.Resources["WindowStyle"] as Style,
                Background = new ImageBrush()
                {
                    ImageSource = new Image()
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Чтение;component/Resources/backgr.jpg"))
                    }.Source
                }
            };
            //ThemeManager.ChangeAppStyle(this, ThemeManager.GetAccent("Blue"), ThemeManager.GetAppTheme("BaseLight"));
            wind.Navigate(new MainPage());
            wind.Show();
            Close();
        }
    }
}
