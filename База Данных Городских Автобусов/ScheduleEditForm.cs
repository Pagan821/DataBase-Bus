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
    public partial class ScheduleEditForm : Form
    {
        public string Route { get; private set; }
        public string Bus { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public DateTime ArrivalTime { get; private set; }
        public decimal Price { get; private set; }
        public string Status { get; private set; }

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
            cmbRoute.Items.AddRange(new string[] {
                "101 Москва-Санкт-Петербург",
                "202 Казань-Уфа",
                "305 Новосибирск-Томск",
                "102 Михайловск-Ставрополь",
                "98 Ипатово-Ставрополь",
                "228 Ипатово-Москва",
                "314 Санкт-Петербург-Питер"
            });

            cmbBus.Items.AddRange(new string[] {
                "А123ВС77 (Mercedes Tourismo)",
                "В456ОР78 (ПАЗ Vector Next)",
                "С789ТУ99 (ЛиАЗ 5292)",
                "E08AD937 (КАвЗ)",
                "D435J543 (НефаЗ-5299)"
            });

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

        public void LoadScheduleData(int id, string route, string bus,
            DateTime departure, DateTime arrival, decimal price, string status)
        {
            txtId.Text = id.ToString();

            // Устанавливаем значения в комбобоксы
            cmbRoute.Text = route;
            cmbBus.Text = bus;

            dtpDeparture.Value = departure;
            dtpArrival.Value = arrival;
            txtPrice.Text = price.ToString("0.00");
            cmbStatus.Text = status;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Route = cmbRoute.Text;
                Bus = cmbBus.Text;
                DepartureTime = dtpDeparture.Value;
                ArrivalTime = dtpArrival.Value;
                Price = decimal.Parse(txtPrice.Text);
                Status = cmbStatus.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
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