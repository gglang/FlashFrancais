using Autofac;
using FlashFrancais.CardServers;
using FlashFrancais.ViewModels;
using System;
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
            builder.RegisterType<SM2CardServer>().As<CardServer>();
            builder.RegisterType<SQLiteFlashDeckProvider>().SingleInstance().As<FlashDeckProvider>(); // TODO I am not convinced of this pattern, but it is probably good at least for a class with SRP to provide the deck...
            builder.RegisterType<ConnectionStringProvider>(); // TODO Resolve this into an interface? Perhaps it should provide Connections, rather than strings, more secure?
            builder.RegisterType<SQLiteDatabase>().SingleInstance().As<Database>();
            builder.RegisterType<FlashDeckViewModel>(); // TODO Resolve this into an interface?

            Container = builder.Build();
        }

        internal static T Resolve<T>()
        {
            throw new NotImplementedException();
        }
    }
}