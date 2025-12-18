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

            InitializeDataTables();

            LoadInitialData();

            SetupEventHandlers();
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
            // Заполняем тестовыми данными
            LoadTestData();

            // Показываем первую вкладку
            ShowRoutes();
        }

        private void LoadTestData()
        {
            // Тестовые маршруты
            routesData.Rows.Add(1, "101", "Москва", "Санкт-Петербург", "700 км", true);
            routesData.Rows.Add(2, "202", "Казань", "Уфа", "450 км", true);
            routesData.Rows.Add(3, "305", "Новосибирск", "Томск", "250 км", false);

            // Тестовые автобусы
            busesData.Rows.Add(1, "А123ВС77", "Mercedes", "Tourismo", 50, 2022, true);
            busesData.Rows.Add(2, "В456ОР78", "ПАЗ", "Vector Next", 35, 2021, true);
            busesData.Rows.Add(3, "С789ТУ99", "ЛиАЗ", "5292", 45, 2020, false);

            // Тестовое расписание
            scheduleData.Rows.Add(1, "101 Москва-СПб", "А123ВС77",
                DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(10),
                2500.00m, "Планируется");
            scheduleData.Rows.Add(2, "202 Казань-Уфа", "В456ОР78",
                DateTime.Now.AddDays(2), DateTime.Now.AddDays(2).AddHours(6),
                1800.00m, "Планируется");

            // Тестовые билеты
            ticketsData.Rows.Add(1, "TKT001", "101 Москва-СПб", "Иванов И.И.",
                15, 2500.00m, DateTime.Now.AddDays(-1), false);
            ticketsData.Rows.Add(2, "TKT002", "101 Москва-СПб", "Петров П.П.",
                16, 2500.00m, DateTime.Now.AddDays(-2), false);
            ticketsData.Rows.Add(3, "TKT003", "202 Казань-Уфа", "Сидоров С.С.",
                8, 1800.00m, DateTime.Now.AddDays(-3), true);

            // Тестовые пользователи
            usersData.Rows.Add(1, "admin", "Администратор Системы", "Администратор", true);
            usersData.Rows.Add(2, "dispatcher", "Диспетчер Иванов", "Диспетчер", true);
            usersData.Rows.Add(3, "cashier1", "Кассир Петрова", "Кассир", true);
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
                // В реальности здесь добавление в БД
                routesData.Rows.Add(
                    routesData.Rows.Count + 1,
                    form.RouteNumber,
                    form.DepartureCity,
                    form.ArrivalCity,
                    form.Distance,
                    form.IsActive);

                ShowRoutes();
                MessageBox.Show("Маршрут добавлен", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                // В реальности здесь обновление в БД
                selectedRow.Cells["Номер"].Value = form.RouteNumber;
                selectedRow.Cells["Отправление"].Value = form.DepartureCity;
                selectedRow.Cells["Прибытие"].Value = form.ArrivalCity;
                selectedRow.Cells["Расстояние"].Value = form.Distance;
                selectedRow.Cells["Активен"].Value = form.IsActive;

                MessageBox.Show("Маршрут изменен", "Успех");
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

                // В реальности здесь удаление из БД
                routesData.Rows.RemoveAt(selectedRow.Index);

                ShowRoutes();
                MessageBox.Show("Маршрут удален", "Успех");
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
                // В реальности здесь добавление в БД
                busesData.Rows.Add(
                    busesData.Rows.Count + 1,
                    form.PlateNumber,
                    form.Brand,
                    form.Model,
                    form.Capacity,
                    form.Year,
                    form.IsActive);

                ShowBuses();
                MessageBox.Show("Автобус добавлен", "Успех");
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
            var form = new BusEditForm();
            form.LoadBusData(
                (int)selectedRow.Cells["ID"].Value,
                selectedRow.Cells["Гос. номер"].Value.ToString(),
                selectedRow.Cells["Марка"].Value.ToString(),
                selectedRow.Cells["Модель"].Value.ToString(),
                (int)selectedRow.Cells["Вместимость"].Value,
                (int)selectedRow.Cells["Год"].Value,
                (bool)selectedRow.Cells["Активен"].Value);

            if (form.ShowDialog() == DialogResult.OK)
            {
                // Обновление данных
                selectedRow.Cells["Гос. номер"].Value = form.PlateNumber;
                selectedRow.Cells["Марка"].Value = form.Brand;
                selectedRow.Cells["Модель"].Value = form.Model;
                selectedRow.Cells["Вместимость"].Value = form.Capacity;
                selectedRow.Cells["Год"].Value = form.Year;
                selectedRow.Cells["Активен"].Value = form.IsActive;

                MessageBox.Show("Автобус изменен", "Успех");
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
                busesData.Rows.RemoveAt(selectedRow.Index);

                ShowBuses();
                MessageBox.Show("Автобус удален", "Успех");
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
            var form = new ScheduleEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                scheduleData.Rows.Add(
                    scheduleData.Rows.Count + 1,
                    form.Route,
                    form.Bus,
                    form.DepartureTime,
                    form.ArrivalTime,
                    form.Price,
                    form.Status);

                ShowSchedule();
                MessageBox.Show("Рейс добавлен", "Успех");
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
            var form = new ScheduleEditForm();
            form.LoadScheduleData(
                (int)selectedRow.Cells["ID"].Value,
                selectedRow.Cells["Маршрут"].Value.ToString(),
                selectedRow.Cells["Автобус"].Value.ToString(),
                (DateTime)selectedRow.Cells["Отправление"].Value,
                (DateTime)selectedRow.Cells["Прибытие"].Value,
                (decimal)selectedRow.Cells["Цена"].Value,
                selectedRow.Cells["Статус"].Value.ToString());

            if (form.ShowDialog() == DialogResult.OK)
            {
                selectedRow.Cells["Маршрут"].Value = form.Route;
                selectedRow.Cells["Автобус"].Value = form.Bus;
                selectedRow.Cells["Отправление"].Value = form.DepartureTime;
                selectedRow.Cells["Прибытие"].Value = form.ArrivalTime;
                selectedRow.Cells["Цена"].Value = form.Price;
                selectedRow.Cells["Статус"].Value = form.Status;

                MessageBox.Show("Рейс изменен", "Успех");
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
                scheduleData.Rows.RemoveAt(selectedRow.Index);

                ShowSchedule();
                MessageBox.Show("Рейс удален", "Успех");
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
                // В реальности здесь добавление в БД
                ticketsData.Rows.Add(
                    ticketsData.Rows.Count + 1,
                    form.TicketNumber,
                    form.Schedule,
                    form.PassengerName,
                    form.SeatNumber,
                    form.Price,
                    form.SaleDate,
                    form.IsReturned);

                ShowTickets();
                MessageBox.Show("Билет добавлен", "Успех");
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
            var form = new TicketEditForm();
            form.LoadTicketData(
                (int)selectedRow.Cells["ID"].Value,
                selectedRow.Cells["Номер"].Value.ToString(),
                selectedRow.Cells["Рейс"].Value.ToString(),
                selectedRow.Cells["Пассажир"].Value.ToString(),
                (int)selectedRow.Cells["Место"].Value,
                (decimal)selectedRow.Cells["Цена"].Value,
                (DateTime)selectedRow.Cells["Дата продажи"].Value,
                (bool)selectedRow.Cells["Возвращен"].Value);

            if (form.ShowDialog() == DialogResult.OK)
            {
                selectedRow.Cells["Номер"].Value = form.TicketNumber;
                selectedRow.Cells["Рейс"].Value = form.Schedule;
                selectedRow.Cells["Пассажир"].Value = form.PassengerName;
                selectedRow.Cells["Место"].Value = form.SeatNumber;
                selectedRow.Cells["Цена"].Value = form.Price;
                selectedRow.Cells["Дата продажи"].Value = form.SaleDate;
                selectedRow.Cells["Возвращен"].Value = form.IsReturned;

                MessageBox.Show("Билет изменен", "Успех");
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
                ticketsData.Rows.RemoveAt(selectedRow.Index);

                ShowTickets();
                MessageBox.Show("Билет удален", "Успех");
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
                // В реальности здесь добавление в БД
                usersData.Rows.Add(
                    usersData.Rows.Count + 1,
                    form.Username,
                    form.FullName,
                    form.Role,
                    form.IsActive);

                ShowUsers();
                MessageBox.Show("Пользователь добавлен", "Успех");
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
            var form = new UserEditForm();
            form.LoadUserData(
                (int)selectedRow.Cells["ID"].Value,
                selectedRow.Cells["Логин"].Value.ToString(),
                selectedRow.Cells["ФИО"].Value.ToString(),
                selectedRow.Cells["Роль"].Value.ToString(),
                (bool)selectedRow.Cells["Активен"].Value);

            if (form.ShowDialog() == DialogResult.OK)
            {
                selectedRow.Cells["Логин"].Value = form.Username;
                selectedRow.Cells["ФИО"].Value = form.FullName;
                selectedRow.Cells["Роль"].Value = form.Role;
                selectedRow.Cells["Активен"].Value = form.IsActive;

                MessageBox.Show("Пользователь изменен", "Успех");
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
                usersData.Rows.RemoveAt(selectedRow.Index);

                ShowUsers();
                MessageBox.Show("Пользователь удален", "Успех");
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

        // ==================== КНОПКА НАЗАД ====================

        private void BtnBack_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вернуться в главное меню?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Закрываем текущую форму
                this.Close();

                // В реальности здесь можно открыть главное меню:
                // MainMenuForm mainForm = new MainMenuForm();
                // mainForm.Show();
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