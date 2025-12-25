using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using База_Данных_Городских_Автобусов;

public static class DatabaseInitializer
{
    public static void Initialize()
    {
        try
        {
            // Создаем простое подключение без использования статического конструктора
            var connectionString = "Data Source=City-Bus.db";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // Простая проверка
                using (var command = new SqliteCommand("SELECT 1", connection))
                {
                    command.ExecuteScalar();
                }
                connection.Close();
            }

            DataBase.CreateAllTables();
            MessageBox.Show("База данных инициализирована успешно!", "Готово");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка");
        }
    }
}