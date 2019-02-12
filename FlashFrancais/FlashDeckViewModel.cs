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
        private string currentCardText;
        private bool showingFront = true;
        private FlashCard currentCard;

        /*private FlashDeck myDeck = new FlashDeck(
            new List<FlashCard>(
                    new FlashCard[]
                    {
                        new FlashCard("bonjour", "hello"),
                        new FlashCard("monde", "world"),
                        new FlashCard("francais", "french")
                    }
                )
        );*/

        private FlashDeck myDeck;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public FlashDeckViewModel()
        {
            string relativePath = @"..\..\..\Decks\pronominalVerbs.csv";
            string absolutePath = Path.GetFullPath(relativePath);
            myDeck = FlashDeck.FromPath(@absolutePath); // TODO Dependency injection in WPF XAML instantiated viewmodels?
            // TODO How to use .resx or resource dictionaries to store paths?
            GetNextCard();
        }

        public string CurrentCardText
        {
            get
            {
                Console.WriteLine("DISPLAYING");
                return currentCardText;
            }
            set
            {
                currentCardText = value;
                RaisePropertyChangedEvent("CurrentCardText");
            }
        }

        public ICommand FlipCurrentCardCommand
        {
            get
            {
                Console.WriteLine("Flip command inc");
                return new DelegateCommand(FlipCurrectCard);
            }
        }

        public ICommand GetNextCardCommand
        {
            get
            {
                Console.WriteLine("Get next command inc");
                return new DelegateCommand(GetNextCard);
            }
        }

        private void FlipCurrectCard()
        {
            Console.WriteLine("Flipping card");
            showingFront = !showingFront;
            RefreshTextDiplay();
        }

        private void GetNextCard()
        {
            Console.WriteLine("Getting next card");
            currentCard = myDeck.GetNextCard();
            showingFront = true;
            RefreshTextDiplay();
        }

        private void RefreshTextDiplay()
        {
            if (currentCard == null)
            {
                currentCard = myDeck.GetNextCard();
            }

            CurrentCardText = showingFront ? currentCard.Front : currentCard.Back;
        }
    }
}

