using System;
using System.Data.SQLite;

namespace FlashFrancais
{
    // TODO PICKUP: Fill in all the blank methods and make deck and history serialization work (and maybe split into 2 classes)
    class DataBase
    {
        private const string _sqliteConnectionString = "Data Source=MyFirstDB.sqlite;Version=3;";

        private SQLiteConnection _connection;
        
        public void SetupConnection()
        {
            if(_connection != null)
                return;

            _connection = new SQLiteConnection(_sqliteConnectionString);
            _connection.Open();
        }

        private void ValidateConnection()
        {
            SetupConnection();
        }

        #region Deck DB

        public void SaveDeck(FlashDeck deck)
        {
            ValidateConnection();

            if(DeckExists(deck))
            {
                IncrementalSaveDeck();
            }
            else
            {
                FreshSaveDeck();
            }
        }

        private void IncrementalSaveDeck()
        {
            string sqlQuery = String.Format("insert into highscores (name, score) values ('Me{0}', {0})", new System.Random().Next());
            var command = new SQLiteCommand(sqlQuery, _connection);
            command.ExecuteNonQuery();
        }

        private void FreshSaveDeck()
        {

        }

        public string[] GetDeckNames()
        {
            ValidateConnection();
            return null;
        }

        public FlashDeck GetDeck(string deckName)
        {
            ValidateConnection();
            return null;
        }

        private bool DeckExists(FlashDeck deck)
        {
            return false;
        }

        #endregion

        #region History DB

        public void AddHistoryEntry(CardHistoryEntry historyEntry)
        {
            ValidateConnection();
        }

        public CardHistoryEntry[] GetHistory(Card card)
        {
            ValidateConnection();
            return null;
        }

        #endregion

       /* static void Main(string[] args)
        {
            // Create DB
            //SQLiteConnection.CreateFile("MyFirstDB.sqlite");

            // Connect

            string sql;
            SQLiteCommand command;

            // Create table
            /*sql = "create table highscores (name varchar(20), score int)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            // Insert into table
            sql = String.Format("insert into highscores (name, score) values ('Me{0}', {0})", new System.Random().Next());
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = String.Format("insert into highscores (name, score) values ('Myself{0}', {0})", new System.Random().Next());
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = String.Format("insert into highscores (name, score) values ('And I{0}', {0})", new System.Random().Next());
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            // Count table
            sql = "select count(*) from highscores";
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            int rowCount = Convert.ToInt32(command.ExecuteScalar());

            // Query table
            sql = "select * from highscores order by score desc";
            command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            Console.WriteLine("READING {0} RECORDS", rowCount);
            while (reader.Read())
            {
                Console.WriteLine("Name: {0} - Score: {1}", reader["name"], reader["score"]);
            }
            Console.WriteLine("DONE");
            Console.ReadKey();
        }*/
    }
}
