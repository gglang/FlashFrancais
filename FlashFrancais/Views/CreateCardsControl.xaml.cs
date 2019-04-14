using Autofac;
using FlashFrancais.CardServers;
using FlashFrancais.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FlashFrancais
{
    /// <summary>
    /// Interaction logic for CreateCards.xaml
    /// </summary>
    public partial class CreateCardsControl : UserControl
    {
        private Database _db;
        private CardServer _cardServer;
        private string _selectedDelimiter = ",,";
        public ObservableCollection<string> _deckNames { get; set; }

        public CreateCardsControl()
        {
            InitializeComponent();
            _db = GlobalFactory.Container.Resolve<Database>();
            _cardServer = GlobalFactory.Container.Resolve<CardServer>();
            _deckNames = new ObservableCollection<string>();
            Refresh();
        }

        public void Refresh()
        {
            _db.GetDeckNames().ToList().ForEach(
                    deckName =>
                    {
                        if (!_deckNames.Contains(deckName))
                        {
                            _deckNames.Add(deckName);
                        }
                    }
            );
            DeckSelectComboBox.ItemsSource = _deckNames;
            DeckSelectComboBox.SelectedIndex = 0;
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FlashDeck cardsToAdd = FlashDeck.FromCSV(_cardServer, openFileDialog.FileName, GetSelectedFlashDeck(), _selectedDelimiter); // Make deck configurable
                _db.AddCardsToDeck(cardsToAdd.GetCards(), cardsToAdd.Name);
            }
        }

        private void BtnSubmitNewDeck_Click(object sender, RoutedEventArgs e)
        {
            if (_deckNames.Contains(NewDeckTextBox.Text))
            {
                NewDeckTextBox.Clear();
                return;
            }
            _deckNames.Add(NewDeckTextBox.Text);
            DeckSelectComboBox.SelectedIndex = _deckNames.Count - 1;
            NewDeckTextBox.Clear();
        }

        private string GetSelectedFlashDeck()
        {
            return DeckSelectComboBox.SelectedItem as string;
        }

        private void BtnSubmitNewCard_Click(object sender, RoutedEventArgs e)
        {
            string cardFront = FrontTextbox.Text;
            string cardBack = BackTextbox.Text;
            FrontTextbox.Clear();
            BackTextbox.Clear();
            Card cardToAdd = new Card(cardFront, cardBack);
            _db.AddCardToDeck(cardToAdd, GetSelectedFlashDeck());
        }
    }
}
