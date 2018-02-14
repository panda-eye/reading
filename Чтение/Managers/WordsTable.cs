using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Чтение.Managers
{
    public class WordsTable : TextBlock
    {
        internal WordSplitter Splitter { get; set; }
        private int CurrentColor = 1;
        private AdornerLayer Layer { get; set; }
        private List<Dot> Dots { get; }

        public WordsTable() : base()
        {
            Margin = new Thickness(0, 5, 0, 5);
            Dots = new List<Dot>();
            Loaded += (sender, e) => Layer = AdornerLayer.GetAdornerLayer(this);
        }

        public WordsTable(Inline inline) : base(inline) { }

        public void T(string word)
        {
            Splitter = new WordSplitter(word);
            Dots.ForEach(it => Layer.Remove(it));
            Inlines.Clear();
            Dots.Clear();
            InitializeComponent();
            AddAdorners();
        }

        public void ToUpper() =>
            new List<Run>(Inlines.Select(it => it as Run)).FirstOrDefault(it => { it.Text = it.Text.ToUpper(); return false; });

        public void ToLower()
        {
            for (int vl = 0; vl < Splitter?.Blocks.Length; vl++)
                (Inlines.ElementAt(vl) as Run).Text = Splitter.Blocks[vl].Block;
        }

        private void InitializeComponent()
        {
            foreach (var block in Splitter.Blocks)
            {
                Brush brush = null;
                switch (CurrentColor)
                {
                    case 1:
                        {
                            brush = Brushes.Blue;
                            CurrentColor++;
                            break;
                        }
                    case 2:
                        {
                            brush = Brushes.Red;
                            CurrentColor++;
                            break;
                        }
                    case 3:
                        {
                            brush = Brushes.Black;
                            CurrentColor++;
                            break;
                        }
                    case 4:
                        {
                            brush = Brushes.Green;
                            CurrentColor = 1;
                            break;
                        }
                }
                var run = new Run(block.Block)
                {
                    Foreground = brush
                };
                Inlines.Add(run);
                //var dot = new Dot(this, run, brush);
                //Layer.Add(dot);
                //Dots.Add(dot);
            }
        }

        private void AddAdorners()
        {
            for (int vl = 0; vl < Splitter.Blocks.Length; vl++)
            {
                if (Splitter.Blocks[vl].IsDot)
                {
                    var run = Inlines.ElementAt(vl) as Run;
                    var dot = new Dot(this, run, run.Foreground);
                    Layer.Add(dot);
                    Dots.Add(dot);
                }
            }
        }
    }

    public class WordSplitter
    {
        private string Word { get; set; }
        public WordSplitterBlock[] Blocks { get; }
        public WordSplitter(string word)
        {
            Word = word;
            Blocks = GenerateBlocks();
        }

        private WordSplitterBlock[] GenerateBlocks()
        {
            var temp = new List<WordSplitterBlock>();

            foreach (string s in SplitWord(Word).Split(new[] { '_' }))
                temp.Add(new WordSplitterBlock(s));

            return temp.ToArray();
        }

        //private string SplitWord(string word)
        //{
        //    //word = word.ToLower();
        //    var consonants = new[] { 'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л',
        //        'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ',
        //        'ь', 'ъ' };
        //    //var thud = new[] { 'к', 'п', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ' };
        //    var vowels = new[] { 'а', 'у', 'о', 'ы', 'и', 'э', 'я', 'ю', 'ё', 'е', 'і', 'є', 'ї' };

        //    int vowelcount()
        //    {
        //        if (word.Length == 0)
        //            return 0;
        //        int cnt = 0;
        //        for (int i = 0; i < word.Length; i++)
        //        {
        //            foreach (char c in vowels)
        //            {
        //                if (c == char.ToLower(word[i]))
        //                    cnt++;
        //            }
        //        }
        //        return cnt;
        //    } // ok

        //    bool isconsonant(char val)
        //    {
        //        var rez = false;
        //        foreach (char c in consonants)
        //        {
        //            if (c == val)
        //            {
        //                rez = true;
        //                break;
        //            }
        //        }
        //        return rez;
        //    }

        //    #region Comments
        //    //bool isthud(char val)
        //    //{
        //    //    char x = char.ToLower(val);
        //    //    for (int t = 0; t < thud.Length; t++)
        //    //    {
        //    //        if (thud[t] == x)
        //    //            return true;
        //    //    }
        //    //    return false;
        //    //}

        //    //bool ismnrl(char val)
        //    //{
        //    //    return (val == 'м' || val == 'н' || val == 'р' || val == 'л' ||
        //    //        val == 'М' || val == 'Н' || val == 'Р' || val == 'Л');
        //    //}
        //    #endregion

        //    bool isvowel(char val)
        //    {
        //        var rez = false;
        //        for (int v = 0; v < vowels.Length; v++)
        //        {
        //            if (vowels[v] == val)
        //            {
        //                rez = true;
        //                break;
        //            }
        //        }
        //        return rez;
        //    }

        //    void split2syllables()
        //    {
        //        if (word.Length < 3)
        //            return;
        //        bool minus;
        //        var splited = "";
        //        char n;
        //        char c;
        //        int i = word.Length - 1;
        //        int scount = vowelcount();
        //        if (scount == 1)
        //            return;

        //        while (i >= 1)
        //        {
        //            minus = false;
        //            n = word[i - 1];
        //            c = word[i];
        //            splited = c + splited;
        //            if (isconsonant(char.ToLower(n)) && isvowel(char.ToLower(c)) && scount > 0)
        //            {
        //                splited = n + splited;
        //                minus = true;
        //                i -= 1;
        //                scount -= 1;
        //                if (char.ToLower(n) == 'ь')
        //                {
        //                    splited = word[i - 1] + splited;
        //                    i -= 1;
        //                }
        //            }
        //            else if (isvowel(char.ToLower(c)) && isvowel(char.ToLower(n)))
        //            {
        //                minus = true;
        //                scount -= 1;
        //            }
        //            if (minus && scount > 0)
        //            {
        //                splited = '-' + splited;
        //            }
        //            i -= 1;
        //            if (i == 0)
        //            {
        //                splited = word[0] + splited;
        //            }
        //        }
        //        word = splited;
        //    }

        //    split2syllables();
        //    return word;
        //}

        private static string SplitWord(string word)
        {
            var vowels = new[] { 'а', 'у', 'о', 'ы', 'и', 'э', 'я', 'ю', 'ё', 'е', 'і', 'є', 'ї' };
            //var symbols = new[] { '\'', '-' };

            bool isvowel(char val)
            {
                for (int v = 0; v < vowels.Length; v++)
                {
                    if (vowels[v] == val)
                    {
                        return true;
                    }
                }
                return false;
            }
            bool issymbol(char val)
            {
                return char.IsSymbol(val);
            }

            if (word.Length <= 2)
                return word;

            int count = word.Length - 1;
            int index = 0;
            string result = "";
            while (index <= count)
            {
                char cur = word[index];
                bool haveNext = index <= count - 1;
                if (isvowel(char.ToLower(cur)))
                {
                    if (haveNext)
                    {
                        if (!isvowel(char.ToLower(word[index + 1])))
                        {
                            if (index <= count - 2 && (isvowel(char.ToLower(word[index + 2])) || char.ToLower(word[index + 2]) == 'ь'))
                            {
                                result = $"{result}_{cur}";
                                index++;
                                continue;
                            }
                            if (!issymbol(word[index + 1]))
                            {
                                result = $"{result}_{cur}{word[index + 1]}";
                                index += 2;
                            }
                            else
                            {
                                result = $"{result}_{cur}_{word[index + 1]}";
                                index += 2;
                            }
                        }
                        else
                        {
                            result = $"{result}_{cur}";
                            index++;
                        }
                    }
                    else
                    {
                        result = $"{result}_{cur}";
                        index++;
                    }
                }
                else
                {
                    if (haveNext)
                    {
                        if (isvowel(char.ToLower(word[index + 1])) || char.ToLower(word[index + 1]) == 'ь')
                        {
                            result = $"{result}_{cur}{word[index + 1]}";
                            index += 2;
                        }
                        else
                        {
                            result = $"{result}_{cur}";
                            if (issymbol(word[index + 1]))
                            {
                                result = $"{result}_{word[index + 1]}";
                                index++;
                            }
                            index++;
                        }
                    }
                    else
                    {
                        result = $"{result}_{cur}";
                        index++;
                    }
                }

            }
            return result.TrimStart(new[] { '_' });
        }
    }

    public class WordSplitterBlock
    {
        public string Block { get; }

        public WordSplitterBlock(string temp)
        {
            var vowels = new[] { 'а', 'у', 'о', 'ы', 'и', 'э', 'я', 'ю', 'ё', 'е', 'і', 'є', 'ї' };
            bool isvowel(char val)
            {
                for (int v = 0; v < vowels.Length; v++)
                {
                    if (vowels[v] == val)
                    {
                        return true;
                    }
                }
                return false;
            }

            Block = temp;
            IsDot = Block.Length == 1 && char.IsLetter(char.Parse(temp)) && !isvowel(char.ToLower(char.Parse(temp)));
        }

        public bool IsDot { get; }
    }

    public class Dot : Adorner
    {
        private readonly Brush brush;
        private readonly Inline inline;

        public Dot(UIElement element, Inline inline, Brush brush) : base(element)
        {
            this.brush = brush;
            this.inline = inline;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var startrect = inline.ContentStart.GetCharacterRect(LogicalDirection.Forward);
            var endrect = inline.ContentEnd.GetCharacterRect(LogicalDirection.Backward);
            var a = endrect.Left - startrect.Left;
            drawingContext.DrawEllipse(brush, null, new Point((a / 2) + startrect.Left, AdornedElement.RenderSize.Height), 1.5, 1.5);
        }
    }
}