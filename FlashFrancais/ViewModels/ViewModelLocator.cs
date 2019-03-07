using System.Reflection;
using FlashFrancais.Services;
using Autofac;

namespace FlashFrancais.ViewModels
{
    public class ViewModelLocator
    {
        public FlashDeckViewModel FlashDeckViewModel => GlobalFactory.Container.Resolve<FlashDeckViewModel>();
    }
}