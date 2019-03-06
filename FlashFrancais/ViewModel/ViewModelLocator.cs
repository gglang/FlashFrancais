using System.Reflection;
using FlashFrancais.Services;
using Autofac;

namespace FlashFrancais.ViewModel
{
    public class ViewModelLocator
    {
        public FlashDeckViewModel FlashDeckViewModel => GlobalFactory.Container.Resolve<FlashDeckViewModel>();
    }
}