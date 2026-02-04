using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace База_Данных_Городских_Автобусов
{
    public partial class ScheduleEditForm : Form
    {
        public string Route { get; private set; }
        public string Bus { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public DateTime ArrivalTime { get; private set; }
        public decimal Price { get; private set; }
        public string Status { get; private set; }

        public int SelectedRouteId { get; private set; }
        public int SelectedBusId { get; private set; }
        public int AvailableSeats { get; private set; }

        // Словари для хранения соответствия между текстом и ID
        private Dictionary<string, int> routeDictionary = new Dictionary<string, int>();
        private Dictionary<string, int> busDictionary = new Dictionary<string, int>();

        public ScheduleEditForm()
        {
            InitializeComponent();

            // Подписываемся на события
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            txtPrice.KeyPress += TxtPrice_KeyPress;
            this.Load += ScheduleEditForm_Load;
        }

        private void ScheduleEditForm_Load(object sender, EventArgs e)
        {
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            try
            {
                // Очищаем комбобоксы и словари
                cmbRoute.Items.Clear();
                cmbBus.Items.Clear();
                routeDictionary.Clear();
                busDictionary.Clear();

                // Загружаем маршруты из базы данных
                DataTable routes = DataBase.GetAllRoutes();
                foreach (DataRow row in routes.Rows)
                {
                    int routeId = Convert.ToInt32(row["route_id"]);
                    string routeNumber = row["route_number"].ToString();
                    string departure = row["departure_city"].ToString();
                    string arrival = row["arrival_city"].ToString();

                    string displayText = $"{routeNumber} {departure}-{arrival}";
                    string key = $"{routeId}|{displayText}";

                    cmbRoute.Items.Add(key);
                    routeDictionary[key] = routeId;
                }

                // Загружаем автобусы из базы данных
                DataTable buses = DataBase.GetAllBuses();
                foreach (DataRow row in buses.Rows)
                {
                    int busId = Convert.ToInt32(row["bus_id"]);
                    string plateNumber = row["plate_number"].ToString();
                    string brand = row["brand"].ToString();
                    string model = row["model"].ToString();

                    string displayText = $"{plateNumber} ({brand} {model})";
                    string key = $"{busId}|{displayText}";

                    cmbBus.Items.Add(key);
                    busDictionary[key] = busId;
                }

                // Статусы
                cmbStatus.Items.AddRange(new string[] {
                    "Планируется",
                    "Выполняется",
                    "Завершен",
                    "Отменен"
                });

                if (cmbRoute.Items.Count > 0) cmbRoute.SelectedIndex = 0;
                if (cmbBus.Items.Count > 0) cmbBus.SelectedIndex = 0;
                if (cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadScheduleData(int id, string route, string bus,
            DateTime departure, DateTime arrival, decimal price, string status)
        {
            txtId.Text = id.ToString();

            // Ищем соответствующий маршрут в словаре
            foreach (string key in routeDictionary.Keys)
            {
                if (key.Contains(route))
                {
                    cmbRoute.Text = key;
                    break;
                }
            }

            // Ищем соответствующий автобус в словаре
            foreach (string key in busDictionary.Keys)
            {
                if (key.Contains(bus))
                {
                    cmbBus.Text = key;
                    break;
                }
            }

            dtpDeparture.Value = departure;
            dtpArrival.Value = arrival;
            txtPrice.Text = price.ToString("0.00");
            cmbStatus.Text = status;

            // Получаем доступные места из базы данных для расчета
            AvailableSeats = CalculateAvailableSeats(id);
        }

        private int CalculateAvailableSeats(int scheduleId)
        {
            try
            {
                if (scheduleId <= 0) return 50; // Значение по умолчанию для нового рейса

                SQLiteConnection conn = DataBase.GetConnection();
                conn.Open();

                // Получаем вместимость автобуса и количество проданных билетов
                SQLiteCommand cmd = new SQLiteCommand(
                    @"SELECT b.capacity, 
                    COUNT(t.ticket_id) as sold_tickets
                    FROM Schedule s
                    JOIN Buses b ON s.bus_id = b.bus_id
                    LEFT JOIN Tickets t ON s.schedule_id = t.schedule_id AND t.is_returned = 0
                    WHERE s.schedule_id = @scheduleId
                    GROUP BY b.capacity", conn);
                cmd.Parameters.AddWithValue("@scheduleId", scheduleId);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int capacity = Convert.ToInt32(reader["capacity"]);
                        int soldTickets = Convert.ToInt32(reader["sold_tickets"]);
                        return capacity - soldTickets;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем значение по умолчанию
                Console.WriteLine($"Ошибка расчета свободных мест: {ex.Message}");
            }

            return 50; // Значение по умолчанию
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                // Извлекаем ID из выбранных значений
                if (cmbRoute.SelectedItem != null && routeDictionary.ContainsKey(cmbRoute.SelectedItem.ToString()))
                {
                    SelectedRouteId = routeDictionary[cmbRoute.SelectedItem.ToString()];
                    Route = cmbRoute.SelectedItem.ToString().Split('|')[1]; // Только текст для отображения
                }
                else if (!string.IsNullOrEmpty(cmbRoute.Text))
                {
                    // Если выбрано из выпадающего списка, но не выбрано через SelectedItem
                    foreach (var kvp in routeDictionary)
                    {
                        if (kvp.Key.Contains(cmbRoute.Text))
                        {
                            SelectedRouteId = kvp.Value;
                            Route = cmbRoute.Text;
                            break;
                        }
                    }
                }

                if (cmbBus.SelectedItem != null && busDictionary.ContainsKey(cmbBus.SelectedItem.ToString()))
                {
                    SelectedBusId = busDictionary[cmbBus.SelectedItem.ToString()];
                    Bus = cmbBus.SelectedItem.ToString().Split('|')[1]; // Только текст для отображения
                }
                else if (!string.IsNullOrEmpty(cmbBus.Text))
                {
                    // Если выбрано из выпадающего списка, но не выбрано через SelectedItem
                    foreach (var kvp in busDictionary)
                    {
                        if (kvp.Key.Contains(cmbBus.Text))
                        {
                            SelectedBusId = kvp.Value;
                            Bus = cmbBus.Text;
                            break;
                        }
                    }
                }

                DepartureTime = dtpDeparture.Value;
                ArrivalTime = dtpArrival.Value;
                Price = decimal.Parse(txtPrice.Text);
                Status = cmbStatus.Text;

                // Для новых рейсов устанавливаем доступные места равными вместимости автобуса
                if (string.IsNullOrEmpty(txtId.Text) || txtId.Text == "0")
                {
                    AvailableSeats = GetBusCapacity(SelectedBusId);
                }
                else
                {
                    AvailableSeats = CalculateAvailableSeats(Convert.ToInt32(txtId.Text));
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private int GetBusCapacity(int busId)
        {
            try
            {
                SQLiteConnection conn = DataBase.GetConnection();
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(
                    "SELECT capacity FROM Buses WHERE bus_id = @busId", conn);
                cmd.Parameters.AddWithValue("@busId", busId);

                object result = cmd.ExecuteScalar();
                conn.Close();

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения вместимости автобуса: {ex.Message}");
            }

            return 50; // Значение по умолчанию
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(cmbRoute.Text))
            {
                MessageBox.Show("Выберите маршрут", "Ошибка");
                cmbRoute.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbBus.Text))
            {
                MessageBox.Show("Выберите автобус", "Ошибка");
                cmbBus.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Введите цену", "Ошибка");
                txtPrice.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (положительное число)", "Ошибка");
                txtPrice.Focus();
                return false;
            }

            if (dtpArrival.Value <= dtpDeparture.Value)
            {
                MessageBox.Show("Время прибытия должно быть позже времени отправления", "Ошибка");
                dtpArrival.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',')
            {
                e.KeyChar = '.';
            }

            if ((e.KeyChar == '.') && (txtPrice.Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}