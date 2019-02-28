using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SQLiteTesting
{
    class Program
    {
        private static SQLiteConnection m_dbConnection;

        static void Main(string[] args)
        {
            // Create DB
            //SQLiteConnection.CreateFile("MyFirstDB.sqlite");

            // Connect
            m_dbConnection = new SQLiteConnection("Data Source=MyFirstDB.sqlite;Version=3;");
            m_dbConnection.Open();

            string sql;
            SQLiteCommand command;

            // Create table
            /*sql = "create table highscores (name varchar(20), score int)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();*/

            // Insert into table
            sql = String.Format("insert into highscores (name, score) values ('Me{0}', {0})", new System.Random().Next());
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = String.Format("insert into highscores (name, score) values ('Myself{0}', {0})", new System.Random().Next());
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = String.Format("insert into highscores (name, score) values ('And I{0}', {0})", new System.Random().Next());
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            // Count table
            sql = "select count(*) from highscores";
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            int rowCount = Convert.ToInt32(command.ExecuteScalar());

            // Query table
            sql = "select * from highscores order by score desc";
            command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            Console.WriteLine("READING {0} RECORDS", rowCount);
            while(reader.Read())
            {
                Console.WriteLine("Name: {0} - Score: {1}", reader["name"], reader["score"]);
            }
            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }
}
