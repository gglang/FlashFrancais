using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace FlashFrancais
{
    
    class FlashDeckViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _showingFront;
        private string _currentCardText;
        private int _cardSuccesses;
        private Card _currentCard;

        private FlashDeck myDeck;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public FlashDeckViewModel()
        {
            //string relativePath = @"..\..\..\Decks\pronominalVerbs.csv";
            string relativePath = @"..\..\..\Decks\debugDeck.csv";

            string absolutePath = Path.GetFullPath(relativePath);
            myDeck = FlashDeck.FromPath(@absolutePath); // TODO Dependency injection in WPF XAML instantiated viewmodels?
            // TODO How to use .resx or resource dictionaries to store paths?
            _showingFront = true;
            GetNextCard();
        }

        public int CardSuccesses
        {
            get
            {
                return _cardSuccesses;
            }
            private set
            {
                _cardSuccesses = value;
                RaisePropertyChangedEvent("CardSuccesses");
            }
        }

        public string CurrentCardText
        {
            get
            {
                return _currentCardText;
            }
            private set
            {
                _currentCardText = value;
                RaisePropertyChangedEvent("CurrentCardText");
            }
        }

        public bool ShowingFront
        {
            get
            {
                return _showingFront;
            }
            set
            {
                _showingFront = value;
                RaisePropertyChangedEvent("ShowingFront");
            }
        }

        public ICommand FlipCurrentCardCommand
        {
            get
            {
                return new DelegateCommand(FlipCurrectCard);
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

