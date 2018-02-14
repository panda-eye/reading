using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Чтение.Localization;
using Чтение.Managers;

namespace Чтение.Pages
{
    /// <summary>
    /// Логика взаимодействия для Show1.xaml
    /// </summary>
    public partial class Show1 : Window
    {
        private List<string> Syllables = new List<string>();
        private List<string> ReversedSyllables = new List<string>();
        private int CurrentWord = 1;
        bool small = true;
        private bool IsHand = false;
        private int CurrentHandFont = 1;
        private int CurrentFont = 1;
        private bool _reversed = false;
        private bool Reversed
        {
            get
            {
                return _reversed;
            }
            set
            {
                if (value == _reversed)
                    return;
                if (value)
                {
                    if (ReversedSyllables.Count > 0)
                    {
                        WordPlace.Text = ReversedSyllables[0];
                        CurrentWord = 1;
                        UpdateSyllPreview(ref ReversedSyllables, true);
                    }
                }
                else
                {
                    if (Syllables.Count > 0)
                    {
                        WordPlace.Text = Syllables[0];
                        CurrentWord = 1;
                        UpdateSyllPreview(ref Syllables, true);
                    }
                }
                _reversed = value;
                if (!small)
                    WordPlace.Text = WordPlace.Text.ToUpper();
            }
        }
        private Random r = new Random();
        private FontManager.HandwrittenFont.HandwrittenFontCollection CustomFonts = FontManager.Fonts;

        public Show1(object tag)
        {
            InitializeComponent();
            BackButton.Content = new Image()
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Чтение;component/Resources/Arrows/back.png"))
            };
            if (tag is string tg)
            {
                GenerateSyllables(tg);
            }
            else if (tag is int id)
            {
                GenerateSyllables(char.Parse(TypesClass.TypesLvl1.Where(t => t.Id == id).First().Name.Split(new[] { ' ' })[2].ToLower()));
            }
            Shuffle();
            WordPlace.Text = Syllables[0];
            UpdateSyllPreview(ref Syllables, true);
        }

        private void UpdateSyllPreview(ref List<string> sylls, bool first = false)
        {
            switch (sylls.Count)
            {
                case 1:
                    {
                        if (first)
                        {
                            SyllPreview.Items.Add("-");
                            SyllPreview.Items.Add("-");
                            SyllPreview.Items.Add(sylls[0]);
                            SyllPreview.Items.Add("-");
                            SyllPreview.Items.Add("-");
                        }
                        break;
                    }
                case 2:
                    {
                        SyllPreview.Items.Clear();
                        SyllPreview.Items.Add("-");
                        SyllPreview.Items.Add("-");
                        SyllPreview.Items.Add(sylls[CurrentWord == 1 ? 0 : 1]);
                        SyllPreview.Items.Add(sylls[CurrentWord == 1 ? 1 : 0]);
                        SyllPreview.Items.Add("-");
                        break;
                    }
                case 3:
                    {
                        SyllPreview.Items.Clear();
                        SyllPreview.Items.Add("-");
                        SyllPreview.Items.Add(sylls[CurrentWord == 1 ? 2 : (CurrentWord == 2 ? 0 : 1)]);
                        SyllPreview.Items.Add(sylls[CurrentWord == 1 ? 0 : (CurrentWord == 2 ? 1 : 2)]);
                        SyllPreview.Items.Add(sylls[CurrentWord == 1 ? 1 : (CurrentWord == 2 ? 2 : 0)]);
                        SyllPreview.Items.Add("-");
                        break;
                    }
                case 4:
                    {
                        SyllPreview.Items.Clear();
                        SyllPreview.Items.Add("-");
                        SyllPreview.Items.Add(sylls[CurrentWord == 1 ? 3 : (CurrentWord == 2 ? 0 : (CurrentWord == 3 ? 1 : 2))]);
                        SyllPreview.Items.Add(sylls[CurrentWord == 1 ? 0 : (CurrentWord == 2 ? 1 : (CurrentWord == 3 ? 2 : 3))]);
                        SyllPreview.Items.Add(sylls[CurrentWord == 1 ? 1 : (CurrentWord == 2 ? 2 : (CurrentWord == 3 ? 3 : 0))]);
                        SyllPreview.Items.Add(sylls[CurrentWord == 1 ? 2 : (CurrentWord == 2 ? 3 : (CurrentWord == 3 ? 0 : 1))]);
                        break;
                    }
                default:
                    {
                        var indexes = new[] { -1, -1, CurrentWord - 1, -1, -1 };
                        indexes[0] = CurrentWord >= 3 ? CurrentWord - 3 : (CurrentWord == 2 ? sylls.Count - 1 : sylls.Count - 2);
                        indexes[1] = CurrentWord >= 2 ? CurrentWord - 2 : sylls.Count - 1;
                        indexes[3] = CurrentWord < sylls.Count ? CurrentWord : 0;
                        indexes[4] = CurrentWord < sylls.Count - 1 ? CurrentWord + 1 : (CurrentWord == sylls.Count - 1 ? 0 : 1);

                        SyllPreview.Items.Clear();
                        SyllPreview.Items.Add(sylls[indexes[0]]);
                        SyllPreview.Items.Add(sylls[indexes[1]]);
                        SyllPreview.Items.Add(sylls[indexes[2]]);
                        SyllPreview.Items.Add(sylls[indexes[3]]);
                        SyllPreview.Items.Add(sylls[indexes[4]]);
                        break;
                    }
            }
        }

