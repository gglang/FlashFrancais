using FlashFrancais.CardServers;
using System.Collections.Generic;

namespace FlashFrancais
{
    public interface Database
    {
        void AddCardToDeck(Card card, string deckName);
        void AddCardsToDeck(IEnumerable<Card> cards, string deckName);
        string[] GetDeckNames();
        FlashDeck GetDeck(CardServer cardServer, string deckName);

        void AddHistoryEntry(Card card, TrialPerformance trialPerformance);
        CardHistoryEntry[] GetHistory(int cardID);
    }
}