using Microsoft.WindowsAPICodePack.Dialogs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Чтение.Localization;
using Чтение.Managers;

namespace Чтение.Pages
{
    public partial class ManagePage : Page
    {
        private int CurrentLevel { get; set; }
        private Managers.Type[] Types { get; set; }
        private int CurrentType { get; set; }
        private AsyncTableQuery<DbWord> Words { get; set; }
        private List<DbWord> NewWords = new List<DbWord>();
        private DbWord CurrentWord { get; set; }
        private bool WordsListOpened = false;
        private bool ButtonsOpened = false;
        private bool InfoOpened = false;

        private string NewWord { get => WordValue.Text; }
        private CommonOpenFileDialog Dialog = new CommonOpenFileDialog()
        {
            IsFolderPicker = true,
            Multiselect = false,
            Title = LocalizationManager.Instance.Localize("DialogTitle") as string
        };
        private bool editMode = false;
        private bool EditMode
        {
            get => editMode;
            set
            {
                if (editMode == value)
                    return;
                editMode = value;
                if (!editMode)
                {
                    add1.Visibility = Visibility.Hidden;
                    remove1.Visibility = Visibility.Hidden;
                    edit1.Visibility = Visibility.Hidden;
                    add0.Visibility = Visibility.Visible;
                    remove0.Visibility = Visibility.Visible;
                    edit0.Visibility = Visibility.Visible;
                    Dialog.ResetUserSelections();
                    WordValue.Text = "";
                    WordSource.Text = "";
                    Preview.Source = null;
                    NewWords.Clear();
                    UpdateWords();
                }
                else
                {
                    add1.Visibility = Visibility.Visible;
                    remove1.Visibility = Visibility.Visible;
                    edit1.Visibility = Visibility.Visible;
                    add0.Visibility = Visibility.Hidden;
                    remove0.Visibility = Visibility.Hidden;
                    edit0.Visibility = Visibility.Hidden;
                    WordValue.Text = "";
                    WordSource.Text = "";
                    Preview.Source = null;
                    UpdateWords();
                }
            }
        }

        public ManagePage()
        {
            InitializeComponent();
            if (LocalizationManager.Instance.CurrentCulture.Name.Equals("ru-RU"))
            {
                RuButton.Opacity = 0.4;
                RuButton.IsEnabled = false;
            }
            else
            {
                UkButton.Opacity = 0.4;
                UkButton.IsEnabled = false;
            }

            BackButton.Content = new Image()
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Чтение;component/Resources/Arrows/back.png"))
            };

            WordsList.MouseEnter += WordsList_MouseEnter;
            ButtonsGrid.MouseEnter += ButtonsGrid_MouseEnter;
            InfoGrid.MouseEnter += InfoGrid_MouseEnter;

            LocalizationManager.Instance.CultureChanged += Instance_CultureChanged;
            Loaded += (send, ev) =>
                NavigationService.Navigating += (sender, e) =>
                    LocalizationManager.Instance.CultureChanged -= Instance_CultureChanged;
        }
        private void Instance_CultureChanged(object sender, EventArgs e)
        {
            if (Dialog != null)
                Dialog.Title = LocalizationManager.Instance.Localize("DialogTitle") as string;

            UpdateTypes();
            WordsList.Items.Clear();
        }

        #region Mouse enter/leave
        private void InfoGrid_MouseLeave(object sender, MouseEventArgs e) =>
            CloseInfo();
        private void InfoGrid_MouseEnter(object sender, MouseEventArgs e) =>
            OpenInfo();

        private void ButtonsGrid_MouseLeave(object sender, MouseEventArgs e) =>
            CloseButtons();
        private void ButtonsGrid_MouseEnter(object sender, MouseEventArgs e) =>
            OpenButtons();

