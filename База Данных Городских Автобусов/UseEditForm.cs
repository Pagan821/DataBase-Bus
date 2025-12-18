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
    public partial class UserEditForm : Form
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string FullName { get; private set; }
        public string Role { get; private set; }
        public bool IsActive { get; private set; }

        public UserEditForm()
        {
            InitializeComponent();

            // Подписываемся на события
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnGeneratePassword.Click += BtnGeneratePassword_Click;
            txtUsername.KeyPress += TxtUsername_KeyPress;
            this.Load += UserEditForm_Load;
        }

        private void UserEditForm_Load(object sender, EventArgs e)
        {
            // Устанавливаем значения по умолчанию
            if (cmbRole.Items.Count > 0) cmbRole.SelectedIndex = 2; // Кассир
            GeneratePassword();
        }

        public void LoadUserData(int id, string username, string fullName,
            string role, bool isActive)
        {
            txtId.Text = id.ToString();
            txtUsername.Text = username;
            txtFullName.Text = fullName;
            cmbRole.Text = role;
            chkActive.Checked = isActive;

            // Пароль не загружаем - должен быть сгенерирован заново
            txtPassword.Text = "";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Username = txtUsername.Text.Trim();
                Password = txtPassword.Text;
                FullName = txtFullName.Text.Trim();
                Role = cmbRole.Text;
                IsActive = chkActive.Checked;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Введите логин", "Ошибка");
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Введите пароль", "Ошибка");
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов", "Ошибка");
                txtPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbRole.Text))
            {
                MessageBox.Show("Выберите роль", "Ошибка");
                cmbRole.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnGeneratePassword_Click(object sender, EventArgs e)
        {
            GeneratePassword();
        }

        private void GeneratePassword()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
            var random = new Random();
            var password = new StringBuilder();

            // Генерируем пароль из 8 символов
            for (int i = 0; i < 8; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            txtPassword.Text = password.ToString();
        }

        private void TxtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Запрещаем пробелы в логине
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }
    }
}