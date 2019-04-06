using System.IO;
using FlashFrancais.CardServers;

namespace FlashFrancais.Services
{
    public class AnkiFlashDeckProvider : FlashDeckProvider// TODO For now this is just a SQLite connection string provider, make interface if it becomes necessary
    {
        private const string relativeDatabasePath = @"..\..\..\Decks\Test\collection.anki2";
        private const string deckName = "Anki5000CourantMots";

        private CardServer _cardServer;

        public AnkiFlashDeckProvider(CardServer cardServer) // TODO Add config provider to here and read what you need from some kind of config database/csv
        {
            _cardServer = cardServer;
        }

        public FlashDeck GetFlashDeck()
        {
            FlashDeck deck = FlashDeck.FromAnki(_cardServer, Path.GetFullPath(relativeDatabasePath), deckName);
            return deck;
        }
    }
}