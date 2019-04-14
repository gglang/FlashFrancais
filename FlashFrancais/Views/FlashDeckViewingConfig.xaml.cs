using FlashFrancais.Services;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;
using Autofac;

namespace FlashFrancais
{
    /// <summary>
    /// Interaction logic for FlashDeckViewingConfig.xaml
    /// </summary>
    public partial class FlashDeckViewingConfig : UserControl
    {
        public static ObservableCollection<CheckedDeckName> CheckedDeckNames { get; set; }
        private Database _db;

        public FlashDeckViewingConfig()
        {
            InitializeComponent();
            CheckedDeckNames = new ObservableCollection<CheckedDeckName>();
            _db = GlobalFactory.Container.Resolve<Database>();
            DataContext = this;
            Refresh();
        }

        public void Refresh()
        {
            var deckNames = _db.GetDeckNames();
            deckNames.ToList().ForEach(deckName =>
                {
                    if((CheckedDeckNames.Where(checkedDeckName => checkedDeckName.DeckName == deckName).Count() == 0))
                    {
                        CheckedDeckNames.Add(new CheckedDeckName() { DeckName = deckName, IsChecked = true });
                    }
                }
            );
        }
    }

    public class CheckedDeckName
    {
        public string DeckName { get; set; }
        public bool IsChecked { get; set; }
    }
}