        private void WordsList_MouseLeave(object sender, MouseEventArgs e) =>
            CloseList();
        private void WordsList_MouseEnter(object sender, MouseEventArgs e) =>
            OpenList();
        #endregion

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
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Tag as string)
            {
                case "uk":
                    {
                        AnimationManager.ApplyDefaultLanguageAnimation(UkButton, RuButton, () =>
                        {
                            LocalizationManager.Instance.CurrentCulture = new CultureInfo("uk-UA");
                            new Properties.Settings() { DefaultLocalization = "uk-UA" }.Save();
                        });
                        break;
                    }
                case "ru":
                    {
                        AnimationManager.ApplyDefaultLanguageAnimation(RuButton, UkButton, () =>
                        {
                            LocalizationManager.Instance.CurrentCulture = new CultureInfo("ru-RU");
                            new Properties.Settings() { DefaultLocalization = "ru-RU" }.Save();
                        });
                        break;
                    }
                case "add1":
                    {
                        if (!string.IsNullOrWhiteSpace(NewWord) && !string.IsNullOrWhiteSpace(WordSource.Text))
                        {
                            if ((await Words.ToListAsync()).Where(w => w.Value.ToLower() == NewWord.ToLower()).Count() < 1)
                            {
                                NewWords[WordsList.SelectedIndex].Value = NewWord;
                                await Application.Words.Connection.InsertAsync(NewWords[WordsList.SelectedIndex], typeof(DbWord));
                                if (!File.Exists(Path.Combine(Application.ImagesPath, LocalizationManager.Instance.CurrentCulture.Name, CurrentWord.Source)))
                                    new FileInfo(Path.Combine(Dialog.FileName, CurrentWord.Source)).CopyTo(Path.Combine(Application.ImagesPath, LocalizationManager.Instance.CurrentCulture.Name, CurrentWord.Source));
                            }
                            WordValue.Text = "";
                            if (NewWords.Count == WordsList.SelectedIndex + 1)
                                EditMode = false;
                            else
                                WordsList.SelectedIndex++;
                        }
                        break;
                    }
                case "remove0":
                    {
                        if (CurrentWord != null)
                        {
                            await Application.Words.Connection.DeleteAsync(new DbWord() { Id = CurrentWord.Id });
                            UpdateWords();
                            WordValue.Text = "";
                            WordSource.Text = "";
                        }
                        break;
                    }
                case "add0":
                    {
                        if (CurrentWord != null && NewWord != CurrentWord.Value)
                        {
                            await Application.Words.Connection.InsertOrReplaceAsync(new DbWord() { Id = CurrentWord.Id, TypeId = CurrentWord.TypeId, Value = NewWord,
                                Source = CurrentWord.Source, Language = CurrentWord.Language });
                            UpdateWords();
                        }
                        break;
                    }
                case "remove1":
                    {
                        if (await Words.CountAsync() == WordsList.SelectedIndex + 1)
                            EditMode = false;
                        else
                            WordsList.SelectedIndex += 1;
                        break;
                    }
                case "folder":
                    {
                        var d = Dialog.ShowDialog();
                        if (d == CommonFileDialogResult.Ok)
                            EditMode = true;
                        break;
                    }
                case "cancel":
                    {
                        EditMode = false;
                        break;
                    }
            }
        }

        #region Open/close
        private void OpenList()
        {
            if (ButtonsOpened)
                return;
            if (InfoOpened)
                return;
            WordsList.MouseEnter -= WordsList_MouseEnter;
            AnimationManager.ApplyMarginAnimation(WordsList, new Thickness(0, 20, 0, 20), () =>
            {
                WordsListOpened = true;
            });
            WordsList.MouseLeave += WordsList_MouseLeave;
        }
        private void CloseList()
        {
            WordsList.MouseLeave -= WordsList_MouseLeave;
            AnimationManager.ApplyMarginAnimation(WordsList, new Thickness(-270, 20, 0, 20), () =>
            {
                WordsListOpened = false;
            }, true);
            WordsList.MouseEnter += WordsList_MouseEnter;
        }

        private void OpenButtons()
        {
            if (InfoOpened)
                return;
            if (WordsListOpened)
                return;
            ButtonsGrid.MouseEnter -= ButtonsGrid_MouseEnter;
            AnimationManager.ApplyMarginAnimation(ButtonsGrid, new Thickness(40, 0, 40, 0), () =>
            {
                ButtonsOpened = true;
            });
            ButtonsGrid.MouseLeave += ButtonsGrid_MouseLeave;
        }
        private void CloseButtons()
        {
            ButtonsGrid.MouseLeave -= ButtonsGrid_MouseLeave;
            AnimationManager.ApplyMarginAnimation(ButtonsGrid, new Thickness(40, 0, 40, -20), () =>
            {
                ButtonsOpened = false;
            }, true);
            ButtonsGrid.MouseEnter += ButtonsGrid_MouseEnter;
        }

        private void OpenInfo()
        {
            if (WordsListOpened)
                return;
            if (ButtonsOpened)
                return;
            InfoGrid.MouseEnter -= InfoGrid_MouseEnter;
            AnimationManager.ApplyMarginAnimation(InfoGrid, new Thickness(40, 0, 40, 0), () =>
            {
                InfoOpened = true;
            });
            InfoGrid.MouseLeave += InfoGrid_MouseLeave;
        }
        private void CloseInfo()
        {
            InfoGrid.MouseLeave -= InfoGrid_MouseLeave;
            AnimationManager.ApplyMarginAnimation(InfoGrid, new Thickness(40, -20, 40, 0), () =>
            {
                InfoOpened = false;
            }, true);
            InfoGrid.MouseEnter += InfoGrid_MouseEnter;
        }
        #endregion

        private async void WordsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WordsListOpened)
                CloseList();

            if (WordsList.SelectedIndex >= 0 && WordsList.Items.Count > 0)
            {
                if (!EditMode)
                    CurrentWord = (await Words.ToArrayAsync())[WordsList.SelectedIndex];
                else
                    CurrentWord = NewWords[WordsList.SelectedIndex];
                UpdateCurrent();
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ComboBox;
            if (box.SelectedIndex == -1)
                return;
            switch (box.Tag as string)
            {
                case "level":
                    {
                        CurrentLevel = box.SelectedIndex + 2;
                        UpdateTypes();
                        WordsList.Items.Clear();
                        break;
                    }
                case "type":
                    {
                        CurrentType = Types[box.SelectedIndex].Id;
                        UpdateWords();
                        CloseInfo();
                        break;
                    }
            }
        }

        private void UpdateTypes()
        {
            if (!EditMode && CurrentLevel != 0)
            {
                TypesComboBox.Items.Clear();
                CurrentWord = null;
                switch (CurrentLevel)
                {
                    case 1:
                        {
                            Types = TypesClass.TypesLvl1;
                            break;
                        }
                    case 2:
                        {
                            Types = TypesClass.TypesLvl2;
                            break;
                        }
                    case 3:
                        {
                            Types = TypesClass.TypesLvl3;
                            break;
                        }
                    case 4:
                        {
                            Types = TypesClass.TypesLvl4;
                            break;
                        }
                    case 5:
                        {
                            Types = TypesClass.TypesLvl5;
                            break;
                        }
                    case 6:
                        {
                            Types = TypesClass.TypesLvl6;
                            break;
                        }
                }
                foreach (var type in Types.Where(t => t.Language == LocalizationManager.Instance.CurrentCulture.Name))
                    TypesComboBox.Items.Add(type.Name);
                TypesComboBox.SelectedIndex = -1;
            }
        }
        private async void UpdateWords()
        {
            WordsList.Items.Clear();
            if (!EditMode)
            {
                Words = Application.Words.Connection.Table<DbWord>().Where(w => w.TypeId == CurrentType && w.Language == LocalizationManager.Instance.CurrentCulture.Name);
                if (await Words.CountAsync() > 0)
                {
                    foreach (var word in await Words.ToArrayAsync())
                        WordsList.Items.Add(word.Value);
                    WordsList.SelectedIndex = 0;
                }
                else
                    CurrentWord = null;
            }
            else
            {
                var files = new[] { "*.jpg", "*.png", "*.jpeg" }.SelectMany(pattern => new DirectoryInfo(Dialog.FileName).GetFiles(pattern, SearchOption.TopDirectoryOnly));
                if (files.Count() == 0)
                    MessageBox.Show("UpdateWords() not found error");
                else
                    AddNewWords(files);
            }
        }
        private void UpdateCurrent()
        {
            if (CurrentWord != null)
            {
                if (EditMode)
                {
                    Preview.Source = new BitmapImage(new Uri(Path.Combine(Dialog.FileName, CurrentWord.Source)));
                    WordSource.Text = CurrentWord.Source;
                    WordValue.Text = CurrentWord.Source.Split(new[] { '.' })[0];
                }
                else
                {
                    try
                    {
                        Preview.Source = new BitmapImage(new Uri(Path.Combine(Application.ImagesPath, LocalizationManager.Instance.CurrentCulture.Name, CurrentWord.Source)));
                    } catch { }
                    WordValue.Text = CurrentWord.Value;
                    WordSource.Text = CurrentWord.Source;
                }
            }
        }

        private void AddNewWords(IEnumerable<FileInfo> fi)
        {
            foreach (var f in fi)
            {
                NewWords.Add(new DbWord(null, f.Name, CurrentType));
                WordsList.Items.Add(f.Name);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private async void Copy_Click(object sender, RoutedEventArgs e)
        {
            string desktop = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Reading");
            string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Reading");
            await Application.Words.Close();
            if (Directory.Exists(desktop))
                Directory.Delete(desktop, true);
            
            foreach (string dirPath in Directory.GetDirectories(appData, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(appData, desktop));
            }
            
            foreach (string newPath in Directory.GetFiles(appData, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    File.Copy(newPath, newPath.Replace(appData, desktop), true);
                }
                catch { }
            }

            if (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) != appData)
            {
                try
                {
                    new FileInfo(Assembly.GetExecutingAssembly().Location).CopyTo(Path.Combine(desktop, "Чтение.exe"));
                    new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "WordsDataBase.db")).CopyTo(Path.Combine(desktop, "WordsDataBase.db"));
                }
                catch { }
            }
            Application.Words = new WordsDataBaseManager();
            MessageBox.Show(LocalizationManager.Instance.Localize("MBoxCopy") as string);
        }
    }
}