using FlashFrancais.ViewModels;
using System.Windows.Controls;
using System.Linq;

namespace FlashFrancais
{
    public partial class FlashDeckControl : UserControl
    {
        public FlashDeckControl()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            FlashDeckViewModel viewModel = DataContext as FlashDeckViewModel;
            string[] deckNamesToView = FlashDeckViewingConfig.CheckedDeckNames.Where(checkedDeckName => checkedDeckName.IsChecked).Select(x => x.DeckName).ToArray();
            viewModel.ReloadWithDeckNames(deckNamesToView);
        }
    }
}
