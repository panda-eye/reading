using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace Чтение.Managers
{
    public static class FontManager
    {
        public static HandwrittenFont.HandwrittenFontCollection Fonts { get => new HandwrittenFont.HandwrittenFontCollection(); }

        public class HandwrittenFont
        {
            private string name;

            protected HandwrittenFont(string fontName)
            {
                name = fontName;
            }

            public FontFamily Font
            {
                get
                {
                    return new FontFamily($"{FontFolder}\\#{Typeface.FamilyNames.FirstOrDefault().Value}");
                }
            }

            private string FontFolder { get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Reading", "Fonts"); } // "pack://application:,,,/Чтение;component/Resources/Fonts"
            private string FileName { get => name + ".ttf"; }
            private GlyphTypeface Typeface { get => new GlyphTypeface(new Uri(Path.Combine(FontFolder, FileName))); }

            public class HandwrittenFontCollection
            {
                public ReadOnlyCollection<HandwrittenFont> Fonts { get => fonts; }

                public HandwrittenFontCollection()
                {
                    fonts = new ReadOnlyCollection<HandwrittenFont>(new List<HandwrittenFont>()
                    {
                        new HandwrittenFont("Manuscript"),
                        new HandwrittenFont("Antonella"),
                        new HandwrittenFont("ScriptC"),
                        new HandwrittenFont("Eskal"),
                        new HandwrittenFont("Tagir DP Normal"),
                        new HandwrittenFont("Anselmo")
                    });
                }

                public HandwrittenFont this[int index]
                {
                    get
                    {
                        if (fonts.Count > index && index > -1)
                            return fonts.ElementAt(index);
                        else
                            return null;
                    }
                }

                private ReadOnlyCollection<HandwrittenFont> fonts;
            }
        }
    }
}
