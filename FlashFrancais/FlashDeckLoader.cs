using System;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace FlashFrancais
{
    public class FlashDeckLoader
    {
        public enum File_Type
        {
            SingleColumnCSV
        }

        public FlashDeckLoader()
        {
        }

        public FlashDeck GetFlashDeck(string deckPath, File_Type fileType = File_Type.SingleColumnCSV)
        {
            switch (fileType)
            {
                case File_Type.SingleColumnCSV:
                    return LoadSingleColumnCSV(deckPath);
                default:
                    throw new System.NotImplementedException("Have not implemented support for loading this file type. SOS!");
            }
        }

        private FlashDeck LoadSingleColumnCSV(string deckPath)
        {
            FlashDeck deckToLoad = FlashDeck.FromNothing();
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