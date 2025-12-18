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
            // В реальности здесь загрузка из БД
            cmbSchedule.Items.AddRange(new string[] {
                "101 Москва-СПб (17.12.2024 10:00)",
                "202 Казань-Уфа (18.12.2024 14:00)",
                "305 Новосибирск-Томск (19.12.2024 08:00)",
                "124 Ипатово-Ставрополь (21.7.2024)",
                "228 Ипатово-Москва (22.06.2025)",
                 "314 Санкт-Петербург-Питер (04.04.2025)" 

            });

            if (cmbSchedule.Items.Count > 0) cmbSchedule.SelectedIndex = 0;
        }

        public void LoadTicketData(int id, string ticketNumber, string schedule,
            string passengerName, int seatNumber, decimal price, DateTime saleDate, bool isReturned)
        {
            txtId.Text = id.ToString();
            txtTicketNumber.Text = ticketNumber;
            cmbSchedule.Text = schedule;
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
                Schedule = cmbSchedule.Text;
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

            if (!string.IsNullOrEmpty(txtSeatNumber.Text))
            {
                if (!int.TryParse(txtSeatNumber.Text, out int seat) || seat < 0)
                {
                    MessageBox.Show("Введите корректный номер места (положительное число)", "Ошибка");
                    txtSeatNumber.Focus();
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