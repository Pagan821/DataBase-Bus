namespace База_Данных_Городских_Автобусов
{
    partial class MainDatabaseForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageRoutes;
        private System.Windows.Forms.TabPage tabPageBuses;
        private System.Windows.Forms.TabPage tabPageSchedule;
        private System.Windows.Forms.TabPage tabPageTickets;
        private System.Windows.Forms.TabPage tabPageUsers;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.DataGridView dataGridViewRoutes;
        private System.Windows.Forms.Panel panelRouteButtons;
        private System.Windows.Forms.Button btnRouteAdd;
        private System.Windows.Forms.Button btnRouteEdit;
        private System.Windows.Forms.Button btnRouteDelete;
        private System.Windows.Forms.Button btnRouteRefresh;
        private System.Windows.Forms.TextBox txtRouteSearch;
        private System.Windows.Forms.Button btnRouteSearch;
        private System.Windows.Forms.DataGridView dataGridViewBuses;
        private System.Windows.Forms.Panel panelBusButtons;
        private System.Windows.Forms.Button btnBusAdd;
        private System.Windows.Forms.Button btnBusEdit;
        private System.Windows.Forms.Button btnBusDelete;
        private System.Windows.Forms.Button btnBusRefresh;
        private System.Windows.Forms.TextBox txtBusSearch;
        private System.Windows.Forms.Button btnBusSearch;
        private System.Windows.Forms.DataGridView dataGridViewSchedule;
        private System.Windows.Forms.Panel panelScheduleButtons;
        private System.Windows.Forms.Button btnScheduleAdd;
        private System.Windows.Forms.Button btnScheduleEdit;
        private System.Windows.Forms.Button btnScheduleDelete;
        private System.Windows.Forms.Button btnScheduleRefresh;
        private System.Windows.Forms.TextBox txtScheduleSearch;
        private System.Windows.Forms.Button btnScheduleSearch;
        private System.Windows.Forms.DataGridView dataGridViewTickets;
        private System.Windows.Forms.Panel panelTicketButtons;
        private System.Windows.Forms.Button btnTicketAdd;
        private System.Windows.Forms.Button btnTicketEdit;
        private System.Windows.Forms.Button btnTicketDelete;
        private System.Windows.Forms.Button btnTicketRefresh;
        private System.Windows.Forms.TextBox txtTicketSearch;
        private System.Windows.Forms.Button btnTicketSearch;
        private System.Windows.Forms.DataGridView dataGridViewUsers;
        private System.Windows.Forms.Panel panelUserButtons;
        private System.Windows.Forms.Button btnUserAdd;
        private System.Windows.Forms.Button btnUserEdit;
        private System.Windows.Forms.Button btnUserDelete;
        private System.Windows.Forms.Button btnUserRefresh;
        private System.Windows.Forms.TextBox txtUserSearch;
        private System.Windows.Forms.Button btnUserSearch;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label labelStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageRoutes = new System.Windows.Forms.TabPage();
            this.dataGridViewRoutes = new System.Windows.Forms.DataGridView();
            this.panelRouteButtons = new System.Windows.Forms.Panel();
            this.btnRouteSearch = new System.Windows.Forms.Button();
            this.txtRouteSearch = new System.Windows.Forms.TextBox();
            this.btnRouteRefresh = new System.Windows.Forms.Button();
            this.btnRouteDelete = new System.Windows.Forms.Button();
            this.btnRouteEdit = new System.Windows.Forms.Button();
            this.btnRouteAdd = new System.Windows.Forms.Button();
            this.tabPageBuses = new System.Windows.Forms.TabPage();
            this.dataGridViewBuses = new System.Windows.Forms.DataGridView();
            this.panelBusButtons = new System.Windows.Forms.Panel();
            this.btnBusSearch = new System.Windows.Forms.Button();
            this.txtBusSearch = new System.Windows.Forms.TextBox();
            this.btnBusRefresh = new System.Windows.Forms.Button();
            this.btnBusDelete = new System.Windows.Forms.Button();
            this.btnBusEdit = new System.Windows.Forms.Button();
            this.btnBusAdd = new System.Windows.Forms.Button();
            this.tabPageSchedule = new System.Windows.Forms.TabPage();
            this.dataGridViewSchedule = new System.Windows.Forms.DataGridView();
            this.panelScheduleButtons = new System.Windows.Forms.Panel();
            this.btnScheduleSearch = new System.Windows.Forms.Button();
            this.txtScheduleSearch = new System.Windows.Forms.TextBox();
            this.btnScheduleRefresh = new System.Windows.Forms.Button();
            this.btnScheduleDelete = new System.Windows.Forms.Button();
            this.btnScheduleEdit = new System.Windows.Forms.Button();
            this.btnScheduleAdd = new System.Windows.Forms.Button();
            this.tabPageTickets = new System.Windows.Forms.TabPage();
            this.dataGridViewTickets = new System.Windows.Forms.DataGridView();
            this.panelTicketButtons = new System.Windows.Forms.Panel();
            this.btnTicketSearch = new System.Windows.Forms.Button();
            this.txtTicketSearch = new System.Windows.Forms.TextBox();
            this.btnTicketRefresh = new System.Windows.Forms.Button();
            this.btnTicketDelete = new System.Windows.Forms.Button();
            this.btnTicketEdit = new System.Windows.Forms.Button();
            this.btnTicketAdd = new System.Windows.Forms.Button();
            this.tabPageUsers = new System.Windows.Forms.TabPage();
            this.dataGridViewUsers = new System.Windows.Forms.DataGridView();
            this.panelUserButtons = new System.Windows.Forms.Panel();
            this.btnUserSearch = new System.Windows.Forms.Button();
            this.txtUserSearch = new System.Windows.Forms.TextBox();
            this.btnUserRefresh = new System.Windows.Forms.Button();
            this.btnUserDelete = new System.Windows.Forms.Button();
            this.btnUserEdit = new System.Windows.Forms.Button();
            this.btnUserAdd = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelStatus = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.tabControlMain.SuspendLayout();
            this.tabPageRoutes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRoutes)).BeginInit();
            this.panelRouteButtons.SuspendLayout();
            this.tabPageBuses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBuses)).BeginInit();
            this.panelBusButtons.SuspendLayout();
            this.tabPageSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSchedule)).BeginInit();
            this.panelScheduleButtons.SuspendLayout();
            this.tabPageTickets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTickets)).BeginInit();
            this.panelTicketButtons.SuspendLayout();
            this.tabPageUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsers)).BeginInit();
            this.panelUserButtons.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageRoutes);
            this.tabControlMain.Controls.Add(this.tabPageBuses);
            this.tabControlMain.Controls.Add(this.tabPageSchedule);
            this.tabControlMain.Controls.Add(this.tabPageTickets);
            this.tabControlMain.Controls.Add(this.tabPageUsers);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControlMain.ItemSize = new System.Drawing.Size(100, 25);
            this.tabControlMain.Location = new System.Drawing.Point(0, 50);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(900, 550);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageRoutes
            // 
            this.tabPageRoutes.Controls.Add(this.dataGridViewRoutes);
            this.tabPageRoutes.Controls.Add(this.panelRouteButtons);
            this.tabPageRoutes.Location = new System.Drawing.Point(4, 29);
            this.tabPageRoutes.Name = "tabPageRoutes";
            this.tabPageRoutes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRoutes.Size = new System.Drawing.Size(892, 517);
            this.tabPageRoutes.TabIndex = 0;
            this.tabPageRoutes.Text = "Маршруты";
            this.tabPageRoutes.UseVisualStyleBackColor = true;
            // 
            // dataGridViewRoutes
            // 
            this.dataGridViewRoutes.AllowUserToAddRows = false;
            this.dataGridViewRoutes.AllowUserToDeleteRows = false;
            this.dataGridViewRoutes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewRoutes.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewRoutes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRoutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRoutes.Location = new System.Drawing.Point(3, 45);
            this.dataGridViewRoutes.Name = "dataGridViewRoutes";
            this.dataGridViewRoutes.ReadOnly = true;
            this.dataGridViewRoutes.RowHeadersVisible = false;
            this.dataGridViewRoutes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewRoutes.Size = new System.Drawing.Size(886, 469);
            this.dataGridViewRoutes.TabIndex = 1;
            // 
            // panelRouteButtons
            // 
            this.panelRouteButtons.Controls.Add(this.btnRouteSearch);
            this.panelRouteButtons.Controls.Add(this.txtRouteSearch);
            this.panelRouteButtons.Controls.Add(this.btnRouteRefresh);
            this.panelRouteButtons.Controls.Add(this.btnRouteDelete);
            this.panelRouteButtons.Controls.Add(this.btnRouteEdit);
            this.panelRouteButtons.Controls.Add(this.btnRouteAdd);
            this.panelRouteButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRouteButtons.Location = new System.Drawing.Point(3, 3);
            this.panelRouteButtons.Name = "panelRouteButtons";
            this.panelRouteButtons.Size = new System.Drawing.Size(886, 42);
            this.panelRouteButtons.TabIndex = 0;
            // 
            // btnRouteSearch
            // 
            this.btnRouteSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRouteSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnRouteSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRouteSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRouteSearch.ForeColor = System.Drawing.Color.White;
            this.btnRouteSearch.Location = new System.Drawing.Point(775, 8);
            this.btnRouteSearch.Name = "btnRouteSearch";
            this.btnRouteSearch.Size = new System.Drawing.Size(100, 26);
            this.btnRouteSearch.TabIndex = 5;
            this.btnRouteSearch.Text = "Поиск";
            this.btnRouteSearch.UseVisualStyleBackColor = false;
            // 
            // txtRouteSearch
            // 
            this.txtRouteSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRouteSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtRouteSearch.Location = new System.Drawing.Point(569, 10);
            this.txtRouteSearch.Name = "txtRouteSearch";
            this.txtRouteSearch.Size = new System.Drawing.Size(200, 23);
            this.txtRouteSearch.TabIndex = 4;
            // 
            // btnRouteRefresh
            // 
            this.btnRouteRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnRouteRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRouteRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRouteRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRouteRefresh.Location = new System.Drawing.Point(250, 8);
            this.btnRouteRefresh.Name = "btnRouteRefresh";
            this.btnRouteRefresh.Size = new System.Drawing.Size(75, 26);
            this.btnRouteRefresh.TabIndex = 3;
            this.btnRouteRefresh.Text = "Обновить";
            this.btnRouteRefresh.UseVisualStyleBackColor = false;
            // 
            // btnRouteDelete
            // 
            this.btnRouteDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnRouteDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRouteDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRouteDelete.ForeColor = System.Drawing.Color.White;
            this.btnRouteDelete.Location = new System.Drawing.Point(169, 8);
            this.btnRouteDelete.Name = "btnRouteDelete";
            this.btnRouteDelete.Size = new System.Drawing.Size(75, 26);
            this.btnRouteDelete.TabIndex = 2;
            this.btnRouteDelete.Text = "Удалить";
            this.btnRouteDelete.UseVisualStyleBackColor = false;
            // 
            // btnRouteEdit
            // 
            this.btnRouteEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnRouteEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRouteEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRouteEdit.ForeColor = System.Drawing.Color.Black;
            this.btnRouteEdit.Location = new System.Drawing.Point(88, 8);
            this.btnRouteEdit.Name = "btnRouteEdit";
            this.btnRouteEdit.Size = new System.Drawing.Size(75, 26);
            this.btnRouteEdit.TabIndex = 1;
            this.btnRouteEdit.Text = "Изменить";
            this.btnRouteEdit.UseVisualStyleBackColor = false;
            // 
            // btnRouteAdd
            // 
            this.btnRouteAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnRouteAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRouteAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRouteAdd.ForeColor = System.Drawing.Color.White;
            this.btnRouteAdd.Location = new System.Drawing.Point(7, 8);
            this.btnRouteAdd.Name = "btnRouteAdd";
            this.btnRouteAdd.Size = new System.Drawing.Size(75, 26);
            this.btnRouteAdd.TabIndex = 0;
            this.btnRouteAdd.Text = "Добавить";
            this.btnRouteAdd.UseVisualStyleBackColor = false;
            // 
            // tabPageBuses
            // 
            this.tabPageBuses.Controls.Add(this.dataGridViewBuses);
            this.tabPageBuses.Controls.Add(this.panelBusButtons);
            this.tabPageBuses.Location = new System.Drawing.Point(4, 29);
            this.tabPageBuses.Name = "tabPageBuses";
            this.tabPageBuses.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBuses.Size = new System.Drawing.Size(892, 517);
            this.tabPageBuses.TabIndex = 1;
            this.tabPageBuses.Text = "Автобусы";
            this.tabPageBuses.UseVisualStyleBackColor = true;
            // 
            // dataGridViewBuses
            // 
            this.dataGridViewBuses.AllowUserToAddRows = false;
            this.dataGridViewBuses.AllowUserToDeleteRows = false;
            this.dataGridViewBuses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBuses.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewBuses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBuses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBuses.Location = new System.Drawing.Point(3, 45);
            this.dataGridViewBuses.Name = "dataGridViewBuses";
            this.dataGridViewBuses.ReadOnly = true;
            this.dataGridViewBuses.RowHeadersVisible = false;
            this.dataGridViewBuses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBuses.Size = new System.Drawing.Size(886, 469);
            this.dataGridViewBuses.TabIndex = 1;
            // 
            // panelBusButtons
            // 
            this.panelBusButtons.Controls.Add(this.btnBusSearch);
            this.panelBusButtons.Controls.Add(this.txtBusSearch);
            this.panelBusButtons.Controls.Add(this.btnBusRefresh);
            this.panelBusButtons.Controls.Add(this.btnBusDelete);
            this.panelBusButtons.Controls.Add(this.btnBusEdit);
            this.panelBusButtons.Controls.Add(this.btnBusAdd);
            this.panelBusButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBusButtons.Location = new System.Drawing.Point(3, 3);
            this.panelBusButtons.Name = "panelBusButtons";
            this.panelBusButtons.Size = new System.Drawing.Size(886, 42);
            this.panelBusButtons.TabIndex = 0;
            // 
            // btnBusSearch
            // 
            this.btnBusSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBusSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnBusSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBusSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBusSearch.ForeColor = System.Drawing.Color.White;
            this.btnBusSearch.Location = new System.Drawing.Point(775, 8);
            this.btnBusSearch.Name = "btnBusSearch";
            this.btnBusSearch.Size = new System.Drawing.Size(100, 26);
            this.btnBusSearch.TabIndex = 5;
            this.btnBusSearch.Text = "Поиск";
            this.btnBusSearch.UseVisualStyleBackColor = false;
            // 
            // txtBusSearch
            // 
            this.txtBusSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBusSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBusSearch.Location = new System.Drawing.Point(569, 10);
            this.txtBusSearch.Name = "txtBusSearch";
            this.txtBusSearch.Size = new System.Drawing.Size(200, 23);
            this.txtBusSearch.TabIndex = 4;
            // 
            // btnBusRefresh
            // 
            this.btnBusRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnBusRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBusRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBusRefresh.ForeColor = System.Drawing.Color.White;
            this.btnBusRefresh.Location = new System.Drawing.Point(250, 8);
            this.btnBusRefresh.Name = "btnBusRefresh";
            this.btnBusRefresh.Size = new System.Drawing.Size(75, 26);
            this.btnBusRefresh.TabIndex = 3;
            this.btnBusRefresh.Text = "Обновить";
            this.btnBusRefresh.UseVisualStyleBackColor = false;
            // 
            // btnBusDelete
            // 
            this.btnBusDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnBusDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBusDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBusDelete.ForeColor = System.Drawing.Color.White;
            this.btnBusDelete.Location = new System.Drawing.Point(169, 8);
            this.btnBusDelete.Name = "btnBusDelete";
            this.btnBusDelete.Size = new System.Drawing.Size(75, 26);
            this.btnBusDelete.TabIndex = 2;
            this.btnBusDelete.Text = "Удалить";
            this.btnBusDelete.UseVisualStyleBackColor = false;
            // 
            // btnBusEdit
            // 
            this.btnBusEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnBusEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBusEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBusEdit.ForeColor = System.Drawing.Color.Black;
            this.btnBusEdit.Location = new System.Drawing.Point(88, 8);
            this.btnBusEdit.Name = "btnBusEdit";
            this.btnBusEdit.Size = new System.Drawing.Size(75, 26);
            this.btnBusEdit.TabIndex = 1;
            this.btnBusEdit.Text = "Изменить";
            this.btnBusEdit.UseVisualStyleBackColor = false;
            // 
            // btnBusAdd
            // 
            this.btnBusAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnBusAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBusAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBusAdd.ForeColor = System.Drawing.Color.White;
            this.btnBusAdd.Location = new System.Drawing.Point(7, 8);
            this.btnBusAdd.Name = "btnBusAdd";
            this.btnBusAdd.Size = new System.Drawing.Size(75, 26);
            this.btnBusAdd.TabIndex = 0;
            this.btnBusAdd.Text = "Добавить";
            this.btnBusAdd.UseVisualStyleBackColor = false;
            // 
            // tabPageSchedule
            // 
            this.tabPageSchedule.Controls.Add(this.dataGridViewSchedule);
            this.tabPageSchedule.Controls.Add(this.panelScheduleButtons);
            this.tabPageSchedule.Location = new System.Drawing.Point(4, 29);
            this.tabPageSchedule.Name = "tabPageSchedule";
            this.tabPageSchedule.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSchedule.Size = new System.Drawing.Size(892, 517);
            this.tabPageSchedule.TabIndex = 2;
            this.tabPageSchedule.Text = "Расписание";
            this.tabPageSchedule.UseVisualStyleBackColor = true;
            // 
            // dataGridViewSchedule
            // 
            this.dataGridViewSchedule.AllowUserToAddRows = false;
            this.dataGridViewSchedule.AllowUserToDeleteRows = false;
            this.dataGridViewSchedule.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSchedule.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSchedule.Location = new System.Drawing.Point(3, 45);
            this.dataGridViewSchedule.Name = "dataGridViewSchedule";
            this.dataGridViewSchedule.ReadOnly = true;
            this.dataGridViewSchedule.RowHeadersVisible = false;
            this.dataGridViewSchedule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSchedule.Size = new System.Drawing.Size(886, 469);
            this.dataGridViewSchedule.TabIndex = 1;
            // 
            // panelScheduleButtons
            // 
            this.panelScheduleButtons.Controls.Add(this.btnScheduleSearch);
            this.panelScheduleButtons.Controls.Add(this.txtScheduleSearch);
            this.panelScheduleButtons.Controls.Add(this.btnScheduleRefresh);
            this.panelScheduleButtons.Controls.Add(this.btnScheduleDelete);
            this.panelScheduleButtons.Controls.Add(this.btnScheduleEdit);
            this.panelScheduleButtons.Controls.Add(this.btnScheduleAdd);
            this.panelScheduleButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelScheduleButtons.Location = new System.Drawing.Point(3, 3);
            this.panelScheduleButtons.Name = "panelScheduleButtons";
            this.panelScheduleButtons.Size = new System.Drawing.Size(886, 42);
            this.panelScheduleButtons.TabIndex = 0;
            // 
            // btnScheduleSearch
            // 
            this.btnScheduleSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScheduleSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnScheduleSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScheduleSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnScheduleSearch.ForeColor = System.Drawing.Color.White;
            this.btnScheduleSearch.Location = new System.Drawing.Point(775, 8);
            this.btnScheduleSearch.Name = "btnScheduleSearch";
            this.btnScheduleSearch.Size = new System.Drawing.Size(100, 26);
            this.btnScheduleSearch.TabIndex = 5;
            this.btnScheduleSearch.Text = "Поиск";
            this.btnScheduleSearch.UseVisualStyleBackColor = false;
            // 
            // txtScheduleSearch
            // 
            this.txtScheduleSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtScheduleSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtScheduleSearch.Location = new System.Drawing.Point(569, 10);
            this.txtScheduleSearch.Name = "txtScheduleSearch";
            this.txtScheduleSearch.Size = new System.Drawing.Size(200, 23);
            this.txtScheduleSearch.TabIndex = 4;
            // 
            // btnScheduleRefresh
            // 
            this.btnScheduleRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnScheduleRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScheduleRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnScheduleRefresh.ForeColor = System.Drawing.Color.White;
            this.btnScheduleRefresh.Location = new System.Drawing.Point(250, 8);
            this.btnScheduleRefresh.Name = "btnScheduleRefresh";
            this.btnScheduleRefresh.Size = new System.Drawing.Size(75, 26);
            this.btnScheduleRefresh.TabIndex = 3;
            this.btnScheduleRefresh.Text = "Обновить";
            this.btnScheduleRefresh.UseVisualStyleBackColor = false;
            // 
            // btnScheduleDelete
            // 
            this.btnScheduleDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnScheduleDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScheduleDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnScheduleDelete.ForeColor = System.Drawing.Color.White;
            this.btnScheduleDelete.Location = new System.Drawing.Point(169, 8);
            this.btnScheduleDelete.Name = "btnScheduleDelete";
            this.btnScheduleDelete.Size = new System.Drawing.Size(75, 26);
            this.btnScheduleDelete.TabIndex = 2;
            this.btnScheduleDelete.Text = "Удалить";
            this.btnScheduleDelete.UseVisualStyleBackColor = false;
            // 
            // btnScheduleEdit
            // 
            this.btnScheduleEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnScheduleEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScheduleEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnScheduleEdit.ForeColor = System.Drawing.Color.Black;
            this.btnScheduleEdit.Location = new System.Drawing.Point(88, 8);
            this.btnScheduleEdit.Name = "btnScheduleEdit";
            this.btnScheduleEdit.Size = new System.Drawing.Size(75, 26);
            this.btnScheduleEdit.TabIndex = 1;
            this.btnScheduleEdit.Text = "Изменить";
            this.btnScheduleEdit.UseVisualStyleBackColor = false;
            // 
            // btnScheduleAdd
            // 
            this.btnScheduleAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnScheduleAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScheduleAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnScheduleAdd.ForeColor = System.Drawing.Color.White;
            this.btnScheduleAdd.Location = new System.Drawing.Point(7, 8);
            this.btnScheduleAdd.Name = "btnScheduleAdd";
            this.btnScheduleAdd.Size = new System.Drawing.Size(75, 26);
            this.btnScheduleAdd.TabIndex = 0;
            this.btnScheduleAdd.Text = "Добавить";
            this.btnScheduleAdd.UseVisualStyleBackColor = false;
            // 
            // tabPageTickets
            // 
            this.tabPageTickets.Controls.Add(this.dataGridViewTickets);
            this.tabPageTickets.Controls.Add(this.panelTicketButtons);
            this.tabPageTickets.Location = new System.Drawing.Point(4, 29);
            this.tabPageTickets.Name = "tabPageTickets";
            this.tabPageTickets.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTickets.Size = new System.Drawing.Size(892, 517);
            this.tabPageTickets.TabIndex = 3;
            this.tabPageTickets.Text = "Билеты";
            this.tabPageTickets.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTickets
            // 
            this.dataGridViewTickets.AllowUserToAddRows = false;
            this.dataGridViewTickets.AllowUserToDeleteRows = false;
            this.dataGridViewTickets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTickets.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewTickets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTickets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTickets.Location = new System.Drawing.Point(3, 45);
            this.dataGridViewTickets.Name = "dataGridViewTickets";
            this.dataGridViewTickets.ReadOnly = true;
            this.dataGridViewTickets.RowHeadersVisible = false;
            this.dataGridViewTickets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTickets.Size = new System.Drawing.Size(886, 469);
            this.dataGridViewTickets.TabIndex = 1;
            // 
            // panelTicketButtons
            // 
            this.panelTicketButtons.Controls.Add(this.btnTicketSearch);
            this.panelTicketButtons.Controls.Add(this.txtTicketSearch);
            this.panelTicketButtons.Controls.Add(this.btnTicketRefresh);
            this.panelTicketButtons.Controls.Add(this.btnTicketDelete);
            this.panelTicketButtons.Controls.Add(this.btnTicketEdit);
            this.panelTicketButtons.Controls.Add(this.btnTicketAdd);
            this.panelTicketButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTicketButtons.Location = new System.Drawing.Point(3, 3);
            this.panelTicketButtons.Name = "panelTicketButtons";
            this.panelTicketButtons.Size = new System.Drawing.Size(886, 42);
            this.panelTicketButtons.TabIndex = 0;
            // 
            // btnTicketSearch
            // 
            this.btnTicketSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTicketSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnTicketSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTicketSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTicketSearch.ForeColor = System.Drawing.Color.White;
            this.btnTicketSearch.Location = new System.Drawing.Point(775, 8);
            this.btnTicketSearch.Name = "btnTicketSearch";
            this.btnTicketSearch.Size = new System.Drawing.Size(100, 26);
            this.btnTicketSearch.TabIndex = 5;
            this.btnTicketSearch.Text = "Поиск";
            this.btnTicketSearch.UseVisualStyleBackColor = false;
            // 
            // txtTicketSearch
            // 
            this.txtTicketSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTicketSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtTicketSearch.Location = new System.Drawing.Point(569, 10);
            this.txtTicketSearch.Name = "txtTicketSearch";
            this.txtTicketSearch.Size = new System.Drawing.Size(200, 23);
            this.txtTicketSearch.TabIndex = 4;
            // 
            // btnTicketRefresh
            // 
            this.btnTicketRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnTicketRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTicketRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTicketRefresh.ForeColor = System.Drawing.Color.White;
            this.btnTicketRefresh.Location = new System.Drawing.Point(250, 8);
            this.btnTicketRefresh.Name = "btnTicketRefresh";
            this.btnTicketRefresh.Size = new System.Drawing.Size(75, 26);
            this.btnTicketRefresh.TabIndex = 3;
            this.btnTicketRefresh.Text = "Обновить";
            this.btnTicketRefresh.UseVisualStyleBackColor = false;
            // 
            // btnTicketDelete
            // 
            this.btnTicketDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnTicketDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTicketDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTicketDelete.ForeColor = System.Drawing.Color.White;
            this.btnTicketDelete.Location = new System.Drawing.Point(169, 8);
            this.btnTicketDelete.Name = "btnTicketDelete";
            this.btnTicketDelete.Size = new System.Drawing.Size(75, 26);
            this.btnTicketDelete.TabIndex = 2;
            this.btnTicketDelete.Text = "Удалить";
            this.btnTicketDelete.UseVisualStyleBackColor = false;
            // 
            // btnTicketEdit
            // 
            this.btnTicketEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnTicketEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTicketEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTicketEdit.ForeColor = System.Drawing.Color.Black;
            this.btnTicketEdit.Location = new System.Drawing.Point(88, 8);
            this.btnTicketEdit.Name = "btnTicketEdit";
            this.btnTicketEdit.Size = new System.Drawing.Size(75, 26);
            this.btnTicketEdit.TabIndex = 1;
            this.btnTicketEdit.Text = "Изменить";
            this.btnTicketEdit.UseVisualStyleBackColor = false;
            // 
            // btnTicketAdd
            // 
            this.btnTicketAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnTicketAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTicketAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTicketAdd.ForeColor = System.Drawing.Color.White;
            this.btnTicketAdd.Location = new System.Drawing.Point(7, 8);
            this.btnTicketAdd.Name = "btnTicketAdd";
            this.btnTicketAdd.Size = new System.Drawing.Size(75, 26);
            this.btnTicketAdd.TabIndex = 0;
            this.btnTicketAdd.Text = "Добавить";
            this.btnTicketAdd.UseVisualStyleBackColor = false;
            // 
            // tabPageUsers
            // 
            this.tabPageUsers.Controls.Add(this.dataGridViewUsers);
            this.tabPageUsers.Controls.Add(this.panelUserButtons);
            this.tabPageUsers.Location = new System.Drawing.Point(4, 29);
            this.tabPageUsers.Name = "tabPageUsers";
            this.tabPageUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUsers.Size = new System.Drawing.Size(892, 517);
            this.tabPageUsers.TabIndex = 4;
            this.tabPageUsers.Text = "Пользователи";
            this.tabPageUsers.UseVisualStyleBackColor = true;
            // 
            // dataGridViewUsers
            // 
            this.dataGridViewUsers.AllowUserToAddRows = false;
            this.dataGridViewUsers.AllowUserToDeleteRows = false;
            this.dataGridViewUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewUsers.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewUsers.Location = new System.Drawing.Point(3, 45);
            this.dataGridViewUsers.Name = "dataGridViewUsers";
            this.dataGridViewUsers.ReadOnly = true;
            this.dataGridViewUsers.RowHeadersVisible = false;
            this.dataGridViewUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewUsers.Size = new System.Drawing.Size(886, 469);
            this.dataGridViewUsers.TabIndex = 1;
            // 
            // panelUserButtons
            // 
            this.panelUserButtons.Controls.Add(this.btnUserSearch);
            this.panelUserButtons.Controls.Add(this.txtUserSearch);
            this.panelUserButtons.Controls.Add(this.btnUserRefresh);
            this.panelUserButtons.Controls.Add(this.btnUserDelete);
            this.panelUserButtons.Controls.Add(this.btnUserEdit);
            this.panelUserButtons.Controls.Add(this.btnUserAdd);
            this.panelUserButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUserButtons.Location = new System.Drawing.Point(3, 3);
            this.panelUserButtons.Name = "panelUserButtons";
            this.panelUserButtons.Size = new System.Drawing.Size(886, 42);
            this.panelUserButtons.TabIndex = 0;
            // 
            // btnUserSearch
            // 
            this.btnUserSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUserSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnUserSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUserSearch.ForeColor = System.Drawing.Color.White;
            this.btnUserSearch.Location = new System.Drawing.Point(775, 8);
            this.btnUserSearch.Name = "btnUserSearch";
            this.btnUserSearch.Size = new System.Drawing.Size(100, 26);
            this.btnUserSearch.TabIndex = 5;
            this.btnUserSearch.Text = "Поиск";
            this.btnUserSearch.UseVisualStyleBackColor = false;
            // 
            // txtUserSearch
            // 
            this.txtUserSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtUserSearch.Location = new System.Drawing.Point(569, 10);
            this.txtUserSearch.Name = "txtUserSearch";
            this.txtUserSearch.Size = new System.Drawing.Size(200, 23);
            this.txtUserSearch.TabIndex = 4;
            // 
            // btnUserRefresh
            // 
            this.btnUserRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnUserRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUserRefresh.ForeColor = System.Drawing.Color.White;
            this.btnUserRefresh.Location = new System.Drawing.Point(250, 8);
            this.btnUserRefresh.Name = "btnUserRefresh";
            this.btnUserRefresh.Size = new System.Drawing.Size(75, 26);
            this.btnUserRefresh.TabIndex = 3;
            this.btnUserRefresh.Text = "Обновить";
            this.btnUserRefresh.UseVisualStyleBackColor = false;
            // 
            // btnUserDelete
            // 
            this.btnUserDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnUserDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUserDelete.ForeColor = System.Drawing.Color.White;
            this.btnUserDelete.Location = new System.Drawing.Point(169, 8);
            this.btnUserDelete.Name = "btnUserDelete";
            this.btnUserDelete.Size = new System.Drawing.Size(75, 26);
            this.btnUserDelete.TabIndex = 2;
            this.btnUserDelete.Text = "Удалить";
            this.btnUserDelete.UseVisualStyleBackColor = false;
            // 
            // btnUserEdit
            // 
            this.btnUserEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnUserEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUserEdit.ForeColor = System.Drawing.Color.Black;
            this.btnUserEdit.Location = new System.Drawing.Point(88, 8);
            this.btnUserEdit.Name = "btnUserEdit";
            this.btnUserEdit.Size = new System.Drawing.Size(75, 26);
            this.btnUserEdit.TabIndex = 1;
            this.btnUserEdit.Text = "Изменить";
            this.btnUserEdit.UseVisualStyleBackColor = false;
            // 
            // btnUserAdd
            // 
            this.btnUserAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnUserAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUserAdd.ForeColor = System.Drawing.Color.White;
            this.btnUserAdd.Location = new System.Drawing.Point(7, 8);
            this.btnUserAdd.Name = "btnUserAdd";
            this.btnUserAdd.Size = new System.Drawing.Size(75, 26);
            this.btnUserAdd.TabIndex = 0;
            this.btnUserAdd.Text = "Добавить";
            this.btnUserAdd.UseVisualStyleBackColor = false;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.panelHeader.Controls.Add(this.labelStatus);
            this.panelHeader.Controls.Add(this.btnBack);
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(900, 50);
            this.panelHeader.TabIndex = 1;
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStatus.ForeColor = System.Drawing.Color.White;
            this.labelStatus.Location = new System.Drawing.Point(500, 15);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(300, 20);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Загружено записей: 0";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(810, 10);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(80, 30);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = false;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(10, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(155, 25);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Управление БД";
            // 
            // MainDatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MinimumSize = new System.Drawing.Size(916, 639);
            this.Name = "MainDatabaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "База данных автобусных билетов";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageRoutes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRoutes)).EndInit();
            this.panelRouteButtons.ResumeLayout(false);
            this.panelRouteButtons.PerformLayout();
            this.tabPageBuses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBuses)).EndInit();
            this.panelBusButtons.ResumeLayout(false);
            this.panelBusButtons.PerformLayout();
            this.tabPageSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSchedule)).EndInit();
            this.panelScheduleButtons.ResumeLayout(false);
            this.panelScheduleButtons.PerformLayout();
            this.tabPageTickets.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTickets)).EndInit();
            this.panelTicketButtons.ResumeLayout(false);
            this.panelTicketButtons.PerformLayout();
            this.tabPageUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsers)).EndInit();
            this.panelUserButtons.ResumeLayout(false);
            this.panelUserButtons.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}