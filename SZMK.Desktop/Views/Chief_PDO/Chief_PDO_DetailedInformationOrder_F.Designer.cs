namespace SZMK.Desktop.Views.Chief_PDO
{
    partial class Chief_PDO_DetailedInformationOrder_F
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Statuses_DGV = new System.Windows.Forms.DataGridView();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.OK_B = new System.Windows.Forms.Button();
            this.PathDetails_TB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ChangePathDetails_B = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Statuses_DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanel1.Controls.Add(this.Statuses_DGV, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.OK_B, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.PathDetails_TB, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ChangePathDetails_B, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(674, 417);
            this.tableLayoutPanel1.TabIndex = 1;
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
            this.tableLayoutPanel1.SetColumnSpan(this.Statuses_DGV, 2);
            this.Statuses_DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Statuses_DGV.Location = new System.Drawing.Point(5, 60);
            this.Statuses_DGV.Margin = new System.Windows.Forms.Padding(5);
            this.Statuses_DGV.Name = "Statuses_DGV";
            this.Statuses_DGV.ReadOnly = true;
            this.Statuses_DGV.RowHeadersVisible = false;
            this.Statuses_DGV.Size = new System.Drawing.Size(664, 217);
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
            this.User.HeaderText = "Пользователь";
            this.User.Name = "User";
            this.User.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(228)))), ((int)(((byte)(213)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(664, 35);
            this.label1.TabIndex = 1;
            this.label1.Text = "Список статусов";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OK_B
            // 
            this.OK_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.SetColumnSpan(this.OK_B, 2);
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
            // PathDetails_TB
            // 
            this.PathDetails_TB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PathDetails_TB.BackColor = System.Drawing.Color.White;
            this.PathDetails_TB.Location = new System.Drawing.Point(3, 344);
            this.PathDetails_TB.Name = "PathDetails_TB";
            this.PathDetails_TB.Size = new System.Drawing.Size(556, 20);
            this.PathDetails_TB.TabIndex = 59;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(228)))), ((int)(((byte)(213)))));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label2.Location = new System.Drawing.Point(5, 292);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(664, 35);
            this.label2.TabIndex = 60;
            this.label2.Text = "Путь до деталей";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChangePathDetails_B
            // 
            this.ChangePathDetails_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.ChangePathDetails_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChangePathDetails_B.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(223)))), ((int)(((byte)(253)))));
            this.ChangePathDetails_B.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.ChangePathDetails_B.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(237)))), ((int)(((byte)(253)))));
            this.ChangePathDetails_B.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangePathDetails_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.ChangePathDetails_B.Location = new System.Drawing.Point(567, 342);
            this.ChangePathDetails_B.Margin = new System.Windows.Forms.Padding(5);
            this.ChangePathDetails_B.Name = "ChangePathDetails_B";
            this.ChangePathDetails_B.Size = new System.Drawing.Size(102, 25);
            this.ChangePathDetails_B.TabIndex = 61;
            this.ChangePathDetails_B.Text = "Изменить";
            this.ChangePathDetails_B.UseVisualStyleBackColor = false;
            this.ChangePathDetails_B.Click += new System.EventHandler(this.ChangePathDetails_B_Click);
            // 
            // Chief_PDO_DetailedInformationOrder_F
            // 
            this.AcceptButton = this.OK_B;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.OK_B;
            this.ClientSize = new System.Drawing.Size(674, 417);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(690, 456);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(690, 456);
            this.Name = "Chief_PDO_DetailedInformationOrder_F";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подробная информация";
            this.Load += new System.EventHandler(this.Chief_PDO_DetailedInformationOrder_F_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Statuses_DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.DataGridView Statuses_DGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        public System.Windows.Forms.Button OK_B;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PathDetails_TB;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button ChangePathDetails_B;
    }
}