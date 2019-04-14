using Autofac;
using FlashFrancais.CardServers;
using FlashFrancais.Services;
using FlashFrancais.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlashFrancais
{
    /// <summary>
    /// Interaction logic for CreateCards.xaml
    /// </summary>
    public partial class CreateCardsControl : UserControl
    {
        private Database _db;
        private CardServer _cardServer;
        private string _selectedFlashDeck = "FlashFrench";
        private string _selectedDelimiter = ",,";

        public CreateCardsControl()
        {
            InitializeComponent();
            _db = GlobalFactory.Container.Resolve<Database>();
            _cardServer = GlobalFactory.Container.Resolve<CardServer>();
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FlashDeck cardsToAdd = FlashDeck.FromCSV(_cardServer, openFileDialog.FileName, _selectedFlashDeck, _selectedDelimiter); // Make deck configurable
                _db.AddCardsToDeck(cardsToAdd.GetCards(), cardsToAdd.Name);
            }
        }

        private void BtnSubmitNewCard_Click(object sender, RoutedEventArgs e)
        {
            string cardFront = FrontTextbox.Text;
            string cardBack = BackTextbox.Text;
            FrontTextbox.Clear();
            BackTextbox.Clear();
            Card cardToAdd = new Card(cardFront, cardBack);
            _db.AddCardToDeck(cardToAdd, _selectedFlashDeck);
        }
    }
}
