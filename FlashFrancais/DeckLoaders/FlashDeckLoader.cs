using System.IO;

namespace FlashFrancais.DeckLoaders
{
    public abstract class FlashDeckLoader
    {
        public abstract FlashDeck GetFlashDeck(string deckPath, string deckName);

        protected string GetDeckNameFromFileName(string deckPath)
        {
            return Path.GetFileNameWithoutExtension(deckPath);
        }
    }
}
