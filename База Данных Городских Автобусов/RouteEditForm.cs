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
    public partial class RouteEditForm : Form
    {
        public string RouteNumber { get; private set; }
        public string DepartureCity { get; private set; }
        public string ArrivalCity { get; private set; }
        public string Distance { get; private set; }
        public bool IsActive { get; private set; }

        public RouteEditForm()
        {
            InitializeComponent();

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            txtDuration.KeyPress += TxtDuration_KeyPress;
            txtDurationMinutes.KeyPress += TxtDurationMinutes_KeyPress;
            txtDuration.TextChanged += TxtDuration_TextChanged;
            txtDurationMinutes.TextChanged += TxtDurationMinutes_TextChanged;

            txtDistance.Enter += txtDistance_Enter;
            txtDistance.Leave += txtDistance_Leave;
            txtDuration.Enter += txtDuration_Enter;
            txtDuration.Leave += txtDuration_Leave;
            txtDurationMinutes.Enter += txtDurationMinutes_Enter;
            txtDurationMinutes.Leave += txtDurationMinutes_Leave;

            this.Load += RouteEditForm_Load;
        }

        public void LoadRouteData(int id, string routeNumber, string departureCity,
            string arrivalCity, string distance, bool isActive)
        {
            txtId.Text = id.ToString();
            txtRouteNumber.Text = routeNumber;
            txtDepartureCity.Text = departureCity;
            txtArrivalCity.Text = arrivalCity;
            txtDistance.Text = distance;
            chkActive.Checked = isActive;

            // Если поле расстояния пустое, показываем плейсхолдер
            if (string.IsNullOrEmpty(distance) || distance == "700 км")
            {
                txtDistance.ForeColor = SystemColors.GrayText;
            }
            else
            {
                txtDistance.ForeColor = SystemColors.WindowText;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                RouteNumber = txtRouteNumber.Text.Trim();
                DepartureCity = txtDepartureCity.Text.Trim();
                ArrivalCity = txtArrivalCity.Text.Trim();
                Distance = txtDistance.Text.Trim();
                IsActive = chkActive.Checked;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateInput()
        {
            // Проверка номера маршрута
            if (string.IsNullOrWhiteSpace(txtRouteNumber.Text))
            {
                MessageBox.Show("Введите номер маршрута", "Ошибка");
                txtRouteNumber.Focus();
                return false;
            }

            // Проверка города отправления
            if (string.IsNullOrWhiteSpace(txtDepartureCity.Text))
            {
                MessageBox.Show("Введите город отправления", "Ошибка");
                txtDepartureCity.Focus();
                return false;
            }

            // Проверка города прибытия
            if (string.IsNullOrWhiteSpace(txtArrivalCity.Text))
            {
                MessageBox.Show("Введите город прибытия", "Ошибка");
                txtArrivalCity.Focus();
                return false;
            }

            // Проверка расстояния
            if (string.IsNullOrWhiteSpace(txtDistance.Text) || txtDistance.Text == "700 км")
            {
                MessageBox.Show("Введите расстояние", "Ошибка");
                txtDistance.Focus();
                return false;
            }

            // Проверка формата расстояния (должно содержать "км")
            if (!txtDistance.Text.ToLower().Contains("км"))
            {
                var result = MessageBox.Show("Расстояние рекомендуется указывать с единицами измерения (км).\nПример: 700 км\n\nПродолжить?", "Внимание",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    txtDistance.Focus();
                    return false;
                }
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtDurationMinutes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ':')
            {
                e.Handled = true;
            }
        }

        private void TxtDuration_TextChanged(object sender, EventArgs e)
        {
            if (txtDuration.Text.Contains(":"))
            {
                string[] parts = txtDuration.Text.Split(':');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int hours) &&
                    int.TryParse(parts[1], out int minutes))
                {
                    if (hours >= 0 && minutes >= 0 && minutes < 60)
                    {
                        int totalMinutes = hours * 60 + minutes;
                        txtDurationMinutes.Text = totalMinutes.ToString();
                    }
                }
            }
        }

        private void TxtDurationMinutes_TextChanged(object sender, EventArgs e)
        {
            // Автоматический расчет формата ЧЧ:ММ при вводе минут
            if (!string.IsNullOrEmpty(txtDurationMinutes.Text) &&
                int.TryParse(txtDurationMinutes.Text, out int totalMinutes))
            {
                if (totalMinutes >= 0)
                {
                    int hours = totalMinutes / 60;
                    int minutes = totalMinutes % 60;
                    txtDuration.Text = $"{hours}:{minutes:D2}";
                }
            }
        }

        private void RouteEditForm_Load(object sender, EventArgs e)
        {
            // Устанавливаем текст по умолчанию вместо placeholder
            if (string.IsNullOrEmpty(txtDistance.Text) || txtDistance.Text == "700 км")
            {
                txtDistance.Text = "700 км";
                txtDistance.ForeColor = SystemColors.GrayText;
            }

            if (string.IsNullOrEmpty(txtDuration.Text) || txtDuration.Text == "10:30")
            {
                txtDuration.Text = "10:30";
                txtDuration.ForeColor = SystemColors.GrayText;
            }

            if (string.IsNullOrEmpty(txtDurationMinutes.Text) || txtDurationMinutes.Text == "630")
            {
                txtDurationMinutes.Text = "630";
                txtDurationMinutes.ForeColor = SystemColors.GrayText;
            }
        }

        private void txtDistance_Enter(object sender, EventArgs e)
        {
            if (txtDistance.Text == "700 км")
            {
                txtDistance.Text = "";
                txtDistance.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtDistance_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDistance.Text))
            {
                txtDistance.Text = "700 км";
                txtDistance.ForeColor = SystemColors.GrayText;
            }
        }

        private void txtDuration_Enter(object sender, EventArgs e)
        {
            if (txtDuration.Text == "10:30")
            {
                txtDuration.Text = "";
                txtDuration.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtDuration_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDuration.Text))
            {
                txtDuration.Text = "10:30";
                txtDuration.ForeColor = SystemColors.GrayText;
            }
        }

        private void txtDurationMinutes_Enter(object sender, EventArgs e)
        {
            if (txtDurationMinutes.Text == "630")
            {
                txtDurationMinutes.Text = "";
                txtDurationMinutes.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtDurationMinutes_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDurationMinutes.Text))
            {
                txtDurationMinutes.Text = "630";
                txtDurationMinutes.ForeColor = SystemColors.GrayText;
            }
        }
    }
}