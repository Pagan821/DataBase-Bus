using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace База_Данных_Городских_Автобусов
{
    internal class DataBase
    {
        static string connectionString = "City-Bus.db";


        public static void CreateAllTables()
        {
            CreateTableUsers();
        }


        private static void CreateTableUsers()
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Users (
                                   user_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                   username TEXT NOT NULL UNIQUE,
                                   password_hash TEXT NOT NULL,
                                   full_name TEXT NOT NULL,
                                   role TEXT NOT NULL,
                                   is_active BOOLEAN DEFAULT 1,
                                   created_date DATETIME DEFAULT CURRENT_TIMESTAMP)";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Таблица пользователей создана успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания таблицы пользователей: {ex}");
            }
            finally
            {
                conn.Close();
            }


        }





    }
}
