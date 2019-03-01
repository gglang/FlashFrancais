using System;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;

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

        public FlashDeck GetFlashDeck(string deckPath, File_Type fileType = File_Type.SingleColumnCSV, string deckName = null)
        {
            deckName = deckName ?? GetDeckNameFromFileName(deckPath);
            switch (fileType)
            {
                case File_Type.SingleColumnCSV:
                    return LoadSingleColumnCSV(deckPath, deckName);
                default:
                    throw new System.NotImplementedException("Have not implemented support for loading this file type. SOS!");
            }
        }

        private string GetDeckNameFromFileName(string deckPath)
        {
            return Path.GetFileNameWithoutExtension(deckPath);
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