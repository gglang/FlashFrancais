using FlashFrancais.CardServers;
using FlashFrancais.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

namespace FlashFrancais
{
    // TODO This class is too big
    class SQLiteDatabase : Database
    {
        private SQLiteConnection _connection;

        public SQLiteDatabase(ConnectionStringProvider connectionStringProvider)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionStringProvider.GetConnectionString());
            _connection = connection ?? throw new InvalidDataBaseOperationException("Cannot construct a database with a null connection. " +
                "Check ConnectionStringProvider for problems loading a valid connection string to initialize database.");
            _connection.Open(); // TODO Where is the best place to open and close connections?
        }

        private void ExecuteSimpleQuery(string sql)
        {
            var sqlCommand = new SQLiteCommand(sql, _connection);
            sqlCommand.ExecuteNonQuery();
        }

        private int ExecuteScalarQuery(string sql)
        {
            var sqlCommand = new SQLiteCommand(sql, _connection);
            int scalarResult = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return scalarResult;
        }

        private SQLiteDataReader ExecuteReaderQuery(string sql)
        {
            var command = new SQLiteCommand(sql, _connection);
            command.CommandType = CommandType.Text;
            SQLiteDataReader dataReader = command.ExecuteReader();
            return dataReader;
        }

        #region Deck DB

        public void AddCardToDeck(Card card, string deckName)
        {
            AddCardToDatabase(card, deckName);
        }

        public void AddCardsToDeck(IEnumerable<Card> cards, string deckName)
        {
            int index = 0;
            using (var transaction = _connection.BeginTransaction())
            {
                foreach (Card card in cards)
                {
                    index++;
                    Debug.WriteLine(string.Format("Card #{0} being added.", index));
                    AddCardToDeck(card, deckName);
                }

                transaction.Commit();
            }
        }

        private void AddCardToDatabase(Card card, string deckName) // TODO Maybe make exception throw if card already exists? Make unique(front, back) constraint on cards table
        {
            int newCardID = AddCardToCardTable(card);
            AddCardToDeckTable(deckName, newCardID);
        }

        /// <returns>ID of new card added to database.</returns>
        private int AddCardToCardTable(Card card)
        {
            var command = new SQLiteCommand("insert into Cards (Front, Back) values (@front, @back)", _connection);
            command.Parameters.AddWithValue("@front", card.Front);
            command.Parameters.AddWithValue("@back", card.Back); // TODO Use this binding technique everywhere
            command.ExecuteNonQuery();

            string getNewCardIDQuery = "select CardID from Cards where rowid=last_insert_rowid()";
            int newCardID = ExecuteScalarQuery(getNewCardIDQuery);
            return newCardID;
        }

        private void AddCardToDeckTable(string deckName, int cardID)
        {
            var command = new SQLiteCommand("insert into Decks (DeckName, Card) values (@deckName, @cardID)", _connection);
            command.Parameters.AddWithValue("@deckName", deckName);
            command.Parameters.AddWithValue("@cardID", cardID); // TODO How do SQL attacks work and why does this prevent them?
            command.ExecuteNonQuery();
        }

        public string[] GetDeckNames()
        {
            List<string> deckNames = new List<string>();
            string getDecksQuery = "select distinct DeckName from Decks";
            SQLiteDataReader dataReader = ExecuteReaderQuery(getDecksQuery);
            while(dataReader.Read())
            {
                deckNames.Add(Convert.ToString(dataReader["DeckName"]));
            }

            return deckNames.ToArray();
        }

        public FlashDeck GetDeck(CardServer cardServer, string deckName)
        {
            FlashDeck deck = FlashDeck.FromNothing(cardServer, deckName);
            var command = new SQLiteCommand(
                @"select card.CardID, card.Front, card.Back 
                from Cards card 
                join Decks deck
                on (deck.Card = card.CardID and deck.DeckName = (@deckName))", _connection);
            command.Parameters.AddWithValue("@deckName", deckName);
            command.CommandType = CommandType.Text;
            SQLiteDataReader dataReader = command.ExecuteReader();
            while(dataReader.Read())
            {
                string cardFront = Convert.ToString(dataReader["Front"]);
                string cardBack = Convert.ToString(dataReader["Back"]);
                int id = Convert.ToInt32(dataReader["CardID"]);
                Card card = new Card(id, cardFront, cardBack);
                deck.AddCard(card);
            }

            return deck;
        }

        #endregion

        #region History DB

        public void AddHistoryEntry(Card card, TrialPerformance trialPerformance)
        {
            AddHistoryEntryToDatabase(card, trialPerformance);
        }

        public CardHistoryEntry[] GetHistory(Card card)
        {
            List<CardHistoryEntry> historyEntries = new List<CardHistoryEntry>();
            string getHistoryQuery = String.Format("select * from CardHistories where Card='{0}'", card.ID);
            SQLiteDataReader dataReader = ExecuteReaderQuery(getHistoryQuery);
            while(dataReader.Read())
            {
                DateTime dateTime = new DateTime(Convert.ToInt64(dataReader["DateTime"]));
                TrialPerformance trialPerformance = (TrialPerformance)Convert.ToInt32(dataReader["Success"]);
                var historyEntry = new CardHistoryEntry(dateTime, trialPerformance);
                historyEntries.Add(historyEntry);
            }

            return historyEntries.ToArray();
        }

        private void AddHistoryEntryToDatabase(Card card, TrialPerformance trialPerformance)
        {
            int successInt = (int)trialPerformance; // TODO change bool in sqlite database to string version of bool...
            string insertHistoryEntryQuery = String.Format("insert into CardHistories (DateTime, Success, Card) values ('{0}', {1}, {2})", DateTime.Now.Ticks, successInt, card.ID);
            ExecuteSimpleQuery(insertHistoryEntryQuery);
        }

        #endregion
    }
}
