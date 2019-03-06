using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace FlashFrancais
{
    // TODO This class is too big
    class SQLiteDatabase : Database
    {
        private SQLiteConnection _connection;

        public SQLiteDatabase(SQLiteConnection connection)
        {
            _connection = connection ?? throw new InvalidDataBaseOperationException("Cannot construct a database with a null connection.");
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

        public void AddCardToDeck(Card card, FlashDeck deck)
        {
            AddCardToDatabase(card, deck);
        }

        public void AddCardsToDeck(IEnumerable<Card> cards, FlashDeck deck)
        {
            foreach(Card card in cards)
            {
                AddCardToDeck(card, deck);
            }
        }

        private void AddCardToDatabase(Card card, FlashDeck deck) // TODO Maybe make exception throw if card already exists? Make unique(front, back) constraint on cards table
        {
            int newCardID = AddCardToCardTable(card);
            AddCardToDeckTable(deck, newCardID);
        }

        /// <returns>ID of new card added to database.</returns>
        private int AddCardToCardTable(Card card)
        {
            string addCardQuery = String.Format("insert into Cards (Front, Back) values ('{0}', '{1}')", card.Front, card.Back);
            string getNewCardIDQuery = "select CardID from Cards where rowid=last_insert_rowid()";

            ExecuteSimpleQuery(addCardQuery);
            int newCardID = ExecuteScalarQuery(getNewCardIDQuery);
            return newCardID;
        }

        private void AddCardToDeckTable(FlashDeck deck, int cardID)
        {
            string addCardToDeckQuery = String.Format("insert into Decks (DeckName, Card) values ('{0}', {1})", deck.Name, cardID);
            ExecuteSimpleQuery(addCardToDeckQuery);
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

        public FlashDeck GetDeck(string deckName)
        {
            FlashDeck deck = FlashDeck.FromNothing(deckName);
            string getDeckQuery = String.Format("select * from Decks where DeckName='{0}'", deckName);
            SQLiteDataReader dataReader = ExecuteReaderQuery(getDeckQuery);
            while(dataReader.Read())
            {
                string cardFront = Convert.ToString(dataReader["Front"]);
                string cardBack = Convert.ToString(dataReader["Back"]);
                Card card = new Card(cardFront, cardBack);
                deck.AddCard(card);
            }

            return deck;
        }

        #endregion

        #region History DB

        public void AddHistoryEntry(Card card, bool success)
        {
            AddHistoryEntryToDatabase(card, success);
        }

        public CardHistoryEntry[] GetHistory(Card card)
        {
            List<CardHistoryEntry> historyEntries = new List<CardHistoryEntry>();
            string getHistoryQuery = String.Format("select * from CardHistories where Card='{0}'", card.ID);
            SQLiteDataReader dataReader = ExecuteReaderQuery(getHistoryQuery);
            while(dataReader.Read())
            {
                DateTime dateTime = new DateTime(Convert.ToInt64(dataReader["DateTime"]));
                int successInt = Convert.ToInt32(dataReader["Success"]);
                bool success = successInt == 1 ? true : false;
                var historyEntry = new CardHistoryEntry(dateTime, success);
                historyEntries.Add(historyEntry);
            }

            return historyEntries.ToArray();
        }

        private void AddHistoryEntryToDatabase(Card card, bool success)
        {
            int successInt = success ? 1 : 0; // TODO change bool in sqlite database to string version of bool...
            string insertHistoryEntryQuery = String.Format("insert into CardHistories (DateTime, Success, Card) values ('{0}', {1}, {2})", DateTime.Now.Ticks, card.ID, successInt);
            ExecuteSimpleQuery(insertHistoryEntryQuery);
        }

        #endregion
    }
}
