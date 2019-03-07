using GalaSoft.MvvmLight;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace FlashFrancais.ViewModels
{
    public class FlashDeckViewModel : ViewModelBase
    {
        private bool _showingFront;
        private string _currentCardText;
        private int _cardSuccesses;
        private Card _currentCard;

        private FlashDeck myDeck;

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

        public FlashDeckViewModel(Database database)
        {
            // TODO Check out if (IsInDesignMode) example in mvvmlight for blend

            //myDeck = FlashDeck.FromCSV(Path.GetFullPath(@"..\..\..\Decks\debugDeck.csv")); // TODO Dependency injection in WPF XAML instantiated viewmodels?
            myDeck = FlashDeck.FromAnki(Path.GetFullPath(@"..\..\..\Decks\Test\collection.anki2"), "5000MotsCourants");
            // TODO How to use .resx or resource dictionaries to store paths?
            _showingFront = true;
            GetNextCard(); // TODO move this to a start event in WPF or something else...
        }

        private void FlipCurrectCard()
        {
            ShowingFront = !ShowingFront;
            RefreshTextDiplay();
        }

        private void GetNextCardSuccess() // TODO Maybe use command parameters to avoid 2 different commands?
        {
            _currentCard.AddHistoryEntry(true);
            GetNextCard();
        }

        private void GetNextCardFailure()
        {
            _currentCard.AddHistoryEntry(false);
            GetNextCard();
        }

        private void GetNextCard()
        {
            _currentCard = myDeck.GetNextCard();
            ShowingFront = true;
            RefreshTextDiplay();
        }

        private void RefreshTextDiplay()
        {
            if (_currentCard == null)
            {
                _currentCard = myDeck.GetNextCard();
            }

            CurrentCardText = ShowingFront ? _currentCard.Front : _currentCard.Back;
            CardSuccesses = _currentCard.HistoryEntries.Where(entry => entry.Success).Count();
        }
    }
}

