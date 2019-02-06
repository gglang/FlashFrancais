﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        private FlashDeck myDeck = new FlashDeck(
            new List<FlashCard>(
                    new FlashCard[]
                    {
                        new FlashCard("bonjour", "hello"),
                        new FlashCard("monde", "world"),
                        new FlashCard("francais", "french")
                    }
                )
        );

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public FlashDeckViewModel()
        {
            Console.WriteLine("I GOT MADE");
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
