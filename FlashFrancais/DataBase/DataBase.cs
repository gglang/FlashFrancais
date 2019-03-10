﻿using System.Collections.Generic;

namespace FlashFrancais
{
    public interface Database
    {
        void AddCardToDeck(Card card, FlashDeck deck);
        void AddCardsToDeck(IEnumerable<Card> cards, FlashDeck deck);
        string[] GetDeckNames();
        FlashDeck GetDeck(string deckName);

        void AddHistoryEntry(Card card, bool success);
        CardHistoryEntry[] GetHistory(Card card);
    }
}