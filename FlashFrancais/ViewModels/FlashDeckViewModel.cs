using GalaSoft.MvvmLight;
using System.IO;
using System.Linq;
using System.Windows.Input;
using FlashFrancais.Services;

namespace FlashFrancais.ViewModels
{
    public class FlashDeckViewModel : ViewModelBase
    {
        private bool _showingFront;
        private string _currentCardText;
        private int _cardSuccesses;
        private Card _currentCard;

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

        public FlashDeckViewModel(Database database, FlashDeckProvider deckProvider)
        {
            // TODO Check out if (IsInDesignMode) example in mvvmlight for blend

            _myDeck = deckProvider.GetFlashDeck(); // TODO is this the right place for this kind of logic?
            _showingFront = true;
            GetNextCard(); // TODO move this to a start event in WPF or something else...
        }

        private void FlipCurrectCard()
        {
            ShowingFront = !ShowingFront;
            RefreshTextDisplay();
        }

        private void GetNextCardSuccess() // TODO Maybe use command parameters to avoid 2 different commands? Also rename to something better...
        {
            _currentCard.AddHistoryEntry(TrialPerformance.Normal);
            GetNextCard();
        }

        private void GetNextCardFailure()
        {
            _currentCard.AddHistoryEntry(TrialPerformance.Fail);
            GetNextCard();
        }

        private void GetNextCard()
        {
            _currentCard = _myDeck.GetNextCardNew();
            ShowingFront = true;
            RefreshTextDisplay();
        }

        private void RefreshTextDisplay()
        {
            if (_currentCard == null)
            {
                _currentCard = _myDeck.GetNextCardNew();
            }

            CurrentCardText = ShowingFront ? _currentCard.Front : _currentCard.Back;
            CardSuccesses = _currentCard.HistoryEntries.Where(entry => entry.TrialPerformance > 0).Count();
        }
    }
}

