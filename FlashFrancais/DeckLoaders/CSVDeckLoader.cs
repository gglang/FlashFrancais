using FlashFrancais.CardServers;
using Microsoft.VisualBasic.FileIO;

namespace FlashFrancais.DeckLoaders
{
    public class CSVDeckLoader : FlashDeckLoader
    {
        private string _delimiter;

        public CSVDeckLoader(string delimiter = ",")
        {
            _delimiter = delimiter;
        }

        public override FlashDeck GetFlashDeck(CardServer cardServer, string deckPath, string deckName = null)
        {
            deckName = deckName ?? GetDeckNameFromFileName(deckPath);
            return LoadSingleColumnCSV(cardServer, deckPath, deckName, _delimiter);
        }

        private FlashDeck LoadSingleColumnCSV(CardServer cardServer, string deckPath, string deckName, string delimiter = ",")
        {
            FlashDeck deckToLoad = FlashDeck.FromNothing(cardServer, deckName);
            using (TextFieldParser deckCSVParser = new TextFieldParser(@deckPath, System.Text.Encoding.GetEncoding("iso-8859-1")))
            {
                deckCSVParser.TextFieldType = FieldType.Delimited;
                deckCSVParser.SetDelimiters(delimiter);
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