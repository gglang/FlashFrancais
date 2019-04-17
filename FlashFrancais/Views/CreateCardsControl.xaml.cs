using Autofac;
using FlashFrancais.CardServers;
using FlashFrancais.Services;
using FlashFrancais.ViewModels;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
                AddDefaultHistoryToActivateNewCards(cardsToAdd);
                SendEmailNotificationOfNewCards(cardsToAdd);
                FlashDeckViewModel.ReloadRequested = true;
            }
        }

        private void AddDefaultHistoryToActivateNewCards(FlashDeck cardsAdded)
        {
            foreach(Card cardAdded in cardsAdded?.GetCards())
            {
                _db.AddHistoryEntry(cardAdded, TrialPerformance.Fail);
            }
        }

        private void SendEmailNotificationOfNewCards(FlashDeck cardsAdded)
        {
            if (cardsAdded == null || cardsAdded.GetCards().Count() == 0)
                return;

            string emailSubject =
                string.Format("Les {0} dernières fiches qui étaient ajoutées "+
                "à la petite application de Gerald",
                cardsAdded.GetCards().Count());
            string emailStart = "Salut Dominic,\n\nC'est moi, le robot fait par Gerald pour vous "+
                                "informer de ses choses. Voila, les derniers développments:\n\n";
            string emailContent = "";
            string emailEnd = "Merçi de votre attention,\nLe petit robot amical de Gerald";
            
            var cardListStringBuilder = new StringBuilder();
            foreach (Card card in cardsAdded.GetCards())
            {
                cardListStringBuilder.Append(card.ToString());
                cardListStringBuilder.Append("\n\n");
            }
            emailContent = cardListStringBuilder.ToString();
            string fullEmailBody = emailStart + emailContent + emailEnd;

            SendEmail(emailSubject, fullEmailBody);
        }

        private void SendEmail(string subject, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("lepetitrobot1984@gmail.com", "lepetitrobot1"),
                EnableSsl = true
            };

            MailMessage message = new MailMessage();
            message.From = new MailAddress("lepetitrobot1984@gmail.com");
            message.To.Add("geraldlang92@gmail.com");
            message.To.Add("dominicaudet@cooptel.qc.ca");
            message.Subject = subject;
            message.Body = body;
            client.SendMailAsync(message);
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
