using SQLite;
using System.Threading.Tasks;

namespace Чтение.Managers
{
    class WordsDataBaseManager
    {
        private string Db_Path { get => "WordsDataBase.db"; }
        private SQLiteAsyncConnection connection;

        internal SQLiteAsyncConnection Connection { get => connection; }

        public WordsDataBaseManager()
        {
            connection = new SQLiteAsyncConnection(Db_Path);
            CreateTables();
        }

        private async void CreateTables()
        {
            await connection.CreateTableAsync<DbWord>();
        }

        public async Task Close() => 
            await connection.CloseAsync();
    }
}
