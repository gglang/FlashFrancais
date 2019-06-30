using FlashFrancais.CardServers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace FlashFrancais.Views
{
    public partial class DebugWindow : Window
    {
        public ObservableCollection<string> _debugCards { get; set; }
        private IEnumerable<AnkiCardIntervalData> _upcomingCards { get; set; }

        public DebugWindow()
        {
            InitializeComponent();
            _debugCards = new ObservableCollection<string>();
            DataContext = this;
        }

        public void InitDebugging(IEnumerable<AnkiCardIntervalData> upcomingCards)
        {
            _upcomingCards = upcomingCards;
            Refresh();
        }

        public void Refresh()
        {
            _debugCards.Clear();
            _upcomingCards.ToList().ForEach(x => _debugCards.Add(x.ToString()));
        }
    }
}
