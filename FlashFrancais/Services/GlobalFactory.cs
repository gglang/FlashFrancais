using Autofac;
using FlashFrancais.ViewModel;
using System.Data.SQLite;

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
            builder.Register(ctx =>
            {
                var sqliteConnectionString = "Data Source=C:/Dev/FlashFrancais/Decks/FlashDB.sqlite;Version=3;";
                var sqliteConnection = new SQLiteConnection(sqliteConnectionString);
                sqliteConnection.Open(); // TODO Is this sort of stuff kosher in autofac?
                return new SQLiteDatabase(sqliteConnection);
            }).As<Database>();

            builder.RegisterType<FlashDeckViewModel>();

            Container = builder.Build();
        }
    }
}