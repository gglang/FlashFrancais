using Microsoft.VisualBasic.FileIO;

namespace FlashFrancais.DeckLoaders
{
    public class CSVDeckLoader : FlashDeckLoader
    {
        public override FlashDeck GetFlashDeck(string deckPath, string deckName = null)
        {
            deckName = deckName ?? GetDeckNameFromFileName(deckPath);
            return LoadSingleColumnCSV(deckPath, deckName);
        }

        private FlashDeck LoadSingleColumnCSV(string deckPath, string deckName)
        {
            FlashDeck deckToLoad = FlashDeck.FromNothing(deckName);
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