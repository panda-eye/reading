using SQLite;
using Чтение.Localization;

namespace Чтение.Managers
{
    public class DbWord
    {
        public DbWord() { }
        public DbWord(string value, string source, int typeId)
        {
            Value = value;
            Source = source;
            TypeId = typeId;
        }

        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        public string Value { get; set; }

        public string Source { get; set; }

        public string Language { get; set; } = LocalizationManager.Instance.CurrentCulture.Name;

        public int TypeId { get; set; }
    }
}
