using Microsoft.Data.Sqlite;
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
    /// <summary>
    /// Основной класс для работы с базой данных системы управления городскими автобусами.
    /// Предоставляет методы для создания таблиц, CRUD-операций и управления данными.
    /// </summary>
    internal class DataBase
    {
        /// <summary>
        /// Строка подключения к базе данных SQLite.
        /// </summary>
        static string connectionString = "Data Source=City-Bus.db";

        // ==================== CREATE ====================

        /// <summary>
        /// Создает все таблицы базы данных в правильном порядке.
        /// </summary>
        /// <remarks>
        /// Метод создает таблицы в следующем порядке:
        /// 1. Users (Пользователи)
        /// 2. Routes (Маршруты)
        /// 3. Buses (Автобусы)
        /// 4. Schedule (Расписание)
        /// 5. Tickets (Билеты)
        /// </remarks>
        public static void CreateAllTables()
        {
            CreateTableUsers();
            CreateTableRoutes();
            CreateTableBuses();
            CreateTableSchedule();
            CreateTableTickets();
        }

        /// <summary>
        /// Создает таблицу пользователей системы.
        /// </summary>
        /// <exception cref="Exception">Выбрасывается при ошибке создания таблицы.</exception>
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

        /// <summary>
        /// Создает таблицу маршрутов автобусов.
        /// </summary>
        /// <exception cref="Exception">Выбрасывается при ошибке создания таблицы.</exception>
        private static void CreateTableRoutes()
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
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

        /// <summary>
        /// Создает таблицу автобусов транспортного предприятия.
        /// </summary>
        /// <exception cref="Exception">Выбрасывается при ошибке создания таблицы.</exception>
        private static void CreateTableBuses()
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
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

        /// <summary>
        /// Создает таблицу расписания рейсов.
        /// </summary>
        /// <remarks>
        /// Таблица содержит связи с таблицами Routes и Buses через внешние ключи.
        /// </remarks>
        /// <exception cref="Exception">Выбрасывается при ошибке создания таблицы.</exception>
        private static void CreateTableSchedule()
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
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

        /// <summary>
        /// Создает таблицу проданных билетов.
        /// </summary>
        /// <remarks>
        /// Таблица содержит уникальное ограничение на комбинацию schedule_id и seat_number,
        /// что предотвращает продажу одного места дважды.
        /// </remarks>
        /// <exception cref="Exception">Выбрасывается при ошибке создания таблицы.</exception>
        private static void CreateTableTickets()
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
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

        /// <summary>
        /// Получает всех пользователей системы.
        /// </summary>
        /// <returns>DataTable со всеми пользователями, отсортированными по ID.</returns>
        /// <remarks>
        /// Возвращает все поля таблицы Users в виде DataTable.
        /// </remarks>
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SqliteConnection conn = new SqliteConnection(connectionString);

            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand("SELECT * FROM Users ORDER BY user_id", conn);

                using (SqliteDataReader reader = cmd.ExecuteReader())
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

        /// <summary>
        /// Получает все маршруты из базы данных.
        /// </summary>
        /// <returns>DataTable со всеми маршрутами, отсортированными по ID.</returns>
        public static DataTable GetAllRoutes()
        {
            DataTable dt = new DataTable();
            SqliteConnection conn = new SqliteConnection(connectionString);

            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand("SELECT * FROM Routes ORDER BY route_id", conn);

                using (SqliteDataReader reader = cmd.ExecuteReader())
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

        /// <summary>
        /// Получает все автобусы транспортного парка.
        /// </summary>
        /// <returns>DataTable со всеми автобусами, отсортированными по ID.</returns>
        public static DataTable GetAllBuses()
        {
            DataTable dt = new DataTable();
            SqliteConnection conn = new SqliteConnection(connectionString);

            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand("SELECT * FROM Buses ORDER BY bus_id", conn);

                using (SqliteDataReader reader = cmd.ExecuteReader())
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

        /// <summary>
        /// Получает полное расписание рейсов с информацией о маршрутах и автобусах.
        /// </summary>
        /// <returns>DataTable с расписанием, отсортированным по времени отправления.</returns>
        /// <remarks>
        /// Выполняет JOIN трех таблиц: Schedule, Routes и Buses.
        /// </remarks>
        public static DataTable GetAllSchedules()
        {
            DataTable dt = new DataTable();
            SqliteConnection conn = new SqliteConnection(connectionString);

            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand(
                    @"SELECT s.*, r.route_number, r.departure_city, r.arrival_city, 
                    b.plate_number, b.brand, b.model 
                    FROM Schedule s 
                    JOIN Routes r ON s.route_id = r.route_id 
                    JOIN Buses b ON s.bus_id = b.bus_id 
                    ORDER BY s.departure_time", conn);

                using (SqliteDataReader reader = cmd.ExecuteReader())
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

        /// <summary>
        /// Получает все проданные билеты с информацией о рейсах.
        /// </summary>
        /// <returns>DataTable с билетами, отсортированными по дате продажи (новые сверху).</returns>
        public static DataTable GetAllTickets()
        {
            DataTable dt = new DataTable();
            SqliteConnection conn = new SqliteConnection(connectionString);

            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand(
                    @"SELECT t.*, s.departure_time, r.route_number, r.departure_city, r.arrival_city 
                    FROM Tickets t 
                    JOIN Schedule s ON t.schedule_id = s.schedule_id 
                    JOIN Routes r ON s.route_id = r.route_id 
                    ORDER BY t.sale_date DESC", conn);

                using (SqliteDataReader reader = cmd.ExecuteReader())
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

        /// <summary>
        /// Получает пользователя по его уникальному идентификатору.
        /// </summary>
        /// <param name="userId">ID пользователя для поиска.</param>
        /// <returns>DataRow с данными пользователя или null, если пользователь не найден.</returns>
        public static DataRow GetUserById(int userId)
        {
            DataTable dt = new DataTable();
            SqliteConnection conn = new SqliteConnection(connectionString);

            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand("SELECT * FROM Users WHERE user_id = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dt.Columns.Add(reader.GetName(i));
                    }

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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска пользователя: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return null;
        }

        // ==================== INSERT ====================

        /// <summary>
        /// Добавляет нового пользователя в систему.
        /// </summary>
        /// <param name="username">Уникальное имя пользователя.</param>
        /// <param name="passwordHash">Хеш пароля пользователя.</param>
        /// <param name="fullName">Полное имя пользователя.</param>
        /// <param name="role">Роль пользователя (Администратор, Диспетчер, Кассир).</param>
        /// <param name="isActive">Статус активности пользователя.</param>
        /// <returns>true если пользователь успешно добавлен, иначе false.</returns>
        public static bool InsertUser(string username, string passwordHash, string fullName, string role, bool isActive)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
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

        /// <summary>
        /// Добавляет новый маршрут в базу данных.
        /// </summary>
        /// <param name="routeNumber">Номер маршрута.</param>
        /// <param name="departureCity">Город отправления.</param>
        /// <param name="arrivalCity">Город прибытия.</param>
        /// <param name="distance">Расстояние маршрута.</param>
        /// <param name="durationMinutes">Продолжительность маршрута в минутах.</param>
        /// <param name="isActive">Статус активности маршрута.</param>
        /// <returns>true если маршрут успешно добавлен, иначе false.</returns>
        public static bool InsertRoute(string routeNumber, string departureCity, string arrivalCity,
                                       string distance, int durationMinutes, bool isActive)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
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

        /// <summary>
        /// Добавляет новый автобус в транспортный парк.
        /// </summary>
        /// <param name="plateNumber">Государственный номер (уникальный).</param>
        /// <param name="brand">Марка автобуса.</param>
        /// <param name="model">Модель автобуса.</param>
        /// <param name="capacity">Вместимость пассажиров.</param>
        /// <param name="year">Год выпуска.</param>
        /// <param name="isActive">Статус активности автобуса.</param>
        /// <returns>true если автобус успешно добавлен, иначе false.</returns>
        public static bool InsertBus(string plateNumber, string brand, string model,
                                     int capacity, int year, bool isActive)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
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

        /// <summary>
        /// Добавляет новый рейс в расписание.
        /// </summary>
        /// <param name="routeId">ID маршрута.</param>
        /// <param name="busId">ID автобуса.</param>
        /// <param name="departureTime">Время отправления.</param>
        /// <param name="arrivalTime">Время прибытия.</param>
        /// <param name="price">Цена билета.</param>
        /// <param name="status">Статус рейса (Планируется, Выполняется, Завершен, Отменен).</param>
        /// <param name="availableSeats">Количество доступных мест.</param>
        /// <returns>true если рейс успешно добавлен, иначе false.</returns>
        public static bool InsertSchedule(int routeId, int busId, DateTime departureTime,
                                          DateTime arrivalTime, decimal price, string status, int availableSeats)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand();
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

        /// <summary>
        /// Добавляет новый проданный билет.
        /// </summary>
        /// <param name="ticketNumber">Уникальный номер билета.</param>
        /// <param name="scheduleId">ID рейса.</param>
        /// <param name="passengerName">ФИО пассажира.</param>
        /// <param name="seatNumber">Номер места.</param>
        /// <param name="price">Цена билета.</param>
        /// <param name="saleDate">Дата и время продажи.</param>
        /// <param name="isReturned">Статус возврата билета.</param>
        /// <returns>true если билет успешно добавлен, иначе false.</returns>
        public static bool InsertTicket(string ticketNumber, int scheduleId, string passengerName,
                                        int seatNumber, decimal price, DateTime saleDate, bool isReturned)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand();
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

        /// <summary>
        /// Обновляет данные пользователя.
        /// </summary>
        /// <param name="userId">ID пользователя для обновления.</param>
        /// <param name="username">Новое имя пользователя.</param>
        /// <param name="passwordHash">Новый хеш пароля.</param>
        /// <param name="fullName">Новое полное имя.</param>
        /// <param name="role">Новая роль.</param>
        /// <param name="isActive">Новый статус активности.</param>
        /// <returns>true если обновление успешно, иначе false.</returns>
        public static bool UpdateUser(int userId, string username, string passwordHash,
                                     string fullName, string role, bool isActive)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
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

        /// <summary>
        /// Обновляет данные маршрута.
        /// </summary>
        /// <param name="routeId">ID маршрута для обновления.</param>
        /// <param name="routeNumber">Новый номер маршрута.</param>
        /// <param name="departureCity">Новый город отправления.</param>
        /// <param name="arrivalCity">Новый город прибытия.</param>
        /// <param name="distance">Новое расстояние.</param>
        /// <param name="durationMinutes">Новая продолжительность в минутах.</param>
        /// <param name="isActive">Новый статус активности.</param>
        /// <returns>true если обновление успешно, иначе false.</returns>
        public static bool UpdateRoute(int routeId, string routeNumber, string departureCity,
                                       string arrivalCity, string distance, int durationMinutes, bool isActive)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
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

        /// <summary>
        /// Обновляет данные автобуса.
        /// </summary>
        /// <param name="busId">ID автобуса для обновления.</param>
        /// <param name="plateNumber">Новый госномер.</param>
        /// <param name="brand">Новая марка.</param>
        /// <param name="model">Новая модель.</param>
        /// <param name="capacity">Новая вместимость.</param>
        /// <param name="year">Новый год выпуска.</param>
        /// <param name="isActive">Новый статус активности.</param>
        /// <returns>true если обновление успешно, иначе false.</returns>
        public static bool UpdateBus(int busId, string plateNumber, string brand, string model,
                                     int capacity, int year, bool isActive)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
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

        /// <summary>
        /// Обновляет данные рейса в расписании.
        /// </summary>
        /// <param name="scheduleId">ID рейса для обновления.</param>
        /// <param name="routeId">Новый ID маршрута.</param>
        /// <param name="busId">Новый ID автобуса.</param>
        /// <param name="departureTime">Новое время отправления.</param>
        /// <param name="arrivalTime">Новое время прибытия.</param>
        /// <param name="price">Новая цена.</param>
        /// <param name="status">Новый статус.</param>
        /// <param name="availableSeats">Новое количество доступных мест.</param>
        /// <returns>true если обновление успешно, иначе false.</returns>
        public static bool UpdateSchedule(int scheduleId, int routeId, int busId, DateTime departureTime,
                                          DateTime arrivalTime, decimal price, string status, int availableSeats)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand();
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

        /// <summary>
        /// Обновляет данные билета.
        /// </summary>
        /// <param name="ticketId">ID билета для обновления.</param>
        /// <param name="ticketNumber">Новый номер билета.</param>
        /// <param name="scheduleId">Новый ID рейса.</param>
        /// <param name="passengerName">Новое ФИО пассажира.</param>
        /// <param name="seatNumber">Новый номер места.</param>
        /// <param name="price">Новая цена.</param>
        /// <param name="saleDate">Новая дата продажи.</param>
        /// <param name="isReturned">Новый статус возврата.</param>
        /// <returns>true если обновление успешно, иначе false.</returns>
        public static bool UpdateTicket(int ticketId, string ticketNumber, int scheduleId, string passengerName,
                                        int seatNumber, decimal price, DateTime saleDate, bool isReturned)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand();
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

        /// <summary>
        /// Удаляет пользователя по ID.
        /// </summary>
        /// <param name="userId">ID пользователя для удаления.</param>
        /// <returns>true если удаление успешно, иначе false.</returns>
        public static bool DeleteUser(int userId)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand("DELETE FROM Users WHERE user_id = @userId", conn);
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

        /// <summary>
        /// Удаляет маршрут по ID.
        /// </summary>
        /// <param name="routeId">ID маршрута для удаления.</param>
        /// <returns>true если удаление успешно, иначе false.</returns>
        public static bool DeleteRoute(int routeId)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand("DELETE FROM Routes WHERE route_id = @routeId", conn);
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

        /// <summary>
        /// Удаляет автобус по ID.
        /// </summary>
        /// <param name="busId">ID автобуса для удаления.</param>
        /// <returns>true если удаление успешно, иначе false.</returns>
        public static bool DeleteBus(int busId)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand("DELETE FROM Buses WHERE bus_id = @busId", conn);
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

        /// <summary>
        /// Удаляет рейс из расписания по ID.
        /// </summary>
        /// <param name="scheduleId">ID рейса для удаления.</param>
        /// <returns>true если удаление успешно, иначе false.</returns>
        public static bool DeleteSchedule(int scheduleId)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand("DELETE FROM Schedule WHERE schedule_id = @scheduleId", conn);
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

        /// <summary>
        /// Удаляет билет по ID.
        /// </summary>
        /// <param name="ticketId">ID билета для удаления.</param>
        /// <returns>true если удаление успешно, иначе false.</returns>
        public static bool DeleteTicket(int ticketId)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand("DELETE FROM Tickets WHERE ticket_id = @ticketId", conn);
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

        /// <summary>
        /// Создает индексы для оптимизации работы базы данных.
        /// </summary>
        /// <remarks>
        /// Создает индексы для часто используемых полей всех таблиц.
        /// </remarks>
        public static void CreateIndexes()
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;

                // Индексы для таблицы Routes
                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_routes_number ON Routes(route_number)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_routes_active ON Routes(is_active)";
                cmd.ExecuteNonQuery();

                // Индексы для таблицы Buses
                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_buses_plate ON Buses(plate_number)";
                cmd.ExecuteNonQuery();

                // Индексы для таблицы Users
                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_users_username ON Users(username)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_users_role ON Users(role)";
                cmd.ExecuteNonQuery();

                // Индексы для таблицы Schedule
                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_schedule_dates ON Schedule(departure_time, arrival_time)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_schedule_status ON Schedule(status)";
                cmd.ExecuteNonQuery();

                // Индексы для таблицы Tickets
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

        /// <summary>
        /// Вставляет тестовые данные в базу данных.
        /// </summary>
        /// <remarks>
        /// Добавляет тестовые маршруты, автобусы, пользователей, расписание и билеты.
        /// Используется для первоначального заполнения базы данных или тестирования.
        /// </remarks>
        public static void InsertTestData()
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;

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

        /// <summary>
        /// Проверяет корректность логина и пароля пользователя.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="passwordHash">Хеш пароля для проверки.</param>
        /// <returns>true если логин и пароль корректны и пользователь активен, иначе false.</returns>
        /// <remarks>
        /// Метод проверяет наличие активного пользователя с указанными именем и хешем пароля.
        /// </remarks>
        public static bool CheckLogin(string username, string passwordHash)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
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

        /// <summary>
        /// Создает SHA256 хеш пароля.
        /// </summary>
        /// <param name="password">Пароль для хеширования.</param>
        /// <returns>Хеш пароля в виде шестнадцатеричной строки.</returns>
        /// <remarks>
        /// Использует алгоритм SHA256 для создания безопасного хеша пароля.
        /// Все хеши приводятся к нижнему регистру для единообразия.
        /// </remarks>
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

        /// <summary>
        /// Инициализирует базу данных: создает файл, таблицы, индексы и тестовые данные.
        /// </summary>
        /// <remarks>
        /// Полный процесс инициализации базы данных при первом запуске приложения.
        /// </remarks>
        public static void InitializeDatabase()
        {
            try
            {
                string dbPath = Path.Combine(Application.StartupPath, "City-Bus.db");

                if (!File.Exists(dbPath))
                {
                    File.WriteAllBytes(dbPath, new byte[0]);

                    using (var conn = new SqliteConnection(connectionString))
                    {
                        conn.Open();
                    }
                }

                CreateAllTables();

                CreateIndexes();

                InsertTestData();

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
        /// Создает и возвращает новое соединение с базой данных.
        /// </summary>
        /// <returns>Новое соединение SqliteConnection.</returns>
        /// <remarks>
        /// Используется для ручного управления соединениями в особых случаях.
        /// </remarks>
        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }
    }
}