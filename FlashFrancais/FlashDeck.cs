using System;
using System.Collections.Generic;

namespace FlashFrancais
{
    public class FlashDeck
    {
        private IList<Card> FlashCards { get; set; }
        private int deckIndex = 0;

        private FlashDeck()
        {
            FlashCards = new List<Card>();
        }

        private FlashDeck(IList<Card> flashCards)
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

        public static FlashDeck FromList(IList<Card> flashCards)
        {
            return new FlashDeck(flashCards);
        }

        public Card GetNextCard()
        {
            return FlashCards[(deckIndex++) % FlashCards.Count];
        }

        public void AddCard(Card card)
        {
            FlashCards.Add(card);
        }
    }
}