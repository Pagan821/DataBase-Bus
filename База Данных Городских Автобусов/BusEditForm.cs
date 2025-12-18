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
    public partial class BusEditForm : Form
    {
        public string PlateNumber { get; private set; }
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public int Capacity { get; private set; }
        public int Year { get; private set; }
        public bool IsActive { get; private set; }

        public BusEditForm()
        {
            InitializeComponent();

            // Подписываемся на события
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            txtCapacity.KeyPress += TxtCapacity_KeyPress;
            txtYear.KeyPress += TxtYear_KeyPress;
        }

        public void LoadBusData(int id, string plateNumber, string brand,
            string model, int capacity, int year, bool isActive)
        {
            txtId.Text = id.ToString();
            txtPlateNumber.Text = plateNumber;
            txtBrand.Text = brand;
            txtModel.Text = model;
            txtCapacity.Text = capacity.ToString();
            txtYear.Text = year.ToString();
            chkActive.Checked = isActive;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                PlateNumber = txtPlateNumber.Text.Trim();
                Brand = txtBrand.Text.Trim();
                Model = txtModel.Text.Trim();
                Capacity = int.Parse(txtCapacity.Text);
                Year = string.IsNullOrEmpty(txtYear.Text) ? DateTime.Now.Year : int.Parse(txtYear.Text);
                IsActive = chkActive.Checked;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtPlateNumber.Text))
            {
                MessageBox.Show("Введите государственный номер", "Ошибка");
                txtPlateNumber.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtBrand.Text))
            {
                MessageBox.Show("Введите марку автобуса", "Ошибка");
                txtBrand.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtModel.Text))
            {
                MessageBox.Show("Введите модель автобуса", "Ошибка");
                txtModel.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCapacity.Text))
            {
                MessageBox.Show("Введите вместимость", "Ошибка");
                txtCapacity.Focus();
                return false;
            }

            if (!int.TryParse(txtCapacity.Text, out int capacity) || capacity <= 0)
            {
                MessageBox.Show("Введите корректную вместимость (положительное число)", "Ошибка");
                txtCapacity.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(txtYear.Text) &&
                (!int.TryParse(txtYear.Text, out int year) || year < 1900 || year > DateTime.Now.Year + 1))
            {
                MessageBox.Show("Введите корректный год выпуска (1900-" + (DateTime.Now.Year + 1) + ")", "Ошибка");
                txtYear.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtCapacity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}