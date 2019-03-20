using FlashFrancais.CardServers;
using Microsoft.VisualBasic.FileIO;

namespace FlashFrancais.DeckLoaders
{
    public class CSVDeckLoader : FlashDeckLoader
    {
        public override FlashDeck GetFlashDeck(CardServer cardServer, string deckPath, string deckName = null)
        {
            deckName = deckName ?? GetDeckNameFromFileName(deckPath);
            return LoadSingleColumnCSV(cardServer, deckPath, deckName);
        }

        private FlashDeck LoadSingleColumnCSV(CardServer cardServer, string deckPath, string deckName)
        {
            FlashDeck deckToLoad = FlashDeck.FromNothing(cardServer, deckName);
            using (TextFieldParser deckCSVParser = new TextFieldParser(@deckPath))
            {
                deckCSVParser.TextFieldType = FieldType.Delimited;
                deckCSVParser.SetDelimiters(",");
                while (!deckCSVParser.EndOfData)
                {
                    string[] flashCardSides = deckCSVParser.ReadFields();
                    Card loadedCard = new Card(flashCardSides[0], flashCardSides[1]);
                    deckToLoad.AddCard(loadedCard);
                }
            }
            return deckToLoad;
        }
    }
}