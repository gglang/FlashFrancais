using AnkiSharp;
using System;
using System.Data;
using System.Data.SQLite;

namespace FlashFrancais.DeckLoaders
{
    public class AnkiDeckLoader : FlashDeckLoader
    {
        // Lolz; process for loading anki .apkg: rename to .zip, unzip, load .anki2 into sqlite, read cards based on format
        // defined in this particular anki deck
        public override FlashDeck GetFlashDeck(string deckPath, string deckName = null)
        {
            deckName = deckName ?? GetDeckNameFromFileName(deckPath);

            // SQLite connection
            string connectionString = String.Format("Data Source={0};Version=3;",deckPath);
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            // Get Reader
            string sql = "select flds from notes";
            var command = new SQLiteCommand(sql, connection);
            command.CommandType = CommandType.Text;
            SQLiteDataReader dataReader = command.ExecuteReader();

            // Read
            FlashDeck deck = FlashDeck.FromNothing(deckName);
            string[] unicodeSpaceSeperator = { "\u001f" };
            while(dataReader.Read())
            {
                string row = Convert.ToString(dataReader["flds"]);
                string[] splitRow = row.Split(unicodeSpaceSeperator, StringSplitOptions.None);
                string french = splitRow[1];
                string english = splitRow[3];

                Card loadedCard = new Card(french, english);
                deck.AddCard(loadedCard);
            }

            return deck;
        }
    }
}
