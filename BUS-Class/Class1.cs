using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BUS_Class
{
    public class DataBase
    {
        public static string connectionString = "Data Source=City-Bus.db";

        // Для тестирования
        public static void SetTestConnectionString(string testConnectionString)
        {
            connectionString = testConnectionString;
        }

        public static void ResetConnectionString()
        {
            connectionString = "Data Source=City-Bus.db";
        }

        // ==================== CREATE ====================
        public static bool CreateAllTables()
        {
            try
            {
                // Создаем таблицы в правильном порядке (сначала родительские, потом дочерние)
                CreateTableUsers();
                CreateTableRoutes();
                CreateTableBuses();

                // Ждем немного чтобы таблицы создались
                System.Threading.Thread.Sleep(100);

                CreateTableSchedule();
                CreateTableTickets();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка создания таблиц: {ex.Message}");
                return false;
            }
        }

        private static bool CreateTableUsers()
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"DROP TABLE IF EXISTS Users";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"CREATE TABLE Users (
                               user_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                               username TEXT NOT NULL UNIQUE,
                               password_hash TEXT NOT NULL,
                               full_name TEXT NOT NULL,
                               role TEXT NOT NULL,
                               is_active BOOLEAN DEFAULT 1,
                               created_date DATETIME DEFAULT CURRENT_TIMESTAMP)";
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка создания таблицы Users: {ex.Message}");
                    return false;
                }
            }
        }


        private static bool CreateTableRoutes()
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Routes (
                            route_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                            route_number TEXT NOT NULL,
                            departure_city TEXT NOT NULL,
                            arrival_city TEXT NOT NULL,
                            distance TEXT,
                            duration_minutes INTEGER,
                            is_active BOOLEAN DEFAULT 1,
                            created_date DATETIME DEFAULT CURRENT_TIMESTAMP
                        )";
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private static bool CreateTableBuses()
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Buses (
                            bus_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                            plate_number TEXT NOT NULL UNIQUE,
                            brand TEXT NOT NULL,
                            model TEXT NOT NULL,
                            capacity INTEGER NOT NULL,
                            year INTEGER,
                            is_active BOOLEAN DEFAULT 1,
                            created_date DATETIME DEFAULT CURRENT_TIMESTAMP
                        )";
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private static bool CreateTableSchedule()
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Schedule (
                            schedule_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                            route_id INTEGER NOT NULL,
                            bus_id INTEGER NOT NULL,
                            departure_time DATETIME NOT NULL,
                            arrival_time DATETIME NOT NULL,
                            price DECIMAL(10,2) NOT NULL,
                            status TEXT NOT NULL,
                            available_seats INTEGER NOT NULL,
                            created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
                            FOREIGN KEY (route_id) REFERENCES Routes(route_id),
                            FOREIGN KEY (bus_id) REFERENCES Buses(bus_id)
                        )";
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private static bool CreateTableTickets()
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Tickets (
                            ticket_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                            ticket_number TEXT NOT NULL UNIQUE,
                            schedule_id INTEGER NOT NULL,
                            passenger_name TEXT NOT NULL,
                            seat_number INTEGER NOT NULL,
                            price DECIMAL(10,2) NOT NULL,
                            sale_date DATETIME NOT NULL,
                            is_returned BOOLEAN DEFAULT 0,
                            created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
                            FOREIGN KEY (schedule_id) REFERENCES Schedule(schedule_id),
                            UNIQUE(schedule_id, seat_number)
                        )";
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // ==================== SELECT ====================
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand("SELECT * FROM Users ORDER BY user_id", conn);

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        // Добавляем столбцы в DataTable
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dt.Columns.Add(reader.GetName(i));
                        }

                        // Заполняем DataTable данными
                        while (reader.Read())
                        {
                            DataRow row = dt.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader.GetValue(i);
                            }
                            dt.Rows.Add(row);
                        }
                    }
                }
                catch (Exception)
                {
                    // В тестах не показываем MessageBox
                }
            }
            return dt;
        }

        public static DataTable GetAllRoutes()
        {
            DataTable dt = new DataTable();
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand("SELECT * FROM Routes ORDER BY route_id", conn);

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        // Добавляем столбцы в DataTable
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dt.Columns.Add(reader.GetName(i));
                        }

                        // Заполняем DataTable данными
                        while (reader.Read())
                        {
                            DataRow row = dt.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader.GetValue(i);
                            }
                            dt.Rows.Add(row);
                        }
                    }
                }
                catch (Exception)
                {
                    // В тестах не показываем MessageBox
                }
            }
            return dt;
        }

        public static DataTable GetAllBuses()
        {
            DataTable dt = new DataTable();
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand("SELECT * FROM Buses ORDER BY bus_id", conn);

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        // Добавляем столбцы в DataTable
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dt.Columns.Add(reader.GetName(i));
                        }

                        // Заполняем DataTable данными
                        while (reader.Read())
                        {
                            DataRow row = dt.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader.GetValue(i);
                            }
                            dt.Rows.Add(row);
                        }
                    }
                }
                catch (Exception)
                {
                    // В тестах не показываем MessageBox
                }
            }
            return dt;
        }

        // ==================== INSERT ====================
        public static bool InsertUser(string username, string passwordHash, string fullName, string role, bool isActive)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO Users (username, password_hash, full_name, role, is_active) 
                                      VALUES (@username, @passwordHash, @fullName, @role, @isActive)";
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@fullName", fullName);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@isActive", isActive ? 1 : 0);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool InsertRoute(string routeNumber, string departureCity, string arrivalCity,
                                       string distance, int durationMinutes, bool isActive)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO Routes (route_number, departure_city, arrival_city, 
                                      distance, duration_minutes, is_active) 
                                      VALUES (@routeNumber, @departureCity, @arrivalCity, 
                                      @distance, @durationMinutes, @isActive)";
                    cmd.Parameters.AddWithValue("@routeNumber", routeNumber);
                    cmd.Parameters.AddWithValue("@departureCity", departureCity);
                    cmd.Parameters.AddWithValue("@arrivalCity", arrivalCity);
                    cmd.Parameters.AddWithValue("@distance", distance);
                    cmd.Parameters.AddWithValue("@durationMinutes", durationMinutes);
                    cmd.Parameters.AddWithValue("@isActive", isActive ? 1 : 0);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool InsertBus(string plateNumber, string brand, string model,
                                     int capacity, int year, bool isActive)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO Buses (plate_number, brand, model, capacity, year, is_active) 
                                      VALUES (@plateNumber, @brand, @model, @capacity, @year, @isActive)";
                    cmd.Parameters.AddWithValue("@plateNumber", plateNumber);
                    cmd.Parameters.AddWithValue("@brand", brand);
                    cmd.Parameters.AddWithValue("@model", model);
                    cmd.Parameters.AddWithValue("@capacity", capacity);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@isActive", isActive ? 1 : 0);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // ==================== UPDATE ====================
        public static bool UpdateUser(int userId, string username, string passwordHash,
                                     string fullName, string role, bool isActive)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"UPDATE Users SET 
                                      username = @username, 
                                      password_hash = @passwordHash, 
                                      full_name = @fullName, 
                                      role = @role, 
                                      is_active = @isActive 
                                      WHERE user_id = @userId";
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@fullName", fullName);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@isActive", isActive ? 1 : 0);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool UpdateRoute(int routeId, string routeNumber, string departureCity,
                                       string arrivalCity, string distance, int durationMinutes, bool isActive)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"UPDATE Routes SET 
                                      route_number = @routeNumber, 
                                      departure_city = @departureCity, 
                                      arrival_city = @arrivalCity, 
                                      distance = @distance, 
                                      duration_minutes = @durationMinutes, 
                                      is_active = @isActive 
                                      WHERE route_id = @routeId";
                    cmd.Parameters.AddWithValue("@routeId", routeId);
                    cmd.Parameters.AddWithValue("@routeNumber", routeNumber);
                    cmd.Parameters.AddWithValue("@departureCity", departureCity);
                    cmd.Parameters.AddWithValue("@arrivalCity", arrivalCity);
                    cmd.Parameters.AddWithValue("@distance", distance);
                    cmd.Parameters.AddWithValue("@durationMinutes", durationMinutes);
                    cmd.Parameters.AddWithValue("@isActive", isActive ? 1 : 0);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool UpdateBus(int busId, string plateNumber, string brand, string model,
                                     int capacity, int year, bool isActive)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"UPDATE Buses SET 
                                      plate_number = @plateNumber, 
                                      brand = @brand, 
                                      model = @model, 
                                      capacity = @capacity, 
                                      year = @year, 
                                      is_active = @isActive 
                                      WHERE bus_id = @busId";
                    cmd.Parameters.AddWithValue("@busId", busId);
                    cmd.Parameters.AddWithValue("@plateNumber", plateNumber);
                    cmd.Parameters.AddWithValue("@brand", brand);
                    cmd.Parameters.AddWithValue("@model", model);
                    cmd.Parameters.AddWithValue("@capacity", capacity);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@isActive", isActive ? 1 : 0);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // ==================== DELETE ====================
        public static bool DeleteUser(int userId)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand("DELETE FROM Users WHERE user_id = @userId", conn);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool DeleteRoute(int routeId)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand("DELETE FROM Routes WHERE route_id = @routeId", conn);
                    cmd.Parameters.AddWithValue("@routeId", routeId);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool DeleteBus(int busId)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand("DELETE FROM Buses WHERE bus_id = @busId", conn);
                    cmd.Parameters.AddWithValue("@busId", busId);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // ==================== ДОПОЛНИТЕЛЬНЫЕ МЕТОДЫ ====================
        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);

                // Убедимся что хеш всегда одинаковый
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2")); // Всегда в нижнем регистре
                }
                return sb.ToString().ToLower(); // Приводим к нижнему регистру
            }
        }

        public static bool CheckLogin(string username, string passwordHash)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand(
                        "SELECT COUNT(*) FROM Users WHERE username = @username AND password_hash = @passwordHash AND is_active = 1",
                        conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static void ClearTestData()
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = "DELETE FROM Users";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Routes";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Buses";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Schedule";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Tickets";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // Игнорируем ошибки при очистке
                }
            }
        }

        // Метод для получения пользователя по ID (для тестирования)
        public static DataRow GetUserById(int userId)
        {
            DataTable dt = new DataTable();
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand("SELECT * FROM Users WHERE user_id = @userId", conn);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        // Добавляем столбцы в DataTable
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dt.Columns.Add(reader.GetName(i));
                        }

                        // Заполняем DataTable данными
                        if (reader.Read())
                        {
                            DataRow row = dt.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader.GetValue(i);
                            }
                            dt.Rows.Add(row);
                            return dt.Rows[0];
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        // Метод для получения маршрута по ID (для тестирования)
        public static DataRow GetRouteById(int routeId)
        {
            DataTable dt = new DataTable();
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand("SELECT * FROM Routes WHERE route_id = @routeId", conn);
                    cmd.Parameters.AddWithValue("@routeId", routeId);

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        // Добавляем столбцы в DataTable
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dt.Columns.Add(reader.GetName(i));
                        }

                        // Заполняем DataTable данными
                        if (reader.Read())
                        {
                            DataRow row = dt.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader.GetValue(i);
                            }
                            dt.Rows.Add(row);
                            return dt.Rows[0];
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        // Метод для получения автобуса по ID (для тестирования)
        public static DataRow GetBusById(int busId)
        {
            DataTable dt = new DataTable();
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand("SELECT * FROM Buses WHERE bus_id = @busId", conn);
                    cmd.Parameters.AddWithValue("@busId", busId);

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        // Добавляем столбцы в DataTable
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dt.Columns.Add(reader.GetName(i));
                        }

                        // Заполняем DataTable данными
                        if (reader.Read())
                        {
                            DataRow row = dt.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader.GetValue(i);
                            }
                            dt.Rows.Add(row);
                            return dt.Rows[0];
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }
    }

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }

    public class Route
    {
        public int RouteId { get; set; }
        public string RouteNumber { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public string Distance { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; }
    }

    public class Bus
    {
        public int BusId { get; set; }
        public string PlateNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }
    }

    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int RouteId { get; set; }
        public int BusId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public int AvailableSeats { get; set; }
    }

    public class Ticket
    {
        public int TicketId { get; set; }
        public string TicketNumber { get; set; }
        public int ScheduleId { get; set; }
        public string PassengerName { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
        public DateTime SaleDate { get; set; }
        public bool IsReturned { get; set; }
    }

    public static class ValidationHelper
    {
        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            // Логин должен содержать только буквы, цифры и подчеркивания
            return System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$");
        }

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return false;

            return true;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Простая проверка email
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool IsValidPlateNumber(string plateNumber)
        {
            if (string.IsNullOrWhiteSpace(plateNumber))
                return false;

            // Простая проверка госномера (пример: А123ВС77)
            return System.Text.RegularExpressions.Regex.IsMatch(plateNumber, @"^[А-ЯA-Z]\d{3}[А-ЯA-Z]{2}\d{2,3}$");
        }

        public static bool IsValidRouteNumber(string routeNumber)
        {
            if (string.IsNullOrWhiteSpace(routeNumber))
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(routeNumber, @"^\d+[A-Za-z]?$");
        }

        public static bool IsValidCityName(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(city, @"^[А-ЯA-Z][а-яa-z]+(?:[- ][А-ЯA-Z][а-яa-z]+)*$");
        }
    }
}