using FlashFrancais.CardServers;

namespace FlashFrancais.Services
{
    public class SQLiteFlashDeckProvider : FlashDeckProvider
    {
        private const string deckName = "FlashFrench";
        private readonly Database _database;
        private readonly CardServer _cardServer;

        public SQLiteFlashDeckProvider(Database database, CardServer cardServer)
        {
            _cardServer = cardServer;
            _database = database;
        }

        public FlashDeck GetFlashDeck()
        {
            FlashDeck deck = _database.GetDeck(_cardServer, deckName);
            return deck;
        }
    }
}
