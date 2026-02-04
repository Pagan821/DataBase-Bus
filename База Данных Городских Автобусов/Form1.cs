using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace База_Данных_Городских_Автобусов
{
    public partial class MainDatabaseForm : Form
    {
        private enum TableMode { Routes, Buses, Schedule, Tickets, Users }
        private TableMode currentMode = TableMode.Routes;

        private DataTable routesData = new DataTable();
        private DataTable busesData = new DataTable();
        private DataTable scheduleData = new DataTable();
        private DataTable ticketsData = new DataTable();
        private DataTable usersData = new DataTable();

        public MainDatabaseForm()
        {
            InitializeSQLitePCL();

            InitializeComponent();

            // Инициализируем базу данных
            InitializeDatabase();

            InitializeDataTables();

            LoadInitialData();

            SetupEventHandlers();

            // Добавляем обработчики ошибок DataGridView
            AddDataGridViewErrorHandlers();
        }

        private void InitializeSQLitePCL()
        {
            try
            {
                using (var conn = new System.Data.SQLite.SQLiteConnection("Data Source=:memory:"))
                {
                    conn.Open();
                    using (var cmd = new System.Data.SQLite.SQLiteCommand("SELECT 1", conn))
                    {
                        var result = cmd.ExecuteScalar();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации SQLite: {ex.Message}\n" +
                               $"Убедитесь, что установлен пакет System.Data.SQLite",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDatabase()
        {
            // Инициализируем базу данных при запуске программы
            DataBase.InitializeDatabase();
        }

        private void InitializeDataTables()
        {
            // Маршруты
            routesData.Columns.Add("ID", typeof(int));
            routesData.Columns.Add("Номер", typeof(string));
            routesData.Columns.Add("Отправление", typeof(string));
            routesData.Columns.Add("Прибытие", typeof(string));
            routesData.Columns.Add("Расстояние", typeof(string));
            routesData.Columns.Add("Активен", typeof(bool));

            // Автобусы
            busesData.Columns.Add("ID", typeof(int));
            busesData.Columns.Add("Гос. номер", typeof(string));
            busesData.Columns.Add("Марка", typeof(string));
            busesData.Columns.Add("Модель", typeof(string));
            busesData.Columns.Add("Вместимость", typeof(int));
            busesData.Columns.Add("Год", typeof(int));
            busesData.Columns.Add("Активен", typeof(bool));

            // Расписание
            scheduleData.Columns.Add("ID", typeof(int));
            scheduleData.Columns.Add("Маршрут", typeof(string));
            scheduleData.Columns.Add("Автобус", typeof(string));
            scheduleData.Columns.Add("Отправление", typeof(DateTime));
            scheduleData.Columns.Add("Прибытие", typeof(DateTime));
            scheduleData.Columns.Add("Цена", typeof(decimal));
            scheduleData.Columns.Add("Статус", typeof(string));

            // Билеты
            ticketsData.Columns.Add("ID", typeof(int));
            ticketsData.Columns.Add("Номер", typeof(string));
            ticketsData.Columns.Add("Рейс", typeof(string));
            ticketsData.Columns.Add("Пассажир", typeof(string));
            ticketsData.Columns.Add("Место", typeof(int));
            ticketsData.Columns.Add("Цена", typeof(decimal));
            ticketsData.Columns.Add("Дата продажи", typeof(DateTime));
            ticketsData.Columns.Add("Возвращен", typeof(bool));

            // Пользователи
            usersData.Columns.Add("ID", typeof(int));
            usersData.Columns.Add("Логин", typeof(string));
            usersData.Columns.Add("ФИО", typeof(string));
            usersData.Columns.Add("Роль", typeof(string));
            usersData.Columns.Add("Активен", typeof(bool));
        }

        private void LoadInitialData()
        {
            LoadDataFromDatabase();
            ShowRoutes();

            // Изначально отключаем кнопки редактирования и удаления
            btnRouteEdit.Enabled = false;
            btnRouteDelete.Enabled = false;
            btnBusEdit.Enabled = false;
            btnBusDelete.Enabled = false;
            btnScheduleEdit.Enabled = false;
            btnScheduleDelete.Enabled = false;
            btnTicketEdit.Enabled = false;
            btnTicketDelete.Enabled = false;
            btnUserEdit.Enabled = false;
            btnUserDelete.Enabled = false;
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                // Загружаем данные из базы данных
                routesData.Clear();
                busesData.Clear();
                scheduleData.Clear();
                ticketsData.Clear();
                usersData.Clear();

                // Загружаем маршруты
                DataTable routesDb = DataBase.GetAllRoutes();
                foreach (DataRow row in routesDb.Rows)
                {
                    DataRow newRow = routesData.NewRow();

                    // Безопасное заполнение полей
                    newRow["ID"] = SafeConvertToInt(row["route_id"]);
                    newRow["Номер"] = SafeConvertToString(row["route_number"]);
                    newRow["Отправление"] = SafeConvertToString(row["departure_city"]);
                    newRow["Прибытие"] = SafeConvertToString(row["arrival_city"]);
                    newRow["Расстояние"] = SafeConvertToString(row["distance"]);
                    newRow["Активен"] = SafeConvertToBool(row["is_active"]);

                    routesData.Rows.Add(newRow);
                }

                // Загружаем автобусы
                DataTable busesDb = DataBase.GetAllBuses();
                foreach (DataRow row in busesDb.Rows)
                {
                    DataRow newRow = busesData.NewRow();

                    newRow["ID"] = SafeConvertToInt(row["bus_id"]);
                    newRow["Гос. номер"] = SafeConvertToString(row["plate_number"]);
                    newRow["Марка"] = SafeConvertToString(row["brand"]);
                    newRow["Модель"] = SafeConvertToString(row["model"]);
                    newRow["Вместимость"] = SafeConvertToInt(row["capacity"]);
                    newRow["Год"] = SafeConvertToInt(row["year"]);
                    newRow["Активен"] = SafeConvertToBool(row["is_active"]);

                    busesData.Rows.Add(newRow);
                }

                // Загружаем расписание
                DataTable scheduleDb = DataBase.GetAllSchedules();
                foreach (DataRow row in scheduleDb.Rows)
                {
                    DataRow newRow = scheduleData.NewRow();

                    newRow["ID"] = SafeConvertToInt(row["schedule_id"]);

                    string routeInfo = $"{SafeConvertToString(row["route_number"])} {SafeConvertToString(row["departure_city"])}-{SafeConvertToString(row["arrival_city"])}";
                    newRow["Маршрут"] = routeInfo;

                    string busInfo = $"{SafeConvertToString(row["plate_number"])} ({SafeConvertToString(row["brand"])} {SafeConvertToString(row["model"])})";
                    newRow["Автобус"] = busInfo;

                    newRow["Отправление"] = SafeConvertToDateTime(row["departure_time"]);
                    newRow["Прибытие"] = SafeConvertToDateTime(row["arrival_time"]);
                    newRow["Цена"] = SafeConvertToDecimal(row["price"]);
                    newRow["Статус"] = SafeConvertToString(row["status"]);

                    scheduleData.Rows.Add(newRow);
                }

                // Загружаем билеты
                DataTable ticketsDb = DataBase.GetAllTickets();
                foreach (DataRow row in ticketsDb.Rows)
                {
                    DataRow newRow = ticketsData.NewRow();

                    newRow["ID"] = SafeConvertToInt(row["ticket_id"]);
                    newRow["Номер"] = SafeConvertToString(row["ticket_number"]);

                    string routeInfo = $"{SafeConvertToString(row["route_number"])} {SafeConvertToString(row["departure_city"])}-{SafeConvertToString(row["arrival_city"])}";
                    newRow["Рейс"] = routeInfo;

                    newRow["Пассажир"] = SafeConvertToString(row["passenger_name"]);
                    newRow["Место"] = SafeConvertToInt(row["seat_number"]);
                    newRow["Цена"] = SafeConvertToDecimal(row["price"]);
                    newRow["Дата продажи"] = SafeConvertToDateTime(row["sale_date"]);
                    newRow["Возвращен"] = SafeConvertToBool(row["is_returned"]);

                    ticketsData.Rows.Add(newRow);
                }

                // Загружаем пользователей
                DataTable usersDb = DataBase.GetAllUsers();
                foreach (DataRow row in usersDb.Rows)
                {
                    DataRow newRow = usersData.NewRow();

                    newRow["ID"] = SafeConvertToInt(row["user_id"]);
                    newRow["Логин"] = SafeConvertToString(row["username"]);
                    newRow["ФИО"] = SafeConvertToString(row["full_name"]);
                    newRow["Роль"] = SafeConvertToString(row["role"]);
                    newRow["Активен"] = SafeConvertToBool(row["is_active"]);

                    usersData.Rows.Add(newRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\n\n{ex.StackTrace}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ДЛЯ КОНВЕРТАЦИИ ====================

        private int SafeConvertToInt(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;

            if (value is int)
                return (int)value;

            int result;
            if (int.TryParse(value.ToString(), out result))
                return result;

            return 0;
        }

        private string SafeConvertToString(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            return value.ToString();
        }

        private bool SafeConvertToBool(object value)
        {
            if (value == null || value == DBNull.Value)
                return false;

            if (value is bool)
                return (bool)value;

            if (value is int)
                return (int)value == 1;

            if (value is string)
            {
                string strValue = value.ToString().ToLower();
                return strValue == "1" || strValue == "true" || strValue == "да" || strValue == "✓";
            }

            return false;
        }

        private DateTime SafeConvertToDateTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return DateTime.MinValue;

            if (value is DateTime)
                return (DateTime)value;

            DateTime result;
            if (DateTime.TryParse(value.ToString(), out result))
                return result;

            return DateTime.MinValue;
        }

        private decimal SafeConvertToDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0m;

            if (value is decimal)
                return (decimal)value;

            decimal result;
            if (decimal.TryParse(value.ToString(), out result))
                return result;

            return 0m;
        }

        // ==================== ОБРАБОТЧИКИ ОШИБОК DATAGRIDVIEW ====================

        private void AddDataGridViewErrorHandlers()
        {
            dataGridViewRoutes.DataError += DataGridView_DataError;
            dataGridViewBuses.DataError += DataGridView_DataError;
            dataGridViewSchedule.DataError += DataGridView_DataError;
            dataGridViewTickets.DataError += DataGridView_DataError;
            dataGridViewUsers.DataError += DataGridView_DataError;
        }

        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void SetupEventHandlers()
        {
            // Обработка переключения вкладок
            tabControlMain.SelectedIndexChanged += TabControlMain_SelectedIndexChanged;

            // Кнопка Назад
            btnBack.Click += BtnBack_Click;

            // Установка обработчиков для каждой вкладки
            SetupRouteHandlers();
            SetupBusHandlers();
            SetupScheduleHandlers();
            SetupTicketHandlers();
            SetupUserHandlers();

            // Обработчики выбора строк в таблицах
            dataGridViewRoutes.SelectionChanged += DataGridViewRoutes_SelectionChanged;
            dataGridViewBuses.SelectionChanged += DataGridViewBuses_SelectionChanged;
            dataGridViewSchedule.SelectionChanged += DataGridViewSchedule_SelectionChanged;
            dataGridViewTickets.SelectionChanged += DataGridViewTickets_SelectionChanged;
            dataGridViewUsers.SelectionChanged += DataGridViewUsers_SelectionChanged;

            // Настройка режима выбора строк
            dataGridViewRoutes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBuses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSchedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Добавляем форматирование для колонок
            dataGridViewRoutes.CellFormatting += DataGridViewRoutes_CellFormatting;
            dataGridViewTickets.CellFormatting += DataGridViewTickets_CellFormatting;
            dataGridViewUsers.CellFormatting += DataGridViewUsers_CellFormatting;
        }

        private void DataGridViewRoutes_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dataGridViewRoutes.SelectedRows.Count > 0;
            btnRouteEdit.Enabled = hasSelection;
            btnRouteDelete.Enabled = hasSelection;
        }

        private void DataGridViewBuses_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dataGridViewBuses.SelectedRows.Count > 0;
            btnBusEdit.Enabled = hasSelection;
            btnBusDelete.Enabled = hasSelection;
        }

        private void DataGridViewSchedule_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dataGridViewSchedule.SelectedRows.Count > 0;
            btnScheduleEdit.Enabled = hasSelection;
            btnScheduleDelete.Enabled = hasSelection;
        }

        private void DataGridViewTickets_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dataGridViewTickets.SelectedRows.Count > 0;
            btnTicketEdit.Enabled = hasSelection;
            btnTicketDelete.Enabled = hasSelection;
        }

        private void DataGridViewUsers_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dataGridViewUsers.SelectedRows.Count > 0;
            btnUserEdit.Enabled = hasSelection;
            btnUserDelete.Enabled = hasSelection;
        }

        // ==================== ОБЩИЕ МЕТОДЫ ====================

        private void UpdateStatusLabel(int count)
        {
            string tableName;

            switch (currentMode)
            {
                case TableMode.Routes:
                    tableName = "маршрутов";
                    break;
                case TableMode.Buses:
                    tableName = "автобусов";
                    break;
                case TableMode.Schedule:
                    tableName = "рейсов";
                    break;
                case TableMode.Tickets:
                    tableName = "билетов";
                    break;
                case TableMode.Users:
                    tableName = "пользователей";
                    break;
                default:
                    tableName = "записей";
                    break;
            }

            labelStatus.Text = $"Загружено {tableName}: {count}";
        }

        private DataGridView GetCurrentDataGridView()
        {
            switch (currentMode)
            {
                case TableMode.Routes:
                    return dataGridViewRoutes;
                case TableMode.Buses:
                    return dataGridViewBuses;
                case TableMode.Schedule:
                    return dataGridViewSchedule;
                case TableMode.Tickets:
                    return dataGridViewTickets;
                case TableMode.Users:
                    return dataGridViewUsers;
                default:
                    return dataGridViewRoutes;
            }
        }

        private TextBox GetCurrentSearchBox()
        {
            switch (currentMode)
            {
                case TableMode.Routes:
                    return txtRouteSearch;
                case TableMode.Buses:
                    return txtBusSearch;
                case TableMode.Schedule:
                    return txtScheduleSearch;
                case TableMode.Tickets:
                    return txtTicketSearch;
                case TableMode.Users:
                    return txtUserSearch;
                default:
                    return txtRouteSearch;
            }
        }

        private void PerformSearch(string searchText, DataGridView dataGridView)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                ShowAllRows(dataGridView);
                UpdateStatusLabel(dataGridView.Rows.Count);
                return;
            }

            searchText = searchText.ToLower();
            int visibleCount = 0;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                bool isVisible = false;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null &&
                        cell.Value.ToString().ToLower().Contains(searchText))
                    {
                        isVisible = true;
                        break;
                    }
                }

                row.Visible = isVisible;
                if (isVisible) visibleCount++;
            }

            UpdateStatusLabel(visibleCount);

            if (visibleCount == 0)
            {
                MessageBox.Show("Записи не найдены", "Результат поиска",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ShowAllRows(DataGridView dataGridView)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                row.Visible = true;
            }
        }

        // ==================== ОБРАБОТЧИКИ ВКЛАДОК ====================

        private void TabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlMain.SelectedIndex)
            {
                case 0: // Маршруты
                    currentMode = TableMode.Routes;
                    ShowRoutes();
                    break;
                case 1: // Автобусы
                    currentMode = TableMode.Buses;
                    ShowBuses();
                    break;
                case 2: // Расписание
                    currentMode = TableMode.Schedule;
                    ShowSchedule();
                    break;
                case 3: // Билеты
                    currentMode = TableMode.Tickets;
                    ShowTickets();
                    break;
                case 4: // Пользователи
                    currentMode = TableMode.Users;
                    ShowUsers();
                    break;
            }
        }

        // ==================== МАРШРУТЫ ====================

        private void SetupRouteHandlers()
        {
            btnRouteAdd.Click += BtnRouteAdd_Click;
            btnRouteEdit.Click += BtnRouteEdit_Click;
            btnRouteDelete.Click += BtnRouteDelete_Click;
            btnRouteRefresh.Click += BtnRouteRefresh_Click;
            btnRouteSearch.Click += BtnRouteSearch_Click;
            
            // Поиск при нажатии Enter
            txtRouteSearch.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    PerformSearch(txtRouteSearch.Text, dataGridViewRoutes);
                    e.Handled = true;
                }
            };
        }

        private void ShowRoutes()
        {
            try
            {
                dataGridViewRoutes.DataSource = null;
                dataGridViewRoutes.DataSource = routesData;

                // Настраиваем форматирование колонок
                if (dataGridViewRoutes.Columns.Contains("Активен"))
                {
                    dataGridViewRoutes.Columns["Активен"].DefaultCellStyle.NullValue = false;
                }

                UpdateStatusLabel(routesData.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отображения маршрутов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRouteAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new RouteEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Проверяем, существует ли уже такой маршрут
                    bool exists = DataBase.CheckRouteExists(
                        form.RouteNumber,
                        form.DepartureCity,
                        form.ArrivalCity
                    );

                    if (exists)
                    {
                        MessageBox.Show("Такой маршрут уже существует!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Добавляем в базу данных
                    int durationMinutes = 0;
                    if (!string.IsNullOrEmpty(form.DurationMinutes))
                    {
                        int.TryParse(form.DurationMinutes, out durationMinutes);
                    }

                    bool success = DataBase.InsertRoute(
                        form.RouteNumber,
                        form.DepartureCity,
                        form.ArrivalCity,
                        form.Distance,
                        durationMinutes,
                        form.IsActive
                    );

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowRoutes();
                        MessageBox.Show("Маршрут добавлен", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении маршрута", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRouteEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewRoutes.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите маршрут для редактирования", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dataGridViewRoutes.SelectedRows[0];
                int routeId = SafeConvertToInt(selectedRow.Cells["ID"].Value);

                var form = new RouteEditForm();
                form.LoadRouteData(
                    routeId,
                    SafeConvertToString(selectedRow.Cells["Номер"].Value),
                    SafeConvertToString(selectedRow.Cells["Отправление"].Value),
                    SafeConvertToString(selectedRow.Cells["Прибытие"].Value),
                    SafeConvertToString(selectedRow.Cells["Расстояние"].Value),
                    SafeConvertToBool(selectedRow.Cells["Активен"].Value));

                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Проверяем, существует ли уже такой маршрут (кроме текущего)
                    bool exists = DataBase.CheckRouteExists(
                        form.RouteNumber,
                        form.DepartureCity,
                        form.ArrivalCity,
                        routeId
                    );

                    if (exists)
                    {
                        MessageBox.Show("Такой маршрут уже существует!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Обновляем в базе данных
                    int durationMinutes = 0;
                    if (!string.IsNullOrEmpty(form.DurationMinutes))
                    {
                        int.TryParse(form.DurationMinutes, out durationMinutes);
                    }

                    bool success = DataBase.UpdateRoute(
                        routeId,
                        form.RouteNumber,
                        form.DepartureCity,
                        form.ArrivalCity,
                        form.Distance,
                        durationMinutes,
                        form.IsActive
                    );

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowRoutes();
                        MessageBox.Show("Маршрут изменен", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении маршрута", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRouteDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewRoutes.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите маршрут для удаления", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show("Удалить выбранный маршрут?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var selectedRow = dataGridViewRoutes.SelectedRows[0];
                    int routeId = SafeConvertToInt(selectedRow.Cells["ID"].Value);

                    // Проверяем, есть ли связанные записи
                    bool hasRelatedRecords = DataBase.CheckRouteHasRelatedRecords(routeId);
                    
                    if (hasRelatedRecords)
                    {
                        var confirm = MessageBox.Show(
                            "У этого маршрута есть связанные рейсы и/или билеты.\n" +
                            "При удалении маршрута все связанные записи также будут удалены.\n\n" +
                            "Продолжить удаление?",
                            "Подтверждение удаления",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (confirm != DialogResult.Yes)
                            return;
                    }

                    // Удаляем из базы данных
                    bool success = DataBase.DeleteRoute(routeId);

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowRoutes();
                        MessageBox.Show("Маршрут удален", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении маршрута", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRouteRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                txtRouteSearch.Text = "";
                // Перезагружаем данные из базы
                LoadDataFromDatabase();
                ShowRoutes();
                MessageBox.Show("Данные обновлены", "Информация");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRouteSearch_Click(object sender, EventArgs e)
        {
            PerformSearch(txtRouteSearch.Text, dataGridViewRoutes);
        }

        // ==================== АВТОБУСЫ ====================

        private void SetupBusHandlers()
        {
            btnBusAdd.Click += BtnBusAdd_Click;
            btnBusEdit.Click += BtnBusEdit_Click;
            btnBusDelete.Click += BtnBusDelete_Click;
            btnBusRefresh.Click += BtnBusRefresh_Click;
            btnBusSearch.Click += BtnBusSearch_Click;
            
            // Поиск при нажатии Enter
            txtBusSearch.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    PerformSearch(txtBusSearch.Text, dataGridViewBuses);
                    e.Handled = true;
                }
            };
        }

        private void ShowBuses()
        {
            try
            {
                dataGridViewBuses.DataSource = null;
                dataGridViewBuses.DataSource = busesData;
                UpdateStatusLabel(busesData.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отображения автобусов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBusAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new BusEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Проверяем, существует ли уже такой автобус
                    bool exists = DataBase.CheckBusExists(form.PlateNumber);
                    
                    if (exists)
                    {
                        MessageBox.Show("Автобус с таким гос. номером уже существует!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Добавляем в базу данных
                    bool success = DataBase.InsertBus(
                        form.PlateNumber,
                        form.Brand,
                        form.Model,
                        form.Capacity,
                        form.Year,
                        form.IsActive
                    );

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowBuses();
                        MessageBox.Show("Автобус добавлен", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении автобуса", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBusEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewBuses.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите автобус для редактирования", "Внимание");
                    return;
                }

                var selectedRow = dataGridViewBuses.SelectedRows[0];
                int busId = SafeConvertToInt(selectedRow.Cells["ID"].Value);

                var form = new BusEditForm();
                form.LoadBusData(
                    busId,
                    SafeConvertToString(selectedRow.Cells["Гос. номер"].Value),
                    SafeConvertToString(selectedRow.Cells["Марка"].Value),
                    SafeConvertToString(selectedRow.Cells["Модель"].Value),
                    SafeConvertToInt(selectedRow.Cells["Вместимость"].Value),
                    SafeConvertToInt(selectedRow.Cells["Год"].Value),
                    SafeConvertToBool(selectedRow.Cells["Активен"].Value));

                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Проверяем, существует ли уже такой автобус (кроме текущего)
                    bool exists = DataBase.CheckBusExists(form.PlateNumber, busId);
                    
                    if (exists)
                    {
                        MessageBox.Show("Автобус с таким гос. номером уже существует!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Обновляем в базе данных
                    bool success = DataBase.UpdateBus(
                        busId,
                        form.PlateNumber,
                        form.Brand,
                        form.Model,
                        form.Capacity,
                        form.Year,
                        form.IsActive
                    );

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowBuses();
                        MessageBox.Show("Автобус изменен", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении автобуса", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBusDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewBuses.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите автобус для удаления", "Внимание");
                    return;
                }

                var result = MessageBox.Show("Удалить выбранный автобус?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var selectedRow = dataGridViewBuses.SelectedRows[0];
                    int busId = SafeConvertToInt(selectedRow.Cells["ID"].Value);

                    // Проверяем, есть ли связанные записи
                    bool hasRelatedRecords = DataBase.CheckBusHasRelatedRecords(busId);
                    
                    if (hasRelatedRecords)
                    {
                        var confirm = MessageBox.Show(
                            "У этого автобуса есть связанные рейсы и/или билеты.\n" +
                            "При удалении автобуса все связанные записи также будут удалены.\n\n" +
                            "Продолжить удаление?",
                            "Подтверждение удаления",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (confirm != DialogResult.Yes)
                            return;
                    }

                    // Удаляем из базы данных
                    bool success = DataBase.DeleteBus(busId);

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowBuses();
                        MessageBox.Show("Автобус удален", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении автобуса", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBusRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                txtBusSearch.Text = "";
                ShowAllRows(dataGridViewBuses);
                UpdateStatusLabel(busesData.Rows.Count);
                MessageBox.Show("Данные обновлены", "Информация");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBusSearch_Click(object sender, EventArgs e)
        {
            PerformSearch(txtBusSearch.Text, dataGridViewBuses);
        }

        // ==================== РАСПИСАНИЕ ====================

        private void SetupScheduleHandlers()
        {
            btnScheduleAdd.Click += BtnScheduleAdd_Click;
            btnScheduleEdit.Click += BtnScheduleEdit_Click;
            btnScheduleDelete.Click += BtnScheduleDelete_Click;
            btnScheduleRefresh.Click += BtnScheduleRefresh_Click;
            btnScheduleSearch.Click += BtnScheduleSearch_Click;
            
            // Поиск при нажатии Enter
            txtScheduleSearch.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    PerformSearch(txtScheduleSearch.Text, dataGridViewSchedule);
                    e.Handled = true;
                }
            };
        }

        private void ShowSchedule()
        {
            try
            {
                dataGridViewSchedule.DataSource = null;
                dataGridViewSchedule.DataSource = scheduleData;
                UpdateStatusLabel(scheduleData.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отображения расписания: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnScheduleAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new ScheduleEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Получаем ID маршрута и автобуса из базы данных
                    int routeId = form.SelectedRouteId;
                    int busId = form.SelectedBusId;
                    
                    if (routeId <= 0 || busId <= 0)
                    {
                        MessageBox.Show("Выберите существующий маршрут и автобус", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Проверяем, существует ли уже такой рейс
                    bool exists = DataBase.CheckScheduleExists(routeId, busId, form.DepartureTime);
                    
                    if (exists)
                    {
                        MessageBox.Show("Такой рейс уже существует!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int availableSeats = form.AvailableSeats;

                    // Добавляем в базу данных
                    bool success = DataBase.InsertSchedule(
                        routeId,
                        busId,
                        form.DepartureTime,
                        form.ArrivalTime,
                        form.Price,
                        form.Status,
                        availableSeats
                    );

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowSchedule();
                        MessageBox.Show("Рейс добавлен", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении рейса", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnScheduleEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewSchedule.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите рейс для редактирования", "Внимание");
                    return;
                }

                var selectedRow = dataGridViewSchedule.SelectedRows[0];
                int scheduleId = SafeConvertToInt(selectedRow.Cells["ID"].Value);

                var form = new ScheduleEditForm();
                form.LoadScheduleData(
                    scheduleId,
                    SafeConvertToString(selectedRow.Cells["Маршрут"].Value),
                    SafeConvertToString(selectedRow.Cells["Автобус"].Value),
                    SafeConvertToDateTime(selectedRow.Cells["Отправление"].Value),
                    SafeConvertToDateTime(selectedRow.Cells["Прибытие"].Value),
                    SafeConvertToDecimal(selectedRow.Cells["Цена"].Value),
                    SafeConvertToString(selectedRow.Cells["Статус"].Value));

                if (form.ShowDialog() == DialogResult.OK)
                {
                    int routeId = form.SelectedRouteId;
                    int busId = form.SelectedBusId;
                    
                    if (routeId <= 0 || busId <= 0)
                    {
                        MessageBox.Show("Выберите существующий маршрут и автобус", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Проверяем, существует ли уже такой рейс (кроме текущего)
                    bool exists = DataBase.CheckScheduleExists(routeId, busId, form.DepartureTime, scheduleId);
                    
                    if (exists)
                    {
                        MessageBox.Show("Такой рейс уже существует!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int availableSeats = form.AvailableSeats;

                    // Обновляем в базе данных
                    bool success = DataBase.UpdateSchedule(
                        scheduleId,
                        routeId,
                        busId,
                        form.DepartureTime,
                        form.ArrivalTime,
                        form.Price,
                        form.Status,
                        availableSeats
                    );

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowSchedule();
                        MessageBox.Show("Рейс изменен", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении рейса", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnScheduleDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewSchedule.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите рейс для удаления", "Внимание");
                    return;
                }

                var result = MessageBox.Show("Удалить выбранный рейс?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var selectedRow = dataGridViewSchedule.SelectedRows[0];
                    int scheduleId = SafeConvertToInt(selectedRow.Cells["ID"].Value);

                    // Проверяем, есть ли связанные билеты
                    bool hasTickets = DataBase.CheckScheduleHasTickets(scheduleId);
                    
                    if (hasTickets)
                    {
                        var confirm = MessageBox.Show(
                            "У этого рейса есть проданные билеты.\n" +
                            "При удалении рейса все связанные билеты также будут удалены.\n\n" +
                            "Продолжить удаление?",
                            "Подтверждение удаления",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (confirm != DialogResult.Yes)
                            return;
                    }

                    // Удаляем из базы данных
                    bool success = DataBase.DeleteSchedule(scheduleId);

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowSchedule();
                        MessageBox.Show("Рейс удален", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении рейса", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnScheduleRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                txtScheduleSearch.Text = "";
                ShowAllRows(dataGridViewSchedule);
                UpdateStatusLabel(scheduleData.Rows.Count);
                MessageBox.Show("Данные обновлены", "Информация");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnScheduleSearch_Click(object sender, EventArgs e)
        {
            PerformSearch(txtScheduleSearch.Text, dataGridViewSchedule);
        }

        // ==================== БИЛЕТЫ ====================

        private void SetupTicketHandlers()
        {
            btnTicketAdd.Click += BtnTicketAdd_Click;
            btnTicketEdit.Click += BtnTicketEdit_Click;
            btnTicketDelete.Click += BtnTicketDelete_Click;
            btnTicketRefresh.Click += BtnTicketRefresh_Click;
            btnTicketSearch.Click += BtnTicketSearch_Click;
            
            // Поиск при нажатии Enter
            txtTicketSearch.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    PerformSearch(txtTicketSearch.Text, dataGridViewTickets);
                    e.Handled = true;
                }
            };
        }

        private void ShowTickets()
        {
            try
            {
                dataGridViewTickets.DataSource = null;
                dataGridViewTickets.DataSource = ticketsData;
                UpdateStatusLabel(ticketsData.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отображения билетов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTicketAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new TicketEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    int scheduleId = form.SelectedScheduleId;
                    
                    if (scheduleId <= 0)
                    {
                        MessageBox.Show("Выберите существующий рейс", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Проверяем, свободно ли место
                    bool isSeatAvailable = DataBase.CheckSeatAvailability(scheduleId, form.SeatNumber);
                    
                    if (!isSeatAvailable)
                    {
                        MessageBox.Show("Это место уже занято!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Проверяем, существует ли уже такой билет
                    bool exists = DataBase.CheckTicketExists(form.TicketNumber);
                    
                    if (exists)
                    {
                        MessageBox.Show("Билет с таким номером уже существует!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Добавляем в базу данных
                    bool success = DataBase.InsertTicket(
                        form.TicketNumber,
                        scheduleId,
                        form.PassengerName,
                        form.SeatNumber,
                        form.Price,
                        form.SaleDate,
                        form.IsReturned
                    );

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowTickets();
                        MessageBox.Show("Билет добавлен", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении билета", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTicketEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTickets.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите билет для редактирования", "Внимание");
                    return;
                }

                var selectedRow = dataGridViewTickets.SelectedRows[0];
                int ticketId = SafeConvertToInt(selectedRow.Cells["ID"].Value);

                var form = new TicketEditForm();
                form.LoadTicketData(
                    ticketId,
                    SafeConvertToString(selectedRow.Cells["Номер"].Value),
                    SafeConvertToString(selectedRow.Cells["Рейс"].Value),
                    SafeConvertToString(selectedRow.Cells["Пассажир"].Value),
                    SafeConvertToInt(selectedRow.Cells["Место"].Value),
                    SafeConvertToDecimal(selectedRow.Cells["Цена"].Value),
                    SafeConvertToDateTime(selectedRow.Cells["Дата продажи"].Value),
                    SafeConvertToBool(selectedRow.Cells["Возвращен"].Value));

                if (form.ShowDialog() == DialogResult.OK)
                {
                    int scheduleId = form.SelectedScheduleId;
                    
                    if (scheduleId <= 0)
                    {
                        MessageBox.Show("Выберите существующий рейс", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Проверяем, свободно ли место (если изменилось место)
                    int oldSeatNumber = SafeConvertToInt(selectedRow.Cells["Место"].Value);
                    if (form.SeatNumber != oldSeatNumber)
                    {
                        bool isSeatAvailable = DataBase.CheckSeatAvailability(scheduleId, form.SeatNumber, ticketId);
                        
                        if (!isSeatAvailable)
                        {
                            MessageBox.Show("Это место уже занято!", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Проверяем, существует ли уже такой билет (кроме текущего)
                    bool exists = DataBase.CheckTicketExists(form.TicketNumber, ticketId);
                    
                    if (exists)
                    {
                        MessageBox.Show("Билет с таким номером уже существует!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Обновляем в базе данных
                    bool success = DataBase.UpdateTicket(
                        ticketId,
                        form.TicketNumber,
                        scheduleId,
                        form.PassengerName,
                        form.SeatNumber,
                        form.Price,
                        form.SaleDate,
                        form.IsReturned
                    );

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowTickets();
                        MessageBox.Show("Билет изменен", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении билета", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTicketDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTickets.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите билет для удаления", "Внимание");
                    return;
                }

                var result = MessageBox.Show("Удалить выбранный билет?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var selectedRow = dataGridViewTickets.SelectedRows[0];
                    int ticketId = SafeConvertToInt(selectedRow.Cells["ID"].Value);

                    // Удаляем из базы данных
                    bool success = DataBase.DeleteTicket(ticketId);

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowTickets();
                        MessageBox.Show("Билет удален", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении билета", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTicketRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                txtTicketSearch.Text = "";
                ShowAllRows(dataGridViewTickets);
                UpdateStatusLabel(ticketsData.Rows.Count);
                MessageBox.Show("Данные обновлены", "Информация");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTicketSearch_Click(object sender, EventArgs e)
        {
            PerformSearch(txtTicketSearch.Text, dataGridViewTickets);
        }

        // ==================== ПОЛЬЗОВАТЕЛИ ====================

        private void SetupUserHandlers()
        {
            btnUserAdd.Click += BtnUserAdd_Click;
            btnUserEdit.Click += BtnUserEdit_Click;
            btnUserDelete.Click += BtnUserDelete_Click;
            btnUserRefresh.Click += BtnUserRefresh_Click;
            btnUserSearch.Click += BtnUserSearch_Click;
            
            // Поиск при нажатии Enter
            txtUserSearch.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    PerformSearch(txtUserSearch.Text, dataGridViewUsers);
                    e.Handled = true;
                }
            };
        }

        private void ShowUsers()
        {
            try
            {
                dataGridViewUsers.DataSource = null;
                dataGridViewUsers.DataSource = usersData;
                UpdateStatusLabel(usersData.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отображения пользователей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUserAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new UserEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Проверяем, существует ли уже такой пользователь
                    bool exists = DataBase.CheckUserExists(form.Username);
                    
                    if (exists)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string passwordHash = HashPassword(form.Password);

                    // Добавляем в базу данных
                    bool success = DataBase.InsertUser(
                        form.Username,
                        passwordHash,
                        form.FullName,
                        form.Role,
                        form.IsActive
                    );

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowUsers();
                        MessageBox.Show("Пользователь добавлен", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении пользователя", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUserEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewUsers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите пользователя для редактирования", "Внимание");
                    return;
                }

                var selectedRow = dataGridViewUsers.SelectedRows[0];
                int userId = SafeConvertToInt(selectedRow.Cells["ID"].Value);

                var form = new UserEditForm();
                form.LoadUserData(
                    userId,
                    SafeConvertToString(selectedRow.Cells["Логин"].Value),
                    SafeConvertToString(selectedRow.Cells["ФИО"].Value),
                    SafeConvertToString(selectedRow.Cells["Роль"].Value),
                    SafeConvertToBool(selectedRow.Cells["Активен"].Value));

                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Проверяем, существует ли уже такой пользователь (кроме текущего)
                    bool exists = DataBase.CheckUserExists(form.Username, userId);
                    
                    if (exists)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Хешируем пароль (только если он был изменен)
                    string passwordHash = "";
                    if (!string.IsNullOrEmpty(form.Password))
                    {
                        passwordHash = HashPassword(form.Password);
                    }
                    else
                    {
                        // Если пароль не был изменен, получаем старый хеш из базы
                        passwordHash = DataBase.GetUserPasswordHash(userId);
                    }

                    // Обновляем в базе данных
                    bool success = DataBase.UpdateUser(
                        userId,
                        form.Username,
                        passwordHash,
                        form.FullName,
                        form.Role,
                        form.IsActive
                    );

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowUsers();
                        MessageBox.Show("Пользователь изменен", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении пользователя", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUserDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewUsers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите пользователя для удаления", "Внимание");
                    return;
                }

                var selectedRow = dataGridViewUsers.SelectedRows[0];
                string username = SafeConvertToString(selectedRow.Cells["Логин"].Value);
                int userId = SafeConvertToInt(selectedRow.Cells["ID"].Value);

                // Нельзя удалить администратора
                if (username == "admin")
                {
                    MessageBox.Show("Нельзя удалить администратора системы", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show("Удалить выбранного пользователя?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Удаляем из базы данных
                    bool success = DataBase.DeleteUser(userId);

                    if (success)
                    {
                        // Обновляем данные из базы
                        LoadDataFromDatabase();
                        ShowUsers();
                        MessageBox.Show("Пользователь удален", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении пользователя", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUserRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                txtUserSearch.Text = "";
                ShowAllRows(dataGridViewUsers);
                UpdateStatusLabel(usersData.Rows.Count);
                MessageBox.Show("Данные обновлены", "Информация");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUserSearch_Click(object sender, EventArgs e)
        {
            PerformSearch(txtUserSearch.Text, dataGridViewUsers);
        }

        // ==================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ====================

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        // ==================== КНОПКА НАЗАД ====================

        private void BtnBack_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы хотите выйти из Базы Данных?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Закрываем текущую форму
                this.Close();
            }
        }

        // ==================== ДОПОЛНИТЕЛЬНЫЕ МЕТОДЫ ====================

        private void DataGridViewRoutes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dataGridViewRoutes.Columns[e.ColumnIndex].Name == "Активен" && e.Value != null)
                {
                    bool value = SafeConvertToBool(e.Value);

                    if (value)
                    {
                        e.Value = "✓";
                        e.CellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        e.Value = "✗";
                        e.CellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    e.FormattingApplied = true;
                }
            }
            catch
            {
                e.Value = "✗";
                e.CellStyle.ForeColor = System.Drawing.Color.Red;
                e.FormattingApplied = true;
            }
        }

        private void DataGridViewTickets_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                // Форматирование данных в таблице билетов
                if (dataGridViewTickets.Columns[e.ColumnIndex].Name == "Возвращен" && e.Value != null)
                {
                    bool value = SafeConvertToBool(e.Value);

                    if (value)
                    {
                        e.Value = "ДА";
                        e.CellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        e.Value = "НЕТ";
                        e.CellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                    e.FormattingApplied = true;
                }

                // Форматирование даты
                if (dataGridViewTickets.Columns[e.ColumnIndex].Name == "Дата продажи" && e.Value != null)
                {
                    DateTime date = SafeConvertToDateTime(e.Value);
                    if (date != DateTime.MinValue)
                    {
                        e.Value = date.ToString("dd.MM.yyyy HH:mm");
                        e.FormattingApplied = true;
                    }
                }
            }
            catch
            {
                // В случае ошибки оставляем значение как есть
            }
        }

        private void DataGridViewUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                // Форматирование данных в таблице пользователей
                if (dataGridViewUsers.Columns[e.ColumnIndex].Name == "Активен" && e.Value != null)
                {
                    bool value = SafeConvertToBool(e.Value);

                    if (value)
                    {
                        e.Value = "✓";
                        e.CellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        e.Value = "✗";
                        e.CellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    e.FormattingApplied = true;
                }

                // Форматирование роли
                if (dataGridViewUsers.Columns[e.ColumnIndex].Name == "Роль" && e.Value != null)
                {
                    string role = e.Value.ToString();
                    switch (role)
                    {
                        case "Администратор":
                            e.CellStyle.ForeColor = System.Drawing.Color.DarkRed;
                            break;
                        case "Диспетчер":
                            e.CellStyle.ForeColor = System.Drawing.Color.Blue;
                            break;
                        case "Кассир":
                            e.CellStyle.ForeColor = System.Drawing.Color.DarkGreen;
                            break;
                    }
                }
            }
            catch
            {
                // В случае ошибки оставляем значение как есть
            }
        }
    }
}