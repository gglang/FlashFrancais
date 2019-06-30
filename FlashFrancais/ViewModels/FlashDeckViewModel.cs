using GalaSoft.MvvmLight;
using System.Linq;
using System.Windows.Input;
using FlashFrancais.Services;
using FlashFrancais.CardServers;
using Autofac;
using System.Windows;
using FlashFrancais.Views;

namespace FlashFrancais.ViewModels
{
    public class FlashDeckViewModel : ViewModelBase
    {
        private bool _showingFront;
        private string _currentCardText;
        private int _cardSuccesses;
        private Card _currentCard;
        private Database _database;
        private CardServer _cardServer;
        private string[] _previouslyLoadedDecks;
        private DebugWindow _debugWindow;

        private FlashDeck _myDeck;

        public int CardSuccesses
        {
            get { return _cardSuccesses; }
            private set { Set(ref _cardSuccesses, value); } // TODO abstract dependency on mvvmlight somehow?
        }

        public string CurrentCardText
        {
            get { return _currentCardText; }
            private set { Set(ref _currentCardText, value); }
        }

        public bool ShowingFront
        {
            get { return _showingFront; }
            private set { Set(ref _showingFront, value); }
        }

        public ICommand FlipCurrentCardCommand
        {
            get
            {
                return new DelegateCommand(FlipCurrectCard); // TODO mvvmlightify this?
            }
        }

        public ICommand GetNextCardSuccessCommand
        {
            get
            {
                return new DelegateCommand(GetNextCardSuccess);
            }
        }

        public ICommand GetNextCardFailureCommand
        {
            get
            {
                return new DelegateCommand(GetNextCardFailure);
            }
        }

        public ICommand CreateReverseCommand
        {
            get
            {
                return new DelegateCommand(CreateReverse);
            }
        }

        public ICommand DebugCommand
        {
            get
            {
                return new DelegateCommand(SpawnDebugWindow);
            }
        }

        public FlashDeckViewModel(Database database)
        {
            // TODO Check out if (IsInDesignMode) example in mvvmlight for blend
            _database = database;
            ReloadWithDeckNames(GetDefaultDeckNames());
        }

        public static bool ReloadRequested = false;
        public void ReloadWithDeckNames(string[] deckNames)
        {
            if(!ReloadRequested && !ShouldReloadDeck(deckNames))
            {
                return;
            }
            ReloadRequested = false;
            _currentCard = null;
            _cardServer = GlobalFactory.Container.Resolve<CardServer>();
            _previouslyLoadedDecks = deckNames;
            _myDeck = _database.GetCompoundDeck(_cardServer, deckNames);
            ShowingFront = true;
            GetNextCard(TrialPerformance.Fail); // TODO move this to a start event in WPF or something else...
        }

        private bool ShouldReloadDeck(string[] deckNames)
        {
            if(deckNames?.Count() != _previouslyLoadedDecks?.Count())
            {
                return true;
            }

            foreach (var deckName in _previouslyLoadedDecks)
            {
                if (!deckNames.Contains(deckName))
                {
                    return true;
                }
            }

            return false;
        }

        private string[] GetDefaultDeckNames()
        {
            return _database.GetDeckNames();
        }

        private void FlipCurrectCard()
        {
            ShowingFront = !ShowingFront;
            RefreshTextDisplay();
        }

        private void GetNextCardSuccess() // TODO Maybe use command parameters to avoid 2 different commands? Also rename to something better...
        {
            _currentCard.AddHistoryEntry(TrialPerformance.Normal);
            GetNextCard(TrialPerformance.Normal);
        }

        private void GetNextCardFailure()
        {
            _currentCard.AddHistoryEntry(TrialPerformance.Fail);
            GetNextCard(TrialPerformance.Fail);
        }

        private void CreateReverse()
        {
            Card reverseCard = new Card(_currentCard.Back, _currentCard.Front);
            if(CreateCardsControl.SelectedDeckToAddTo == null)
            {
                MessageBox.Show("No selected deck to add to, go select one on the create cards page first.");
                return;
            }
            _database.AddCardToDeck(reverseCard, CreateCardsControl.SelectedDeckToAddTo);
            _myDeck.AddCard(reverseCard);
        }

        private void SpawnDebugWindow()
        {
            _debugWindow = new DebugWindow();
            _debugWindow.Show();
            _debugWindow.InitDebugging(_myDeck._cardServer.GetUpcomingCards());
        }

        private void GetNextCard(TrialPerformance trialPerformance)
        {
            _currentCard = _myDeck.GetNextCard(_database, trialPerformance);
            ShowingFront = true;
            RefreshTextDisplay();
        }

        private void RefreshTextDisplay()
        {
            if (_currentCard == null)
            {
                _currentCard = _myDeck.GetNextCard(_database, TrialPerformance.Fail);
                if(_currentCard == null)
                {
                    return;
                }
            }

            CurrentCardText = ShowingFront ? _currentCard.Front : _currentCard.Back;
            CardSuccesses = _currentCard.HistoryEntries.Where(entry => entry.TrialPerformance > 0).Count();

            _debugWindow?.Refresh();
        }
    }
}

