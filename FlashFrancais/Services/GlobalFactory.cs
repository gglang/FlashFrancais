using Autofac;
using FlashFrancais.ViewModels;
using System.Data.SQLite;
using System.IO;

namespace FlashFrancais.Services
{
    public class GlobalFactory
    {
        public static IContainer Container { get; }

        static GlobalFactory()
        {
            var builder = new ContainerBuilder();

            // TODO perhaps refactor this to be more abstracted
            // Ala: https://autofaccn.readthedocs.io/en/latest/faq/injecting-configured-parameters.html
            builder.RegisterType<ConnectionStringProvider>(); // TODO Resolve this into an interface?
            builder.RegisterType<SQLiteDatabase>().SingleInstance().As<Database>();
            builder.RegisterType<FlashDeckViewModel>(); // TODO Resolve this into an interface?

            Container = builder.Build();
        }
    }
}