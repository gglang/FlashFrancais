using FlashFrancais.CardServers;
using FlashFrancais.DeckLoaders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlashFrancais
{
    public class FlashDeck
    {
        public string Name { get; }
        public CardServer _cardServer; // TODO Why private property like this?
        public ObservableCollection<Card> _flashCards { get; set; }
        private int _deckIndex = 0;

        private FlashDeck(CardServer cardServer, string deckName, IList<Card> flashCards = null)
        {
            Name = deckName ?? throw new InvalidOperationException("No deck name declared for a deck being constructed. Please provide a deck name!");
            _flashCards = flashCards == null ? new ObservableCollection<Card>() : new ObservableCollection<Card>(flashCards);
            _cardServer = cardServer;
            _cardServer.Init(_flashCards); // TODO I am not convinced of this pattern :( Also it seems to take way too long to set new things up when doing DI like this
        }

        public static FlashDeck FromNothing(CardServer cardServer, string deckName)
        {
            return new FlashDeck(cardServer, deckName);
        }

        public static FlashDeck FromCSV(CardServer cardServer, string deckPath, string deckName = null, string delimiter = ",")
        {
            var deckLoader = new CSVDeckLoader(delimiter);
            return deckLoader.GetFlashDeck(cardServer, deckPath, deckName: deckName);
        }

        public static FlashDeck FromAnki(CardServer cardServer, string deckPath, string deckName = null) // TODO does the server really belong here?
        {
            var deckLoader = new AnkiDeckLoader(); // TODO lol between providers, factory constructors, loader classes, the creation of a flashdeck has become a monster
            return deckLoader.GetFlashDeck(cardServer, deckPath, deckName); // TODO is there better pattern here for loading different file types? Injecting the loader perhaps?
        }

        public static FlashDeck FromList(CardServer cardServer, IList<Card> flashCards, string deckName)
        {
            return new FlashDeck(cardServer, deckName, flashCards);
        }

        public Card GetNextCard(Database database, TrialPerformance trialPerformance)
        {
            return _cardServer.GetNextCard(trialPerformance);
        }

        public void AddCard(Card card)
        {
            _flashCards.Add(card);
        }

        public void AddCards(IList<Card> cards)
        {
            foreach(var card in cards)
            {
                AddCard(card);
            }
        }

        public IEnumerable<Card> GetCards()
        {
            return _flashCards;
        }
    }
}