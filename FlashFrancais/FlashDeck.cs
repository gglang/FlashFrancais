using System;
using System.Collections.Generic;

namespace FlashFrancais
{
    public class FlashDeck
    {
        private IList<FlashCard> FlashCards { get; set; }
        private int deckIndex = 0;

        private FlashDeck()
        {
            FlashCards = new List<FlashCard>();
        }

        private FlashDeck(IList<FlashCard> flashCards)
        {
            FlashCards = flashCards;
        }

        public static FlashDeck FromNothing()
        {
            return new FlashDeck();
        }

        public static FlashDeck FromPath(string deckPath)
        {
            var deckLoader = new FlashDeckLoader();
            return deckLoader.GetFlashDeck(deckPath);
        }

        public static FlashDeck FromList(IList<FlashCard> flashCards)
        {
            return new FlashDeck(flashCards);
        }

        public FlashCard GetNextCard()
        {
            return FlashCards[(deckIndex++) % FlashCards.Count];
        }

        public void AddCard(FlashCard card)
        {
            FlashCards.Add(card);
        }
    }
}