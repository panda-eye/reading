using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Чтение.Localization;
using Чтение.Managers;

namespace Чтение.Pages
{
    public partial class Show : Window
    {
        private ImageBrush TempPlace { get; set; }
        private FontManager.HandwrittenFont.HandwrittenFontCollection CustomFonts = FontManager.Fonts;
        private int CurrentFont = 1;
        private int CurrentHandFont = 1;
        private bool IsHand = false;
        private DbWord[] CurrentWords { get; set; }
        private WordsTable CurrentTable { get; }
        bool small = true;

        public Show(object tag)
        {
            InitializeComponent();
            UnderImage.Background = new ImageBrush()
            {
                ImageSource = new Image()
                {
                    Source = new BitmapImage(
                            new Uri("pack://application:,,,/Чтение;component/Resources/over_place.png"))
                }.Source
            };
            BackButton.Content = new Image()
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Чтение;component/Resources/Arrows/back.png"))
            };
            TempPlace = new ImageBrush()
            {
                ImageSource = new Image()
                {
                    Source = new BitmapImage(
                        new Uri("pack://application:,,,/Чтение;component/Resources/place.jpg"))
                }.Source
            };
            ImagePlace.Background = TempPlace;
            CurrentTable = new WordsTable();
            ForWords.Child = CurrentTable;
            InputLanguageManager.SetInputLanguage(WordInput, CultureInfo.CreateSpecificCulture("ru"));
            Loaded += (sender, e) =>
                Update((int)tag);
        }

        private async void Update(int typeId)
        {
            Words.Items.Clear();
            try
            {
                CurrentWords = await Application.Words.Connection.Table<DbWord>().Where(word => word.TypeId == typeId).ToArrayAsync();
                if (CurrentWords.Count() > 0)
                {
                    foreach (var word in CurrentWords.OrderBy(w => w.Value.ToLower()))
                        Words.Items.Add(word.Value);
                    Words.SelectedIndex = 0;
                }
                else
                    CurrentWords = null;
            }
            catch { CurrentWords = null; }
        }

        private void UsualFont_Click(object sender, RoutedEventArgs e)
        {
            if (!IsHand)
            {
                switch (CurrentFont)
                {
                    case 1:
                        {
                            CurrentFont = 2;
                            SetFont(new FontFamily("Times New Roman"));
                            break;
                        }
                    case 2:
                        {
                            CurrentFont = 3;
                            SetFont(new FontFamily("Calibri"));
                            break;
                        }
                    case 3:
                        {
                            CurrentFont = 1;
                            SetFont(new FontFamily("Trebushet MS"));
                            break;
                        }
                }
            }
            else
            {
                IsHand = false;
                switch (CurrentFont)
                {
                    case 1:
                        {
                            SetFont(new FontFamily("Trebushet MS"));
                            break;
                        }
                    case 2:
                        {
                            SetFont(new FontFamily("Times New Roman"));
                            break;
                        }
                    case 3:
                        {
                            SetFont(new FontFamily("Calibri"));
                            break;
                        }
                }
            }
        } // Done

        private void HandwrittenFont_Click(object sender, RoutedEventArgs e)
        {
            if (IsHand)
            {
                if (CurrentHandFont == CustomFonts.Fonts.Count)
                {
                    CurrentHandFont = 1;
                }
                else
                    CurrentHandFont++;
                SetFont(CustomFonts[CurrentHandFont - 1].Font);
            }
            else
            {
                IsHand = true;
                SetFont(CustomFonts[CurrentHandFont - 1].Font);
            }
        } // Done

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            Words.IsDropDownOpen = true;
        } // Done

        private void Big_Click(object sender, RoutedEventArgs e)
        {
            //if (CurrentTable != null)
            //    CurrentTable.ToUpper();
            //    foreach (var tb in CurrentTable.Children.OfType<Grid>().Select(it => it.Children.OfType<Viewbox>().First()).Select(it => it.Child as TextBlock))
            //        tb.Text = tb.Text.ToUpper();
        } // Done

        private void Small_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTable != null)
            {
                if (!small)
                {
                    small = true;
                    CurrentTable.ToLower();
                }
                else
                {
                    small = false;
                    CurrentTable.ToUpper();
                }

            }
        } // Done

        private void SetFont(FontFamily font)
        {
            if (CurrentTable != null)
                CurrentTable.FontFamily = font;
                //foreach (var tb in CurrentTable.Children.OfType<Grid>().Select(it => it.Children.OfType<Viewbox>().First()).Select(it => it.Child as TextBlock))
                //    tb.FontFamily = font;
        } // Done

        private void Words_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Words.SelectedIndex != -1)
            {
                CurrentTable.T(CurrentWords[Words.SelectedIndex].Value);
                if (!small) CurrentTable.ToUpper();
            }
        }

        private void ImagePlace_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                ImagePlace.Background = new ImageBrush()
                {
                    ImageSource = new Image()
                    {
                        Source = new BitmapImage(
                            new Uri(Path.Combine(Application.ImagesPath, LocalizationManager.Instance.CurrentCulture.Name, CurrentWords[Words.SelectedIndex].Source)))
                    }.Source
                };
            }
            catch { }
        }

        private void ImagePlace_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ImagePlace.Background = TempPlace;
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (Words.SelectedIndex == 0)
                Words.SelectedIndex = CurrentWords.Length - 1;
            else
                if (Words.SelectedIndex != -1)
                    Words.SelectedIndex--;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentWords != null)
            {
                if (Words.SelectedIndex == CurrentWords.Length - 1)
                    Words.SelectedIndex = 0;
                else
                    Words.SelectedIndex++;
            }
        }

        private void HandwrittenFont_Loaded(object sender, RoutedEventArgs e)
        {
            HandwrittenFont.FontFamily = CustomFonts[0].Font;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}