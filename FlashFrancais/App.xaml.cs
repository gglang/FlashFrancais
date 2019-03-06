using GalaSoft.MvvmLight.Threading;
using System.Windows;

namespace FlashFrancais
{
    public partial class App : Window
    {
        public App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
