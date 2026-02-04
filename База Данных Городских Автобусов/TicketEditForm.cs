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
    public partial class TicketEditForm : Form
    {
        public string TicketNumber { get; private set; }
        public string Schedule { get; private set; }
        public string PassengerName { get; private set; }
        public int SeatNumber { get; private set; }
        public decimal Price { get; private set; }
        public DateTime SaleDate { get; private set; }
        public bool IsReturned { get; private set; }

        public int SelectedScheduleId { get; private set; }

        // Словарь для хранения соответствия между текстом и ID расписания
        private Dictionary<string, int> scheduleDictionary = new Dictionary<string, int>();

        public TicketEditForm()
        {
            InitializeComponent();

            // Подписываемся на события
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnGenerateNumber.Click += BtnGenerateNumber_Click;
            txtSeatNumber.KeyPress += TxtSeatNumber_KeyPress;
            txtPrice.KeyPress += TxtPrice_KeyPress;
            this.Load += TicketEditForm_Load;
        }

        private void TicketEditForm_Load(object sender, EventArgs e)
        {
            LoadComboBoxData();
            GenerateTicketNumber();
        }

        private void LoadComboBoxData()
        {
            try
            {
                // Очищаем комбобокс и словарь
                cmbSchedule.Items.Clear();
                scheduleDictionary.Clear();

                // Загружаем расписание из базы данных
                DataTable schedules = DataBase.GetAllSchedules();
                foreach (DataRow row in schedules.Rows)
                {
                    int scheduleId = Convert.ToInt32(row["schedule_id"]);
                    string routeNumber = row["route_number"].ToString();
                    string departureCity = row["departure_city"].ToString();
                    string arrivalCity = row["arrival_city"].ToString();
                    DateTime departureTime = Convert.ToDateTime(row["departure_time"]);

                    string displayText = $"{routeNumber} {departureCity}-{arrivalCity} ({departureTime:dd.MM.yyyy HH:mm})";
                    string key = $"{scheduleId}|{displayText}";

                    cmbSchedule.Items.Add(key);
                    scheduleDictionary[key] = scheduleId;
                }

                if (cmbSchedule.Items.Count > 0) cmbSchedule.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки расписания: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadTicketData(int id, string ticketNumber, string schedule,
            string passengerName, int seatNumber, decimal price, DateTime saleDate, bool isReturned)
        {
            txtId.Text = id.ToString();
            txtTicketNumber.Text = ticketNumber;

            // Ищем соответствующее расписание в словаре
            foreach (string key in scheduleDictionary.Keys)
            {
                if (key.Contains(schedule))
                {
                    cmbSchedule.Text = key;
                    break;
                }
            }

            txtPassengerName.Text = passengerName;
            txtSeatNumber.Text = seatNumber.ToString();
            txtPrice.Text = price.ToString("0.00");
            dtpSaleDate.Value = saleDate;
            chkIsReturned.Checked = isReturned;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                TicketNumber = txtTicketNumber.Text.Trim();

                // Извлекаем ID расписания из выбранного значения
                if (cmbSchedule.SelectedItem != null && scheduleDictionary.ContainsKey(cmbSchedule.SelectedItem.ToString()))
                {
                    SelectedScheduleId = scheduleDictionary[cmbSchedule.SelectedItem.ToString()];
                    Schedule = cmbSchedule.SelectedItem.ToString().Split('|')[1]; // Только текст для отображения
                }
                else if (!string.IsNullOrEmpty(cmbSchedule.Text))
                {
                    // Пытаемся найти ID по тексту
                    foreach (var kvp in scheduleDictionary)
                    {
                        if (kvp.Key.Contains(cmbSchedule.Text))
                        {
                            SelectedScheduleId = kvp.Value;
                            Schedule = cmbSchedule.Text;
                            break;
                        }
                    }
                }

                PassengerName = txtPassengerName.Text.Trim();
                SeatNumber = string.IsNullOrEmpty(txtSeatNumber.Text) ? 0 : int.Parse(txtSeatNumber.Text);
                Price = decimal.Parse(txtPrice.Text);
                SaleDate = dtpSaleDate.Value;
                IsReturned = chkIsReturned.Checked;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(cmbSchedule.Text))
            {
                MessageBox.Show("Выберите рейс", "Ошибка");
                cmbSchedule.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTicketNumber.Text))
            {
                MessageBox.Show("Введите номер билета", "Ошибка");
                txtTicketNumber.Focus();
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

            if (string.IsNullOrWhiteSpace(txtSeatNumber.Text))
            {
                MessageBox.Show("Введите номер места", "Ошибка");
                txtSeatNumber.Focus();
                return false;
            }

            if (!int.TryParse(txtSeatNumber.Text, out int seat) || seat <= 0)
            {
                MessageBox.Show("Введите корректный номер места (положительное число)", "Ошибка");
                txtSeatNumber.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassengerName.Text))
            {
                MessageBox.Show("Введите ФИО пассажира", "Ошибка");
                txtPassengerName.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnGenerateNumber_Click(object sender, EventArgs e)
        {
            GenerateTicketNumber();
        }

        private void GenerateTicketNumber()
        {
            string prefix = "TKT";
            string date = DateTime.Now.ToString("yyMMdd");
            string random = new Random().Next(1000, 9999).ToString();

            txtTicketNumber.Text = $"{prefix}{date}-{random}";
        }

        private void TxtSeatNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',') e.KeyChar = '.';

            if ((e.KeyChar == '.') && (txtPrice.Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}