using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace База_Данных_Городских_Автобусов
{
    internal class DataBase
    {
        static string connectionString = "Data Source=City-Bus.db";

        // ==================== CREATE ====================

        public static void CreateAllTables()
        {
            CreateTableUsers();
            CreateTableRoutes();
            CreateTableBuses();
            CreateTableSchedule();
            CreateTableTickets();
        }

        private static void CreateTableUsers()
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания таблицы пользователей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private static void CreateTableRoutes()
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания таблицы маршрутов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private static void CreateTableBuses()
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания таблицы автобусов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private static void CreateTableSchedule()
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания таблицы расписания: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private static void CreateTableTickets()
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания таблицы билетов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // ==================== SELECT ====================

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SQLiteConnection conn = new SQLiteConnection(connectionString);

            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Users ORDER BY user_id", conn);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dt.Columns.Add(reader.GetName(i));
                    }

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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения пользователей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }

        public static DataTable GetAllRoutes()
        {
            DataTable dt = new DataTable();
            SQLiteConnection conn = new SQLiteConnection(connectionString);

            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Routes ORDER BY route_id", conn);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dt.Columns.Add(reader.GetName(i));
                    }

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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения маршрутов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }

        public static DataTable GetAllBuses()
        {
            DataTable dt = new DataTable();
            SQLiteConnection conn = new SQLiteConnection(connectionString);

            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Buses ORDER BY bus_id", conn);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dt.Columns.Add(reader.GetName(i));
                    }

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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения автобусов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }

        public static DataTable GetAllSchedules()
        {
            DataTable dt = new DataTable();
            SQLiteConnection conn = new SQLiteConnection(connectionString);

            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(
                    @"SELECT s.*, r.route_number, r.departure_city, r.arrival_city, 
                    b.plate_number, b.brand, b.model 
                    FROM Schedule s 
                    JOIN Routes r ON s.route_id = r.route_id 
                    JOIN Buses b ON s.bus_id = b.bus_id 
                    ORDER BY s.departure_time", conn);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dt.Columns.Add(reader.GetName(i));
                    }

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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения расписания: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }

        public static DataTable GetAllTickets()
        {
            DataTable dt = new DataTable();
            SQLiteConnection conn = new SQLiteConnection(connectionString);

            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(
                    @"SELECT t.*, s.departure_time, r.route_number, r.departure_city, r.arrival_city 
                    FROM Tickets t 
                    JOIN Schedule s ON t.schedule_id = s.schedule_id 
                    JOIN Routes r ON s.route_id = r.route_id 
                    ORDER BY t.sale_date DESC", conn);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dt.Columns.Add(reader.GetName(i));
                    }

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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения билетов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }

        // ==================== ПРОВЕРКИ СУЩЕСТВОВАНИЯ (НОВЫЕ МЕТОДЫ) ====================

        /// <summary>
        /// Проверяет, существует ли уже такой маршрут
        /// </summary>
        public static bool CheckRouteExists(string routeNumber, string departure, string arrival, int excludeId = 0)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                string query = @"SELECT COUNT(*) FROM Routes 
                               WHERE route_number = @routeNumber 
                               AND departure_city = @departure 
                               AND arrival_city = @arrival";

                if (excludeId > 0)
                {
                    query += " AND route_id != @excludeId";
                }

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@routeNumber", routeNumber);
                cmd.Parameters.AddWithValue("@departure", departure);
                cmd.Parameters.AddWithValue("@arrival", arrival);

                if (excludeId > 0)
                {
                    cmd.Parameters.AddWithValue("@excludeId", excludeId);
                }

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки маршрута: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Проверяет, существует ли уже такой автобус
        /// </summary>
        public static bool CheckBusExists(string plateNumber, int excludeId = 0)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Buses WHERE plate_number = @plateNumber";

                if (excludeId > 0)
                {
                    query += " AND bus_id != @excludeId";
                }

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@plateNumber", plateNumber);

                if (excludeId > 0)
                {
                    cmd.Parameters.AddWithValue("@excludeId", excludeId);
                }

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки автобуса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Проверяет, существует ли уже такой рейс
        /// </summary>
        public static bool CheckScheduleExists(int routeId, int busId, DateTime departureTime, int excludeId = 0)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                string query = @"SELECT COUNT(*) FROM Schedule 
                               WHERE route_id = @routeId 
                               AND bus_id = @busId 
                               AND departure_time = @departureTime";

                if (excludeId > 0)
                {
                    query += " AND schedule_id != @excludeId";
                }

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@routeId", routeId);
                cmd.Parameters.AddWithValue("@busId", busId);
                cmd.Parameters.AddWithValue("@departureTime", departureTime);

                if (excludeId > 0)
                {
                    cmd.Parameters.AddWithValue("@excludeId", excludeId);
                }

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки рейса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Проверяет, существует ли уже такой билет
        /// </summary>
        public static bool CheckTicketExists(string ticketNumber, int excludeId = 0)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Tickets WHERE ticket_number = @ticketNumber";

                if (excludeId > 0)
                {
                    query += " AND ticket_id != @excludeId";
                }

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);

                if (excludeId > 0)
                {
                    cmd.Parameters.AddWithValue("@excludeId", excludeId);
                }

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки билета: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Проверяет, существует ли уже такой пользователь
        /// </summary>
        public static bool CheckUserExists(string username, int excludeId = 0)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE username = @username";

                if (excludeId > 0)
                {
                    query += " AND user_id != @excludeId";
                }

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);

                if (excludeId > 0)
                {
                    cmd.Parameters.AddWithValue("@excludeId", excludeId);
                }

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки пользователя: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Проверяет, свободно ли место в рейсе
        /// </summary>
        public static bool CheckSeatAvailability(int scheduleId, int seatNumber, int excludeTicketId = 0)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                string query = @"SELECT COUNT(*) FROM Tickets 
                               WHERE schedule_id = @scheduleId 
                               AND seat_number = @seatNumber 
                               AND is_returned = 0";

                if (excludeTicketId > 0)
                {
                    query += " AND ticket_id != @excludeTicketId";
                }

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@scheduleId", scheduleId);
                cmd.Parameters.AddWithValue("@seatNumber", seatNumber);

                if (excludeTicketId > 0)
                {
                    cmd.Parameters.AddWithValue("@excludeTicketId", excludeTicketId);
                }

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count == 0; // Место свободно, если нет записей
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки места: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // ==================== ПРОВЕРКИ СВЯЗАННЫХ ЗАПИСЕЙ ====================

        /// <summary>
        /// Проверяет, есть ли у маршрута связанные записи (рейсы, билеты)
        /// </summary>
        public static bool CheckRouteHasRelatedRecords(int routeId)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();

                // Проверяем, есть ли рейсы с этим маршрутом
                string query = @"SELECT COUNT(*) FROM Schedule WHERE route_id = @routeId";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@routeId", routeId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки связанных записей маршрута: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Проверяет, есть ли у автобуса связанные записи (рейсы)
        /// </summary>
        public static bool CheckBusHasRelatedRecords(int busId)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();

                // Проверяем, есть ли рейсы с этим автобусом
                string query = @"SELECT COUNT(*) FROM Schedule WHERE bus_id = @busId";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@busId", busId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки связанных записей автобуса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Проверяет, есть ли у рейса проданные билеты
        /// </summary>
        public static bool CheckScheduleHasTickets(int scheduleId)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();

                // Проверяем, есть ли билеты на этот рейс
                string query = @"SELECT COUNT(*) FROM Tickets 
                               WHERE schedule_id = @scheduleId 
                               AND is_returned = 0";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@scheduleId", scheduleId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки билетов рейса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Получает хеш пароля пользователя по ID
        /// </summary>
        public static string GetUserPasswordHash(int userId)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();

                string query = "SELECT password_hash FROM Users WHERE user_id = @userId";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения хеша пароля: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // ==================== INSERT ====================

        public static bool InsertUser(string username, string passwordHash, string fullName, string role, bool isActive)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления пользователя: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool InsertRoute(string routeNumber, string departureCity, string arrivalCity,
                                       string distance, int durationMinutes, bool isActive)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления маршрута: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool InsertBus(string plateNumber, string brand, string model,
                                     int capacity, int year, bool isActive)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления автобуса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool InsertSchedule(int routeId, int busId, DateTime departureTime,
                                          DateTime arrivalTime, decimal price, string status, int availableSeats)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"INSERT INTO Schedule (route_id, bus_id, departure_time, arrival_time, 
                                  price, status, available_seats) 
                                  VALUES (@routeId, @busId, @departureTime, @arrivalTime, 
                                  @price, @status, @availableSeats)";
                cmd.Parameters.AddWithValue("@routeId", routeId);
                cmd.Parameters.AddWithValue("@busId", busId);
                cmd.Parameters.AddWithValue("@departureTime", departureTime);
                cmd.Parameters.AddWithValue("@arrivalTime", arrivalTime);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@availableSeats", availableSeats);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления рейса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool InsertTicket(string ticketNumber, int scheduleId, string passengerName,
                                        int seatNumber, decimal price, DateTime saleDate, bool isReturned)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"INSERT INTO Tickets (ticket_number, schedule_id, passenger_name, 
                                  seat_number, price, sale_date, is_returned) 
                                  VALUES (@ticketNumber, @scheduleId, @passengerName, 
                                  @seatNumber, @price, @saleDate, @isReturned)";
                cmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                cmd.Parameters.AddWithValue("@scheduleId", scheduleId);
                cmd.Parameters.AddWithValue("@passengerName", passengerName);
                cmd.Parameters.AddWithValue("@seatNumber", seatNumber);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@saleDate", saleDate);
                cmd.Parameters.AddWithValue("@isReturned", isReturned ? 1 : 0);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления билета: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // ==================== UPDATE ====================

        public static bool UpdateUser(int userId, string username, string passwordHash,
                                     string fullName, string role, bool isActive)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления пользователя: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool UpdateRoute(int routeId, string routeNumber, string departureCity,
                                       string arrivalCity, string distance, int durationMinutes, bool isActive)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления маршрута: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool UpdateBus(int busId, string plateNumber, string brand, string model,
                                     int capacity, int year, bool isActive)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления автобуса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool UpdateSchedule(int scheduleId, int routeId, int busId, DateTime departureTime,
                                          DateTime arrivalTime, decimal price, string status, int availableSeats)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"UPDATE Schedule SET 
                                  route_id = @routeId, 
                                  bus_id = @busId, 
                                  departure_time = @departureTime, 
                                  arrival_time = @arrivalTime, 
                                  price = @price, 
                                  status = @status, 
                                  available_seats = @availableSeats 
                                  WHERE schedule_id = @scheduleId";
                cmd.Parameters.AddWithValue("@scheduleId", scheduleId);
                cmd.Parameters.AddWithValue("@routeId", routeId);
                cmd.Parameters.AddWithValue("@busId", busId);
                cmd.Parameters.AddWithValue("@departureTime", departureTime);
                cmd.Parameters.AddWithValue("@arrivalTime", arrivalTime);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@availableSeats", availableSeats);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления расписания: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool UpdateTicket(int ticketId, string ticketNumber, int scheduleId, string passengerName,
                                        int seatNumber, decimal price, DateTime saleDate, bool isReturned)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"UPDATE Tickets SET 
                                  ticket_number = @ticketNumber, 
                                  schedule_id = @scheduleId, 
                                  passenger_name = @passengerName, 
                                  seat_number = @seatNumber, 
                                  price = @price, 
                                  sale_date = @saleDate, 
                                  is_returned = @isReturned 
                                  WHERE ticket_id = @ticketId";
                cmd.Parameters.AddWithValue("@ticketId", ticketId);
                cmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                cmd.Parameters.AddWithValue("@scheduleId", scheduleId);
                cmd.Parameters.AddWithValue("@passengerName", passengerName);
                cmd.Parameters.AddWithValue("@seatNumber", seatNumber);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@saleDate", saleDate);
                cmd.Parameters.AddWithValue("@isReturned", isReturned ? 1 : 0);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления билета: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // ==================== DELETE ====================

        public static bool DeleteUser(int userId)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Users WHERE user_id = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления пользователя: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool DeleteRoute(int routeId)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Routes WHERE route_id = @routeId", conn);
                cmd.Parameters.AddWithValue("@routeId", routeId);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления маршрута: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool DeleteBus(int busId)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Buses WHERE bus_id = @busId", conn);
                cmd.Parameters.AddWithValue("@busId", busId);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления автобуса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool DeleteSchedule(int scheduleId)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Schedule WHERE schedule_id = @scheduleId", conn);
                cmd.Parameters.AddWithValue("@scheduleId", scheduleId);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления рейса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool DeleteTicket(int ticketId)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Tickets WHERE ticket_id = @ticketId", conn);
                cmd.Parameters.AddWithValue("@ticketId", ticketId);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления билета: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // ==================== ДОПОЛНИТЕЛЬНЫЕ МЕТОДЫ ====================

        public static void CreateIndexes()
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_routes_number ON Routes(route_number)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_routes_active ON Routes(is_active)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_buses_plate ON Buses(plate_number)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_users_username ON Users(username)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_users_role ON Users(role)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_schedule_dates ON Schedule(departure_time, arrival_time)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_schedule_status ON Schedule(status)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_tickets_number ON Tickets(ticket_number)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_tickets_schedule ON Tickets(schedule_id)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_tickets_sale_date ON Tickets(sale_date)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_tickets_returned ON Tickets(is_returned)";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания индексов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static void InsertTestData()
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                // Используем INSERT OR IGNORE, чтобы избежать дубликатов
                // Тестовые маршруты
                cmd.CommandText = @"
                    INSERT OR IGNORE INTO Routes (route_number, departure_city, arrival_city, distance, duration_minutes, is_active) VALUES
                    ('101', 'Москва', 'Санкт-Петербург', '700 км', 630, 1),
                    ('202', 'Казань', 'Уфа', '450 км', 360, 1),
                    ('305', 'Новосибирск', 'Томск', '250 км', 180, 0),
                    ('102', 'Михайловск', 'Ставрополь', '35 км', 45, 1),
                    ('98', 'Ипатово', 'Ставрополь', '120 км', 120, 1),
                    ('228', 'Ипатово', 'Москва', '1400 км', 1440, 1),
                    ('314', 'Санкт-Петербург', 'Питер', '5 км', 10, 1)";
                cmd.ExecuteNonQuery();

                // Тестовые автобусы
                cmd.CommandText = @"
                    INSERT OR IGNORE INTO Buses (plate_number, brand, model, capacity, year, is_active) VALUES
                    ('А123ВС77', 'Mercedes', 'Tourismo', 50, 2022, 1),
                    ('В456ОР78', 'ПАЗ', 'Vector Next', 35, 2021, 1),
                    ('С789ТУ99', 'ЛиАЗ', '5292', 45, 2020, 0),
                    ('E08AD937', 'КАвЗ', 'Неизвестно', 40, 2019, 1),
                    ('D435J543', 'НефаЗ', '5299', 55, 2023, 1)";
                cmd.ExecuteNonQuery();

                // Тестовые пользователи (пароль: 123456)
                cmd.CommandText = @"
                    INSERT OR IGNORE INTO Users (username, password_hash, full_name, role, is_active) VALUES
                    ('admin', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Администратор Системы', 'Администратор', 1),
                    ('dispatcher', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Диспетчер Иванов', 'Диспетчер', 1),
                    ('cashier1', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Кассир Петрова', 'Кассир', 1)";
                cmd.ExecuteNonQuery();

                // Тестовое расписание
                cmd.CommandText = @"
                    INSERT OR IGNORE INTO Schedule (route_id, bus_id, departure_time, arrival_time, price, status, available_seats) VALUES
                    (1, 1, DATETIME('now', '+1 day', '10:00'), DATETIME('now', '+1 day', '20:30'), 2500.00, 'Планируется', 50),
                    (2, 2, DATETIME('now', '+2 days', '14:00'), DATETIME('now', '+2 days', '20:00'), 1800.00, 'Планируется', 35),
                    (3, 3, DATETIME('now', '+3 days', '08:00'), DATETIME('now', '+3 days', '11:00'), 1200.00, 'Планируется', 45)";
                cmd.ExecuteNonQuery();

                // Тестовые билеты
                cmd.CommandText = @"
                    INSERT OR IGNORE INTO Tickets (ticket_number, schedule_id, passenger_name, seat_number, price, sale_date, is_returned) VALUES
                    ('TKT001', 1, 'Иванов Иван Иванович', 15, 2500.00, DATETIME('now', '-1 day'), 0),
                    ('TKT002', 1, 'Петров Петр Петрович', 16, 2500.00, DATETIME('now', '-2 days'), 0),
                    ('TKT003', 2, 'Сидоров Сидор Сидорович', 8, 1800.00, DATETIME('now', '-3 days'), 1)";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления тестовых данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static bool CheckLogin(string username, string passwordHash)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(
                    "SELECT COUNT(*) FROM Users WHERE username = @username AND password_hash = @passwordHash AND is_active = 1",
                    conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@passwordHash", passwordHash);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки логина: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static void InitializeDatabase()
        {
            try
            {
                string dbPath = Path.Combine(Application.StartupPath, "City-Bus.db");

                if (!File.Exists(dbPath))
                {
                    File.WriteAllBytes(dbPath, new byte[0]);

                    using (var conn = new SQLiteConnection(connectionString))
                    {
                        conn.Open();
                    }
                }

                CreateAllTables();

                CreateIndexes();

                // Проверяем, есть ли уже данные, чтобы не добавлять тестовые каждый раз
                if (IsDatabaseEmpty())
                {
                    InsertTestData();
                }

                MessageBox.Show("База данных успешно инициализирована!", "Готово",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        /// <summary>
        /// Проверяет, пустая ли база данных
        /// </summary>
        private static bool IsDatabaseEmpty()
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();

                // Проверяем все таблицы на наличие данных
                string query = @"
                    SELECT (
                        (SELECT COUNT(*) FROM Users) +
                        (SELECT COUNT(*) FROM Routes) +
                        (SELECT COUNT(*) FROM Buses) +
                        (SELECT COUNT(*) FROM Schedule) +
                        (SELECT COUNT(*) FROM Tickets)
                    ) as total_count";

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                int totalCount = Convert.ToInt32(cmd.ExecuteScalar());

                return totalCount == 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки базы данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true; // Если ошибка, считаем что база пустая
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }
}