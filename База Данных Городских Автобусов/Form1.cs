using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            InitializeComponent();

            // Инициализируем базу данных
            InitializeDatabase();

            InitializeDataTables();

            LoadInitialData();

            SetupEventHandlers();
        }

        private void InitializeDatabase()
        {
            // Инициализируем базу данных при запуске программы
            DataBase.InitializeDatabase();
        }

        private void InitializeDataTables()
        {
            // Структура таблиц остается прежней
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
        }

        private void LoadDataFromDatabase()
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
                routesData.Rows.Add(
                    row["route_id"],
                    row["route_number"],
                    row["departure_city"],
                    row["arrival_city"],
                    row["distance"],
                    Convert.ToInt32(row["is_active"]) == 1
                );
            }

            // Загружаем автобусы
            DataTable busesDb = DataBase.GetAllBuses();
            foreach (DataRow row in busesDb.Rows)
            {
                busesData.Rows.Add(
                    row["bus_id"],
                    row["plate_number"],
                    row["brand"],
                    row["model"],
                    row["capacity"],
                    row["year"] == DBNull.Value ? 0 : row["year"],
                    Convert.ToInt32(row["is_active"]) == 1
                );
            }

            // Загружаем расписание
            DataTable scheduleDb = DataBase.GetAllSchedules();
            foreach (DataRow row in scheduleDb.Rows)
            {
                string routeInfo = $"{row["route_number"]} {row["departure_city"]}-{row["arrival_city"]}";
                string busInfo = $"{row["plate_number"]} ({row["brand"]} {row["model"]})";

                scheduleData.Rows.Add(
                    row["schedule_id"],
                    routeInfo,
                    busInfo,
                    row["departure_time"],
                    row["arrival_time"],
                    row["price"],
                    row["status"]
                );
            }

            // Загружаем билеты
            DataTable ticketsDb = DataBase.GetAllTickets();
            foreach (DataRow row in ticketsDb.Rows)
            {
                string routeInfo = $"{row["route_number"]} {row["departure_city"]}-{row["arrival_city"]}";

                ticketsData.Rows.Add(
                    row["ticket_id"],
                    row["ticket_number"],
                    routeInfo,
                    row["passenger_name"],
                    row["seat_number"],
                    row["price"],
                    row["sale_date"],
                    Convert.ToInt32(row["is_returned"]) == 1
                );
            }

            // Загружаем пользователей
            DataTable usersDb = DataBase.GetAllUsers();
            foreach (DataRow row in usersDb.Rows)
            {
                usersData.Rows.Add(
                    row["user_id"],
                    row["username"],
                    row["full_name"],
                    row["role"],
                    Convert.ToInt32(row["is_active"]) == 1
                );
            }
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
        }

        private void ShowRoutes()
        {
            dataGridViewRoutes.DataSource = routesData;
            UpdateStatusLabel(routesData.Rows.Count);
        }

        private void BtnRouteAdd_Click(object sender, EventArgs e)
        {
            var form = new RouteEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
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

        private void BtnRouteEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewRoutes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите маршрут для редактирования", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridViewRoutes.SelectedRows[0];
            int routeId = (int)selectedRow.Cells["ID"].Value;

            var form = new RouteEditForm();
            form.LoadRouteData(
                routeId,
                selectedRow.Cells["Номер"].Value.ToString(),
                selectedRow.Cells["Отправление"].Value.ToString(),
                selectedRow.Cells["Прибытие"].Value.ToString(),
                selectedRow.Cells["Расстояние"].Value.ToString(),
                (bool)selectedRow.Cells["Активен"].Value);

            if (form.ShowDialog() == DialogResult.OK)
            {
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

        private void BtnRouteDelete_Click(object sender, EventArgs e)
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
                int routeId = (int)selectedRow.Cells["ID"].Value;

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

        private void BtnRouteRefresh_Click(object sender, EventArgs e)
        {
            txtRouteSearch.Text = "";
            ShowAllRows(dataGridViewRoutes);
            UpdateStatusLabel(routesData.Rows.Count);
            MessageBox.Show("Данные обновлены", "Информация");
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
        }

        private void ShowBuses()
        {
            dataGridViewBuses.DataSource = busesData;
            UpdateStatusLabel(busesData.Rows.Count);
        }

        private void BtnBusAdd_Click(object sender, EventArgs e)
        {
            var form = new BusEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
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

        private void BtnBusEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewBuses.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите автобус для редактирования", "Внимание");
                return;
            }

            var selectedRow = dataGridViewBuses.SelectedRows[0];
            int busId = (int)selectedRow.Cells["ID"].Value;

            var form = new BusEditForm();
            form.LoadBusData(
                busId,
                selectedRow.Cells["Гос. номер"].Value.ToString(),
                selectedRow.Cells["Марка"].Value.ToString(),
                selectedRow.Cells["Модель"].Value.ToString(),
                (int)selectedRow.Cells["Вместимость"].Value,
                (int)selectedRow.Cells["Год"].Value,
                (bool)selectedRow.Cells["Активен"].Value);

            if (form.ShowDialog() == DialogResult.OK)
            {
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

        private void BtnBusDelete_Click(object sender, EventArgs e)
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
                int busId = (int)selectedRow.Cells["ID"].Value;

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

        private void BtnBusRefresh_Click(object sender, EventArgs e)
        {
            txtBusSearch.Text = "";
            ShowAllRows(dataGridViewBuses);
            UpdateStatusLabel(busesData.Rows.Count);
            MessageBox.Show("Данные обновлены", "Информация");
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
        }

        private void ShowSchedule()
        {
            dataGridViewSchedule.DataSource = scheduleData;
            UpdateStatusLabel(scheduleData.Rows.Count);
        }

        private void BtnScheduleAdd_Click(object sender, EventArgs e)
        {
            // В реальном приложении нужно получать route_id и bus_id из выбранных значений
            // Здесь упрощенная версия
            var form = new ScheduleEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                // В реальном приложении нужно получать ID из базы данных
                // Здесь используем фиктивные значения для демонстрации
                int routeId = 1;
                int busId = 1;
                int availableSeats = 50; // По умолчанию

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

        private void BtnScheduleEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewSchedule.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите рейс для редактирования", "Внимание");
                return;
            }

            var selectedRow = dataGridViewSchedule.SelectedRows[0];
            int scheduleId = (int)selectedRow.Cells["ID"].Value;

            var form = new ScheduleEditForm();
            form.LoadScheduleData(
                scheduleId,
                selectedRow.Cells["Маршрут"].Value.ToString(),
                selectedRow.Cells["Автобус"].Value.ToString(),
                (DateTime)selectedRow.Cells["Отправление"].Value,
                (DateTime)selectedRow.Cells["Прибытие"].Value,
                (decimal)selectedRow.Cells["Цена"].Value,
                selectedRow.Cells["Статус"].Value.ToString());

            if (form.ShowDialog() == DialogResult.OK)
            {
                int routeId = 1;
                int busId = 1;
                int availableSeats = 50; // По умолчанию

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

        private void BtnScheduleDelete_Click(object sender, EventArgs e)
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
                int scheduleId = (int)selectedRow.Cells["ID"].Value;

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

        private void BtnScheduleRefresh_Click(object sender, EventArgs e)
        {
            txtScheduleSearch.Text = "";
            ShowAllRows(dataGridViewSchedule);
            UpdateStatusLabel(scheduleData.Rows.Count);
            MessageBox.Show("Данные обновлены", "Информация");
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
        }

        private void ShowTickets()
        {
            dataGridViewTickets.DataSource = ticketsData;
            UpdateStatusLabel(ticketsData.Rows.Count);
        }

        private void BtnTicketAdd_Click(object sender, EventArgs e)
        {
            var form = new TicketEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                int scheduleId = 1;

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

        private void BtnTicketEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewTickets.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите билет для редактирования", "Внимание");
                return;
            }

            var selectedRow = dataGridViewTickets.SelectedRows[0];
            int ticketId = (int)selectedRow.Cells["ID"].Value;

            var form = new TicketEditForm();
            form.LoadTicketData(
                ticketId,
                selectedRow.Cells["Номер"].Value.ToString(),
                selectedRow.Cells["Рейс"].Value.ToString(),
                selectedRow.Cells["Пассажир"].Value.ToString(),
                (int)selectedRow.Cells["Место"].Value,
                (decimal)selectedRow.Cells["Цена"].Value,
                (DateTime)selectedRow.Cells["Дата продажи"].Value,
                (bool)selectedRow.Cells["Возвращен"].Value);

            if (form.ShowDialog() == DialogResult.OK)
            {
                int scheduleId = 1;

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

        private void BtnTicketDelete_Click(object sender, EventArgs e)
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
                int ticketId = (int)selectedRow.Cells["ID"].Value;

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

        private void BtnTicketRefresh_Click(object sender, EventArgs e)
        {
            txtTicketSearch.Text = "";
            ShowAllRows(dataGridViewTickets);
            UpdateStatusLabel(ticketsData.Rows.Count);
            MessageBox.Show("Данные обновлены", "Информация");
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
        }

        private void ShowUsers()
        {
            dataGridViewUsers.DataSource = usersData;
            UpdateStatusLabel(usersData.Rows.Count);
        }

        private void BtnUserAdd_Click(object sender, EventArgs e)
        {
            var form = new UserEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
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

        private void BtnUserEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для редактирования", "Внимание");
                return;
            }

            var selectedRow = dataGridViewUsers.SelectedRows[0];
            int userId = (int)selectedRow.Cells["ID"].Value;

            var form = new UserEditForm();
            form.LoadUserData(
                userId,
                selectedRow.Cells["Логин"].Value.ToString(),
                selectedRow.Cells["ФИО"].Value.ToString(),
                selectedRow.Cells["Роль"].Value.ToString(),
                (bool)selectedRow.Cells["Активен"].Value);

            if (form.ShowDialog() == DialogResult.OK)
            {
                // Хешируем пароль
                string passwordHash = HashPassword(form.Password);

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

        private void BtnUserDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления", "Внимание");
                return;
            }

            var selectedRow = dataGridViewUsers.SelectedRows[0];
            string username = selectedRow.Cells["Логин"].Value.ToString();
            int userId = (int)selectedRow.Cells["ID"].Value;

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

        private void BtnUserRefresh_Click(object sender, EventArgs e)
        {
            txtUserSearch.Text = "";
            ShowAllRows(dataGridViewUsers);
            UpdateStatusLabel(usersData.Rows.Count);
            MessageBox.Show("Данные обновлены", "Информация");
        }

        private void BtnUserSearch_Click(object sender, EventArgs e)
        {
            PerformSearch(txtUserSearch.Text, dataGridViewUsers);
        }

        // ==================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ====================

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                // Используем BitConverter для преобразования байтов в строку
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        // ==================== КНОПКА НАЗАД ====================

        private void BtnBack_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вернуться в главное меню?", "Подтверждение",
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
            // Форматирование данных в таблице маршрутов
            if (dataGridViewRoutes.Columns[e.ColumnIndex].Name == "Активен" && e.Value != null)
            {
                if ((bool)e.Value == true)
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

        private void DataGridViewTickets_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Форматирование данных в таблице билетов
            if (dataGridViewTickets.Columns[e.ColumnIndex].Name == "Возвращен" && e.Value != null)
            {
                if ((bool)e.Value == true)
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
                if (e.Value is DateTime)
                {
                    e.Value = ((DateTime)e.Value).ToString("dd.MM.yyyy HH:mm");
                    e.FormattingApplied = true;
                }
            }
        }
    }
}