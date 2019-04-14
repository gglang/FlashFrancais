using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _lastSelectedTab = PracticeTab;
        }

        private TabItem _lastSelectedTab;

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl senderTabControl = sender as TabControl;
            if(senderTabControl == null)
            {
                return;
            }

            if(PracticeTab.IsSelected && _lastSelectedTab != PracticeTab)
            {
                _lastSelectedTab = PracticeTab;
                var flashDeckControl = PracticeTab.Content as FlashDeckControl;
                flashDeckControl?.Refresh();
            }
            if(CreateTab.IsSelected && _lastSelectedTab != CreateTab)
            {
                _lastSelectedTab = CreateTab;
                var createCardsControl = CreateTab.Content as CreateCardsControl;
                createCardsControl?.Refresh();
            }
            if(ViewingConfigTab.IsSelected && _lastSelectedTab != ViewingConfigTab)
            {
                _lastSelectedTab = ViewingConfigTab;
                var viewingConfigControl = ViewingConfigTab.Content as FlashDeckViewingConfig;
                viewingConfigControl?.Refresh();
            }
        }
    }
}
