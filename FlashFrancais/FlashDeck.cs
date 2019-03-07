using FlashFrancais.DeckLoaders;
using System;
using System.Collections.Generic;

namespace FlashFrancais
{
    public class FlashDeck
    {
        public string Name { get; }
        private IList<Card> _flashCards { get; set; }
        private int _deckIndex = 0;

        private FlashDeck(string deckName, IList<Card> flashCards = null)
        {
            if (deckName == null)
                throw new InvalidOperationException("No deck name declared for a deck being constructed. Please provide a deck name!");

            Name = deckName;
            _flashCards = flashCards ?? new List<Card>();
        }

        public static FlashDeck FromNothing(string deckName)
        {
            return new FlashDeck(deckName);
        }

        public static FlashDeck FromCSV(string deckPath, string deckName = null)
        {
            var deckLoader = new CSVDeckLoader();
            return deckLoader.GetFlashDeck(deckPath, deckName: deckName);
        }

        public static FlashDeck FromAnki(string deckPath, string deckName = null)
        {
            var deckLoader = new AnkiDeckLoader();
            return deckLoader.GetFlashDeck(deckPath, deckName); // TODO is there better pattern here for loading different file types? Injecting the loader perhaps?
        }

        public static FlashDeck FromList(IList<Card> flashCards, string deckName)
        {
            return new FlashDeck(deckName, flashCards);
        }

        public Card GetNextCard()
        {
            return _flashCards[(_deckIndex++) % _flashCards.Count];
        }

        public void AddCard(Card card)
        {
            _flashCards.Add(card);
        }

        public IEnumerable<Card> GetCards()
        {
            return _flashCards;
        }
    }
}