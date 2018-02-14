/*using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Чтение.Localization;

namespace Чтение.Managers
{
    public static class UpdateManager
    {
        private const double CurrentAppVersion = 1.1;
        private static string AppPath { get; set; }
        public async static void Check(Application app)
        {
            UpdateModel converted = new UpdateModel();
            bool error = false;
            var set = new Properties.Settings();
            if (!set.UpdateAvailable)
            {
                using (var http = new HttpClient())
                {
                    try
                    {
                        string result = await http.GetStringAsync("https://uhwl20dyfc.execute-api.us-east-2.amazonaws.com/reading/getLastVersion");
                        converted = JsonConvert.DeserializeObject<UpdateModel>(result);
                    }
                    catch { error = true; }
                }
            }
            else converted.Version = set.NewAppVersion;

            if (!error)
            {
                AppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Reading", "Обновление.exe");
                if (converted.UpdaterVersion > set.UpdaterVersion)
                {
                    await Updater();
                    set.UpdaterVersion = converted.UpdaterVersion;
                }
                if (converted.Version > CurrentAppVersion)
                {
                    var cmnd = MessageBox.Show(L("NewVersion"), L("NewVersionTitle"), MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (cmnd == MessageBoxResult.Yes)
                    {
                        app.Exit += App_Exit;
                        set.UpdateAvailable = false;
                        set.Save();
                        app.Shutdown();
                    }
                    else
                    {
                        set.NewAppVersion = converted.Version;
                        set.UpdateAvailable = true;
                        set.Save();
                    }
                }
            }
        }
        private static void App_Exit(object sender, ExitEventArgs e)
        {
            new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Verb = "Open",
                    FileName = AppPath
                }
            }.Start();
        }
        private static string L(string key) =>
            LocalizationManager.Instance.Localize(key) as string;
        private async static Task Updater()
        {
            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    await wc.DownloadFileTaskAsync("https://s3.eu-central-1.amazonaws.com/reading-bucket/%D0%9E%D0%B1%D0%BD%D0%BE%D0%B2%D0%BB%D0%B5%D0%BD%D0%B8%D0%B5.exe", AppPath);
                }
            }
            catch { }
        }
    }
}*/