        private void GenerateSyllables(char c)
        {
            bool isvowel(char val, IOrderedEnumerable<char> vowels)
            {
                foreach (var v in vowels)
                {
                    if (v == val)
                        return true;
                }
                return false;
            }

            var set = new Properties.Settings();
            switch (LocalizationManager.Instance.CurrentCulture.Name)
            {
                case "ru-RU":
                    {
                        var consonants = "бвгджзклмнпрстфхцшйчщ".ToCharArray().OrderBy(t => t);
                        var vowels = "ауыэоеёюяиь".ToCharArray().OrderBy(t => t);
                        var mjagkie = "еёюяийчщь".ToCharArray().OrderBy(t => t);

                        bool ism(char val)
                        {
                            foreach (var m in mjagkie)
                                if (m == val)
                                    return true;
                            return false;
                        }

                        if (isvowel(c, vowels))
                        {
                            switch (c)
                            {
                                case 'ы':
                                    {
                                        foreach (var cons in consonants)
                                        {
                                            if (cons != 'ж' && cons != 'ш' && !ism(cons))
                                                Syllables.Add(cons.ToString() + c);
                                            ReversedSyllables.Add(c + cons.ToString());
                                        }
                                        break;
                                    }
                                case 'ь':
                                    {
                                        foreach (var cons in consonants)
                                        {
                                            if (cons != 'й')
                                                Syllables.Add(vowels.Where(cur => cur != 'ь').ElementAt(r.Next(vowels.Count() - 1)) + cons.ToString() + c);
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        bool isM = ism(c);
                                        foreach (var cons in consonants)
                                        {
                                            if (!isM && !ism(cons))
                                            {
                                                Syllables.Add(cons.ToString() + c);
                                                ReversedSyllables.Add(c + cons.ToString());
                                            }
                                            else if (isM)
                                            {
                                                if (cons != 'й' && c != 'ь')
                                                    Syllables.Add(cons.ToString() + c);
                                                if (c != 'ь')
                                                    ReversedSyllables.Add(c + cons.ToString());
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            switch (c)
                            {
                                case 'й':
                                    {

                                        foreach (var v in vowels.Where(tmp => !ism(tmp)))
                                        {
                                            ReversedSyllables.Add(v.ToString() + c);
                                            Syllables.Add(c + v.ToString());
                                        }
                                        break;
                                    }
                                case 'ж':
                                case 'ш':
                                    {
                                        foreach (var v in vowels.Where(tmp => !ism(tmp)))
                                        {
                                            ReversedSyllables.Add(v.ToString() + c);
                                            if (v != 'ы')
                                                Syllables.Add(c + v.ToString());
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        bool isM = ism(c);
                                        foreach (var v in vowels)
                                        {
                                            if (!isM && !ism(v))
                                            {
                                                ReversedSyllables.Add(v.ToString() + c);
                                                Syllables.Add(c + v.ToString());
                                            }
                                            else if (isM)
                                            {
                                                if (v != 'ь')
                                                    ReversedSyllables.Add(v.ToString() + c);
                                                if (v != 'ь')
                                                    Syllables.Add(c + v.ToString());
                                                else Syllables.Add($"{vowels.Where(cur => cur != 'ь').ElementAt(r.Next(vowels.Count() - 1))}{c}{v}");
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "uk-UA":
                    {
                        var mjagkie = "яюєіьї".ToCharArray().OrderBy(t => t);
                        var consonants = "бвгґджзклмнпрстфхцчшщй".ToCharArray().OrderBy(t => t);
                        var vowels = "яюєіьїаеиоу".ToCharArray().OrderBy(t => t);

                        bool ism(char val)
                        {
                            foreach (var m in mjagkie)
                                if (m == val)
                                    return true;
                            return false;
                        }

                        bool isM = ism(c);

                        if (isvowel(c, vowels))
                        {
                            switch (c)
                            {
                                case 'ь':
                                    {
                                        foreach (var cons in consonants)
                                        {
                                            if (cons != 'й')
                                                Syllables.Add($"{vowels.Where(cur => cur != 'ь').ElementAt(r.Next(vowels.Count() - 1))}{cons.ToString()}{c}");
                                        }
                                        break;
                                    }
                                case 'ї':
                                    {
                                        foreach (var cons in consonants)
                                        {
                                            Syllables.Add($"{c}{cons.ToString()}");
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        foreach (var cons in consonants)
                                        {
                                            if (!isM && !ism(cons))
                                            {
                                                if (cons != 'й')
                                                    Syllables.Add(cons.ToString() + c);
                                                ReversedSyllables.Add(c + cons.ToString());
                                            }
                                            else if (isM)
                                            {
                                                Syllables.Add(cons.ToString() + c);
                                                ReversedSyllables.Add(c + cons.ToString());
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            switch (c)
                            {
                                case 'й':
                                    {

                                        foreach (var v in vowels.Where(cur => !ism(cur)))
                                        {
                                            ReversedSyllables.Add(v.ToString() + c);
                                            Syllables.Add(c + v.ToString());
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        foreach (var v in vowels)
                                        {
                                            if (!isM && !ism(v))
                                            {
                                                ReversedSyllables.Add(v.ToString() + c);
                                                Syllables.Add(c + v.ToString());
                                            }
                                            else if (isM)
                                            {
                                                if (v != 'ь')
                                                    ReversedSyllables.Add(v.ToString() + c);
                                                if (v != 'ї')
                                                {
                                                    if (v != 'ь')
                                                        Syllables.Add(c + v.ToString());
                                                    else Syllables.Add($"{vowels.Where(cur => cur != 'ь').ElementAt(r.Next(vowels.Count() - 1))}{c}{v}");
                                                }
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    }
            }
        }

        private void GenerateSyllables(string bs)
        {
            var consonants = default(IOrderedEnumerable<char>);
            var vowels = default(IOrderedEnumerable<char>);
            var mjagkie = default(IOrderedEnumerable<char>);
            var b4 = default(IOrderedEnumerable<char>);
            var set = new Properties.Settings();
            string currentCulture = LocalizationManager.Instance.CurrentCulture.Name;

            bool isvowel(char val)
            {
                foreach (var v in vowels)
                    if (v == val)
                        return true;
                return false;
            }

            switch (currentCulture)
            {
                case "ru-RU":
                    {
                        mjagkie = "еёюяийчщь".ToCharArray().OrderBy(t => t);
                        consonants = "бвгджзклмнпрстфхцшйчщ".ToCharArray().OrderBy(t => t);
                        vowels = "ауыэоеёюяиь".ToCharArray().OrderBy(t => t);
                        b4 = "яюеёи".ToCharArray().OrderBy(t => t);
                        break;
                    }
                case "uk-UA":
                    {
                        mjagkie = "яюєіьї".ToCharArray().OrderBy(t => t);
                        consonants = "бвгґджзклмнпрстфхцчшщй".ToCharArray().OrderBy(t => t);
                        vowels = "яюєіьїаеиоу".ToCharArray().OrderBy(t => t);
                        b4 = "яюєї".ToCharArray().OrderBy(t => t);
                        break;
                    }
            }

            bool ism(char val)
            {
                foreach (var m in mjagkie)
                    if (m == val)
                        return true;
                return false;
            }

            switch (bs)
            {
                case "base1":
                    {
                        foreach (var cons in consonants)
                            foreach (var v in vowels)
                            {
                                if ((!ism(cons) && !ism(v)) && ((isvowel(v) && !isvowel(cons)) || (isvowel(cons) && !isvowel(v))))
                                {
                                    if (!(cons == 'ж' || cons == 'ш') || ((cons == 'ж' || cons == 'ш') && v != 'ы'))
                                        Syllables.Add(cons.ToString() + v);
                                    ReversedSyllables.Add(v.ToString() + cons);
                                }
                            }
                        break;
                    }
                case "base2":
                    {
                        foreach (var cons in consonants)
                            foreach (var v in vowels)
                            {
                                if (!(!ism(v) && !ism(cons))) //  && ((isvowel(v) && !isvowel(cons))) || (isvowel(cons) && !isvowel(v))
                                {
                                    if (v != 'ї')
                                    {
                                        if (v != 'ь')
                                            Syllables.Add(cons.ToString() + v);
                                        else Syllables.Add(vowels.Where(cur => cur != 'ь').ElementAt(r.Next(vowels.Count() - 1)) + cons.ToString() + v);
                                    }
                                    if (v != 'ь')
                                        ReversedSyllables.Add(v.ToString() + cons);
                                }
                            }
                        break;
                    }
                case "base3":
                    {
                        foreach (var cons in consonants)
                            foreach (var v in vowels)
                            {
                                /*if ((isvowel(v) && !isvowel(cons)) || (isvowel(cons) && !isvowel(v)))
                                {*/
                                if (cons != 'й')
                                {
                                    if (v != 'ї')
                                    {
                                        if (v != 'ь')
                                        {
                                            if (!(cons == 'ж' || cons == 'ш') || ((cons == 'ж' || cons == 'ш') && v != 'ы'))
                                                Syllables.Add(cons.ToString() + v);
                                        }
                                        else Syllables.Add(vowels.Where(cur => cur != 'ь').ElementAt(r.Next(vowels.Count() - 1)) + cons.ToString() + v);
                                    }
                                    if (v != 'ь')
                                        ReversedSyllables.Add(v.ToString() + cons);
                                }
                                //}
                            }
                        break;
                    }
                case "base4":
                    {
                        foreach (var cons in consonants)
                            foreach (var v in b4)
                            {
                                if (cons != 'й')
                                    Syllables.Add($"{cons}ь{v}");
                            }
                        break;
                    }
                case "base5":
                    {
                        foreach (var cons in consonants)
                            foreach (var v in b4)
                            {
                                if (cons != 'й')
                                    Syllables.Add($"{cons}{(currentCulture == "ru-RU" ? "ъ" : "'")}{v}");
                            }
                        break;
                    }
            }
        }

        private void Small_Click(object sender, RoutedEventArgs e)
        {
            if (!small)
            {
                small = true;
                WordPlace.Text = WordPlace.Text.ToLower();
            }
            else
            {
                small = false;
                WordPlace.Text = WordPlace.Text.ToUpper();
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (!Reversed)
            {
                if (CurrentWord == Syllables.Count)
                {
                    CurrentWord = 1;
                    WordPlace.Text = !small ? Syllables[CurrentWord - 1].ToUpper() : Syllables[CurrentWord - 1];
                }
                else
                    WordPlace.Text = !small ? Syllables[++CurrentWord - 1].ToUpper() : Syllables[++CurrentWord - 1];
                UpdateSyllPreview(ref Syllables);
            }
            else
            {
                if (CurrentWord == ReversedSyllables.Count)
                {
                    CurrentWord = 1;
                    WordPlace.Text = !small ? ReversedSyllables[CurrentWord - 1].ToUpper() : ReversedSyllables[CurrentWord - 1];
                }
                else
                    WordPlace.Text = !small ? ReversedSyllables[++CurrentWord - 1].ToUpper() : ReversedSyllables[++CurrentWord - 1];
                UpdateSyllPreview(ref ReversedSyllables);
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (!Reversed)
            {
                if (CurrentWord == 1)
                {
                    CurrentWord = Syllables.Count;
                    WordPlace.Text = !small ? Syllables[CurrentWord - 1].ToUpper() : Syllables[CurrentWord - 1];
                }
                else
                    WordPlace.Text = !small ? Syllables[--CurrentWord - 1].ToUpper() : Syllables[--CurrentWord - 1];
                UpdateSyllPreview(ref Syllables);
            }
            else
            {
                if (CurrentWord == 1)
                {
                    CurrentWord = ReversedSyllables.Count;
                    WordPlace.Text = !small ? ReversedSyllables[CurrentWord - 1].ToUpper() : ReversedSyllables[CurrentWord - 1];
                }
                else
                    WordPlace.Text = !small ? ReversedSyllables[--CurrentWord - 1].ToUpper() : ReversedSyllables[--CurrentWord - 1];
                UpdateSyllPreview(ref ReversedSyllables);
            }
        }
        
        private void Shuffle()
        {
            var rng = new Random();
            int n = Syllables.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = Syllables[k];
                Syllables[k] = Syllables[n];
                Syllables[n] = value;
            }

            n = ReversedSyllables.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = ReversedSyllables[k];
                ReversedSyllables[k] = ReversedSyllables[n];
                ReversedSyllables[n] = value;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetFont(FontFamily font)
        {
            WordPlace.FontFamily = font;
        }

        private void HandwrittenFont_Loaded(object sender, RoutedEventArgs e)
        {
            HandwrittenFont.FontFamily = CustomFonts[0].Font;
        }

        private void Reverse_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Tag as string)
            {
                case "back":
                    {
                        Reversed = true;
                        return;
                    }
                case "forward":
                    {
                        Reversed = false;
                        return;
                    }
            }
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
    }
}