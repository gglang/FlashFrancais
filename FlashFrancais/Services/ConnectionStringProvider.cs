using System;
using System.IO;

namespace FlashFrancais.Services
{
    public class ConnectionStringProvider // TODO For now this is just a SQLite connection string provider, make interface if it becomes necessary
    {
        private const string relativeDatabasePath = @"..\..\..\Decks\FlashDB.sqlite";

        public ConnectionStringProvider() // TODO Add config provider to here and read what you need from some kind of config database/csv
        {
        }

        public string GetConnectionString()
        {
            string absoluteDatabasePath = Path.GetFullPath(relativeDatabasePath);
            string connectionString = String.Format("Data Source={0};Version=3;", absoluteDatabasePath);
            return connectionString;
        }
    }
}
