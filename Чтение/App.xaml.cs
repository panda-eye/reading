using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Чтение.Localization;
using Чтение.Managers;

namespace Чтение
{
    public partial class Application : System.Windows.Application
    {
        internal static WordsDataBaseManager Words { get; set; }
        internal static string ImagesPath { get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Reading", "Words"); }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CheckInstance();

            var set = new Properties.Settings();
            LocalizationManager.Instance.LocalizationProvider = new ResxLocalizationProvider();

            //new Thread(() => UpdateManager.Check(this)).Start();
            CreateFolders();
            
            Words = new WordsDataBaseManager();
            Exit += App_Exit;
            
            if (string.IsNullOrEmpty(set.DefaultLocalization))
                StartupUri = new Uri("Language.xaml", UriKind.Relative);
            else
            {
                var wind = new Nav
                {
                    Style = Current.Resources["WindowStyle"] as Style,
                    Background = new ImageBrush()
                    {
                        ImageSource = new Image()
                        {
                            Source = new BitmapImage(new Uri("pack://application:,,,/Чтение;component/Resources/backgr.jpg"))
                        }.Source
                    }
                };
                wind.Navigate(new MainPage());
                wind.Show();
                
                if (set.DefaultLocalization != "ru-RU")
                    LocalizationManager.Instance.CurrentCulture = new CultureInfo(set.DefaultLocalization);
            }
        }

        private async void App_Exit(object sender, ExitEventArgs e)
        {
            await Words?.Close();
        }

        /// <summary>
        /// Если один экземпляр программы уже открыт, закрывает новый и выполняет фокусировку на открытом старом экземпляре.
        /// </summary>
        private void CheckInstance()
        {
            var cur = Process.GetCurrentProcess();
            var proc = Process.GetProcessesByName(cur.ProcessName).Where(pr => pr.Id != cur.Id).FirstOrDefault();
            if (proc != default(Process))
            {
                SwitchToThisWindow(proc.MainWindowHandle, true);
                Shutdown();
            }
        }

        /// <summary>
        /// Фокусирует на окне.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        
        private void CreateFolders()
        {
            new DirectoryInfo(Path.Combine(ImagesPath, "ru-RU")).Create();
            new DirectoryInfo(Path.Combine(ImagesPath, "uk-UA")).Create();
        }
    }
}
