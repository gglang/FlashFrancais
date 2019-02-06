using System;
using System.Collections.Generic;

namespace FlashFrancais
{
    public class FlashDeck
    {
        private IList<FlashCard> FlashCards { get; set; }
        private int deckIndex = 0;

        public FlashDeck(IList<FlashCard> flashCards)
        {
            FlashCards = flashCards;
        }

        public FlashCard GetNextCard()
        {
            return FlashCards[(deckIndex++) % FlashCards.Count];
        }
    }
}