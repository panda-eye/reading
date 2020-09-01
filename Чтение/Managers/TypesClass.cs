using System.Collections.Generic;
using System.Linq;

namespace Чтение.Managers
{
    public static class TypesClass
    {
        private const string with_ru = "Слоги с ";
        private const string with_uk = "Склади з ";

        static TypesClass()
        {
            CurrentId = TypesLvl6.Last().Id;
            var list = new List<Type>();
            var set = new Properties.Settings();

            var tverdie = "бвгджзклмнпрстфхцшауыэо".ToCharArray().OrderBy(c => c);
            var mjagkie = "еёюяийчщь".ToCharArray().OrderBy(c => c);

            foreach (var letter in tverdie)
                list.Add(new Type($"{with_ru}{char.ToUpper(letter)}", "ru-RU", 0));
            foreach (var letter in mjagkie)
                list.Add(new Type($"{with_ru}{char.ToUpper(letter)}", "ru-RU", 1));

            tverdie = "бвгґджзклмнпрстфхцчшщйаеиоу".ToCharArray().OrderBy(c => c);
            mjagkie = "яюєіьї".ToCharArray().OrderBy(c => c);

            foreach (var letter in tverdie)
                list.Add(new Type($"{with_uk}{char.ToUpper(letter)}", "uk-UA", 0));
            foreach (var letter in mjagkie)
                list.Add(new Type($"{with_uk}{char.ToUpper(letter)}", "uk-UA", 1));

            TypesLvl1 = list.ToArray();
        }
        public static Type[] TypesLvl1;
        public static Type[] TypesLvl2 = new[]
        {
            new Type("1-сложные слова", "ru-RU"),
            new Type("2-сложные слова", "ru-RU"),
            new Type("3-сложные слова", "ru-RU"),

            new Type("1-складові слова", "uk-UA"),
            new Type("2-складові слова", "uk-UA"),
            new Type("3-складові слова", "uk-UA")
        };
        public static Type[] TypesLvl3 = new[]
        {
            new Type("1-сложные слова", "ru-RU"),
            new Type("2-сложные слова", "ru-RU"),
            new Type("3-сложные слова", "ru-RU"),

            new Type("1-складові слова", "uk-UA"),
            new Type("2-складові слова", "uk-UA"),
            new Type("3-складові слова", "uk-UA")
        };
        public static Type[] TypesLvl4 = new[]
        {
            new Type("1-сложные слова", "ru-RU"),
            new Type("2-сложные слова", "ru-RU"),
            new Type("3-сложные слова", "ru-RU"),
            new Type("4-сложные слова", "ru-RU"),
            new Type("Слова с мягкими слогами", "ru-RU"),

            new Type("1-складові слова", "uk-UA"),
            new Type("2-складові слова", "uk-UA"),
            new Type("3-складові слова", "uk-UA"),
            new Type("4-складові слова", "uk-UA"),
            new Type("Слова з м'якими складами", "uk-UA")
        };
        public static Type[] TypesLvl5 = new[]
        {
            new Type("Словосочетания", "ru-RU"),
            new Type("Слова с мягким знаком", "ru-RU"),
            new Type("3-сложные слова", "ru-RU"),
            new Type("4-сложные слова", "ru-RU"),
            new Type("Слоги с разделительным мягким знаком", "ru-RU"),
            new Type("Слоги с твёрдым знаком", "ru-RU"),

            new Type("Словосполучення", "uk-UA"),
            new Type("Слова з м'яким знаком", "uk-UA"),
            new Type("3-складові слова", "uk-UA"),
            new Type("4-складові слова", "uk-UA"),
            new Type("Склади з розділовим м'яким знаком", "uk-UA"),
            new Type("Склади з апострофом", "uk-UA")
        };
        public static Type[] TypesLvl6 = new[]
        {
            new Type("Словосочетания", "ru-RU"),
            new Type("Предложения", "ru-RU"),

            new Type("Словосполучення", "uk-UA"),
            new Type("Речення", "uk-UA")
        };

        private static int currentId = 0;
        public static int CurrentId
        {
            get => currentId;
            set => currentId = value;
        }
    }

    public class Type
    {
        public Type(string name, string language, int baseId = -1)
        {
            Name = name;
            BaseId = baseId;
            Language = language;
            Id = GenerateId();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int BaseId { get; set; }
        public string Language { get; set; }

        private int GenerateId() =>
            ++TypesClass.CurrentId;
    }
}
