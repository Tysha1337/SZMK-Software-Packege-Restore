
namespace SZMK.Desktop.Views.Shared
{
    partial class DetailedInformaionsOrder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ListStatuses_TP = new System.Windows.Forms.TableLayoutPanel();
            this.OK_B = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Statuses_DGV = new System.Windows.Forms.DataGridView();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Locations_TP = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PathArhive_TB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ChangePathDetailsDWG_B = new System.Windows.Forms.Button();
            this.ChangePathDetailsPDF_B = new System.Windows.Forms.Button();
            this.ChangePathDetailsDXF_B = new System.Windows.Forms.Button();
            this.ChangePathModel_B = new System.Windows.Forms.Button();
            this.ChangePathArhive_B = new System.Windows.Forms.Button();
            this.PathDetailsPDF_TB = new System.Windows.Forms.TextBox();
            this.PathDetailsDWG_TB = new System.Windows.Forms.TextBox();
            this.PathDetailsDXF_TB = new System.Windows.Forms.TextBox();
            this.PathModel_TB = new System.Windows.Forms.TextBox();
            this.ListStatuses_TP.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Statuses_DGV)).BeginInit();
            this.Locations_TP.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListStatuses_TP
            // 
            this.ListStatuses_TP.BackColor = System.Drawing.Color.White;
            this.ListStatuses_TP.ColumnCount = 1;
            this.ListStatuses_TP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ListStatuses_TP.Controls.Add(this.OK_B, 0, 1);
            this.ListStatuses_TP.Controls.Add(this.tabControl1, 0, 0);
            this.ListStatuses_TP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListStatuses_TP.Location = new System.Drawing.Point(0, 0);
            this.ListStatuses_TP.Name = "ListStatuses_TP";
            this.ListStatuses_TP.RowCount = 2;
            this.ListStatuses_TP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ListStatuses_TP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.ListStatuses_TP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ListStatuses_TP.Size = new System.Drawing.Size(674, 417);
            this.ListStatuses_TP.TabIndex = 1;
            // 
            // OK_B
            // 
            this.OK_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.OK_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OK_B.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(223)))), ((int)(((byte)(253)))));
            this.OK_B.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.OK_B.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(237)))), ((int)(((byte)(253)))));
            this.OK_B.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OK_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.OK_B.Location = new System.Drawing.Point(5, 377);
            this.OK_B.Margin = new System.Windows.Forms.Padding(5);
            this.OK_B.Name = "OK_B";
            this.OK_B.Size = new System.Drawing.Size(664, 35);
            this.OK_B.TabIndex = 58;
            this.OK_B.Text = "ОК";
            this.OK_B.UseVisualStyleBackColor = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.Locations_TP);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(674, 372);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Statuses_DGV);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(666, 343);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Список статусов";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Statuses_DGV
            // 
            this.Statuses_DGV.AllowUserToAddRows = false;
            this.Statuses_DGV.AllowUserToDeleteRows = false;
            this.Statuses_DGV.AllowUserToResizeColumns = false;
            this.Statuses_DGV.AllowUserToResizeRows = false;
            this.Statuses_DGV.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.Statuses_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Statuses_DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Status,
            this.Date,
            this.User});
            this.Statuses_DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Statuses_DGV.Location = new System.Drawing.Point(0, 0);
            this.Statuses_DGV.Margin = new System.Windows.Forms.Padding(5);
            this.Statuses_DGV.Name = "Statuses_DGV";
            this.Statuses_DGV.ReadOnly = true;
            this.Statuses_DGV.RowHeadersVisible = false;
            this.Statuses_DGV.Size = new System.Drawing.Size(666, 343);
            this.Statuses_DGV.TabIndex = 11;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.FillWeight = 80F;
            this.Status.HeaderText = "Статус";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // Date
            // 
            this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Date.FillWeight = 50F;
            this.Date.HeaderText = "Дата";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // User
            // 
            this.User.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.User.FillWeight = 60F;
            this.User.HeaderText = "Пользователь";
            this.User.Name = "User";
            this.User.ReadOnly = true;
            // 
            // Locations_TP
            // 
            this.Locations_TP.BackColor = System.Drawing.Color.White;
            this.Locations_TP.Controls.Add(this.tableLayoutPanel1);
            this.Locations_TP.Location = new System.Drawing.Point(4, 25);
            this.Locations_TP.Margin = new System.Windows.Forms.Padding(0);
            this.Locations_TP.Name = "Locations_TP";
            this.Locations_TP.Size = new System.Drawing.Size(666, 343);
            this.Locations_TP.TabIndex = 1;
            this.Locations_TP.Text = "Местоположения";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tableLayoutPanel1.Controls.Add(this.PathArhive_TB, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.ChangePathDetailsDWG_B, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.ChangePathDetailsPDF_B, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.ChangePathDetailsDXF_B, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.ChangePathModel_B, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.ChangePathArhive_B, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.PathDetailsPDF_TB, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.PathDetailsDWG_TB, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.PathDetailsDXF_TB, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.PathModel_TB, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(666, 343);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // PathArhive_TB
            // 
            this.PathArhive_TB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PathArhive_TB.BackColor = System.Drawing.Color.White;
            this.PathArhive_TB.Location = new System.Drawing.Point(103, 151);
            this.PathArhive_TB.Name = "PathArhive_TB";
            this.PathArhive_TB.Size = new System.Drawing.Size(442, 22);
            this.PathArhive_TB.TabIndex = 71;
            this.PathArhive_TB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PathArhive_TB_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Детали DWG";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(4, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 35);
            this.label2.TabIndex = 1;
            this.label2.Text = "Детали PDF";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(4, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 35);
            this.label3.TabIndex = 2;
            this.label3.Text = "Детали DXF";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(4, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 35);
            this.label4.TabIndex = 3;
            this.label4.Text = "Модель";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(4, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 35);
            this.label5.TabIndex = 4;
            this.label5.Text = "Архив";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChangePathDetailsDWG_B
            // 
            this.ChangePathDetailsDWG_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.ChangePathDetailsDWG_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChangePathDetailsDWG_B.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(223)))), ((int)(((byte)(253)))));
            this.ChangePathDetailsDWG_B.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.ChangePathDetailsDWG_B.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(237)))), ((int)(((byte)(253)))));
            this.ChangePathDetailsDWG_B.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangePathDetailsDWG_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.ChangePathDetailsDWG_B.Location = new System.Drawing.Point(554, 6);
            this.ChangePathDetailsDWG_B.Margin = new System.Windows.Forms.Padding(5);
            this.ChangePathDetailsDWG_B.Name = "ChangePathDetailsDWG_B";
            this.ChangePathDetailsDWG_B.Size = new System.Drawing.Size(106, 25);
            this.ChangePathDetailsDWG_B.TabIndex = 62;
            this.ChangePathDetailsDWG_B.Text = "Изменить";
            this.ChangePathDetailsDWG_B.UseVisualStyleBackColor = false;
            this.ChangePathDetailsDWG_B.Click += new System.EventHandler(this.ChangePathDetailsDWG_B_Click);
            // 
            // ChangePathDetailsPDF_B
            // 
            this.ChangePathDetailsPDF_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.ChangePathDetailsPDF_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChangePathDetailsPDF_B.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(223)))), ((int)(((byte)(253)))));
            this.ChangePathDetailsPDF_B.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.ChangePathDetailsPDF_B.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(237)))), ((int)(((byte)(253)))));
            this.ChangePathDetailsPDF_B.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangePathDetailsPDF_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.ChangePathDetailsPDF_B.Location = new System.Drawing.Point(554, 42);
            this.ChangePathDetailsPDF_B.Margin = new System.Windows.Forms.Padding(5);
            this.ChangePathDetailsPDF_B.Name = "ChangePathDetailsPDF_B";
            this.ChangePathDetailsPDF_B.Size = new System.Drawing.Size(106, 25);
            this.ChangePathDetailsPDF_B.TabIndex = 63;
            this.ChangePathDetailsPDF_B.Text = "Изменить";
            this.ChangePathDetailsPDF_B.UseVisualStyleBackColor = false;
            this.ChangePathDetailsPDF_B.Click += new System.EventHandler(this.ChangePathDetailsPDF_B_Click);
            // 
            // ChangePathDetailsDXF_B
            // 
            this.ChangePathDetailsDXF_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.ChangePathDetailsDXF_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChangePathDetailsDXF_B.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(223)))), ((int)(((byte)(253)))));
            this.ChangePathDetailsDXF_B.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.ChangePathDetailsDXF_B.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(237)))), ((int)(((byte)(253)))));
            this.ChangePathDetailsDXF_B.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangePathDetailsDXF_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.ChangePathDetailsDXF_B.Location = new System.Drawing.Point(554, 78);
            this.ChangePathDetailsDXF_B.Margin = new System.Windows.Forms.Padding(5);
            this.ChangePathDetailsDXF_B.Name = "ChangePathDetailsDXF_B";
            this.ChangePathDetailsDXF_B.Size = new System.Drawing.Size(106, 25);
            this.ChangePathDetailsDXF_B.TabIndex = 64;
            this.ChangePathDetailsDXF_B.Text = "Изменить";
            this.ChangePathDetailsDXF_B.UseVisualStyleBackColor = false;
            this.ChangePathDetailsDXF_B.Click += new System.EventHandler(this.ChangePathDetailsDXF_B_Click);
            // 
            // ChangePathModel_B
            // 
            this.ChangePathModel_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.ChangePathModel_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChangePathModel_B.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(223)))), ((int)(((byte)(253)))));
            this.ChangePathModel_B.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.ChangePathModel_B.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(237)))), ((int)(((byte)(253)))));
            this.ChangePathModel_B.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangePathModel_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.ChangePathModel_B.Location = new System.Drawing.Point(554, 114);
            this.ChangePathModel_B.Margin = new System.Windows.Forms.Padding(5);
            this.ChangePathModel_B.Name = "ChangePathModel_B";
            this.ChangePathModel_B.Size = new System.Drawing.Size(106, 25);
            this.ChangePathModel_B.TabIndex = 65;
            this.ChangePathModel_B.Text = "Изменить";
            this.ChangePathModel_B.UseVisualStyleBackColor = false;
            this.ChangePathModel_B.Click += new System.EventHandler(this.ChangePathModel_B_Click);
            // 
            // ChangePathArhive_B
            // 
            this.ChangePathArhive_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.ChangePathArhive_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChangePathArhive_B.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(223)))), ((int)(((byte)(253)))));
            this.ChangePathArhive_B.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.ChangePathArhive_B.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(237)))), ((int)(((byte)(253)))));
            this.ChangePathArhive_B.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangePathArhive_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.ChangePathArhive_B.Location = new System.Drawing.Point(554, 150);
            this.ChangePathArhive_B.Margin = new System.Windows.Forms.Padding(5);
            this.ChangePathArhive_B.Name = "ChangePathArhive_B";
            this.ChangePathArhive_B.Size = new System.Drawing.Size(106, 25);
            this.ChangePathArhive_B.TabIndex = 66;
            this.ChangePathArhive_B.Text = "Изменить";
            this.ChangePathArhive_B.UseVisualStyleBackColor = false;
            this.ChangePathArhive_B.Click += new System.EventHandler(this.ChangePathArhive_B_Click);
            // 
            // PathDetailsPDF_TB
            // 
            this.PathDetailsPDF_TB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PathDetailsPDF_TB.BackColor = System.Drawing.Color.White;
            this.PathDetailsPDF_TB.Location = new System.Drawing.Point(103, 43);
            this.PathDetailsPDF_TB.Name = "PathDetailsPDF_TB";
            this.PathDetailsPDF_TB.Size = new System.Drawing.Size(442, 22);
            this.PathDetailsPDF_TB.TabIndex = 67;
            this.PathDetailsPDF_TB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PathDetailsPDF_TB_MouseDoubleClick);
            // 
            // PathDetailsDWG_TB
            // 
            this.PathDetailsDWG_TB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PathDetailsDWG_TB.BackColor = System.Drawing.Color.White;
            this.PathDetailsDWG_TB.Location = new System.Drawing.Point(103, 7);
            this.PathDetailsDWG_TB.Name = "PathDetailsDWG_TB";
            this.PathDetailsDWG_TB.Size = new System.Drawing.Size(442, 22);
            this.PathDetailsDWG_TB.TabIndex = 68;
            this.PathDetailsDWG_TB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PathDetailsDWG_TB_MouseDoubleClick);
            // 
            // PathDetailsDXF_TB
            // 
            this.PathDetailsDXF_TB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PathDetailsDXF_TB.BackColor = System.Drawing.Color.White;
            this.PathDetailsDXF_TB.Location = new System.Drawing.Point(103, 79);
            this.PathDetailsDXF_TB.Name = "PathDetailsDXF_TB";
            this.PathDetailsDXF_TB.Size = new System.Drawing.Size(442, 22);
            this.PathDetailsDXF_TB.TabIndex = 69;
            this.PathDetailsDXF_TB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PathDetailsDXF_TB_MouseDoubleClick);
            // 
            // PathModel_TB
            // 
            this.PathModel_TB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PathModel_TB.BackColor = System.Drawing.Color.White;
            this.PathModel_TB.Location = new System.Drawing.Point(103, 115);
            this.PathModel_TB.Name = "PathModel_TB";
            this.PathModel_TB.Size = new System.Drawing.Size(442, 22);
            this.PathModel_TB.TabIndex = 70;
            this.PathModel_TB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PathModel_TB_MouseDoubleClick);
            // 
            // DetailedInformaionsOrder
            // 
            this.AcceptButton = this.OK_B;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.OK_B;
            this.ClientSize = new System.Drawing.Size(674, 417);
            this.Controls.Add(this.ListStatuses_TP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(690, 456);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(690, 456);
            this.Name = "DetailedInformaionsOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подробная информация";
            this.Load += new System.EventHandler(this.DetailedInformaionsOrder_Load);
            this.ListStatuses_TP.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Statuses_DGV)).EndInit();
            this.Locations_TP.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ListStatuses_TP;
        public System.Windows.Forms.Button OK_B;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.DataGridView Statuses_DGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.TabPage Locations_TP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox PathArhive_TB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Button ChangePathDetailsDWG_B;
        public System.Windows.Forms.Button ChangePathDetailsPDF_B;
        public System.Windows.Forms.Button ChangePathDetailsDXF_B;
        public System.Windows.Forms.Button ChangePathModel_B;
        public System.Windows.Forms.Button ChangePathArhive_B;
        private System.Windows.Forms.TextBox PathDetailsPDF_TB;
        private System.Windows.Forms.TextBox PathDetailsDWG_TB;
        private System.Windows.Forms.TextBox PathDetailsDXF_TB;
        private System.Windows.Forms.TextBox PathModel_TB;
    }
}