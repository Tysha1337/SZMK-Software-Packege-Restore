
namespace SZMK.TeklaInteraction.Tekla2018i.Views.Main
{
    partial class ReportWarnings
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.Close_B = new System.Windows.Forms.Button();
            this.Report_DGV = new System.Windows.Forms.DataGridView();
            this.DataMatrix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Discription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Report_DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 389F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Close_B, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Report_DGV, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button1, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 403);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Yellow;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(790, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Отчет о несоответствии";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Close_B
            // 
            this.Close_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.Close_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Close_B.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(223)))), ((int)(((byte)(253)))));
            this.Close_B.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.Close_B.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(237)))), ((int)(((byte)(253)))));
            this.Close_B.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.Close_B.Location = new System.Drawing.Point(5, 364);
            this.Close_B.Margin = new System.Windows.Forms.Padding(5, 9, 5, 5);
            this.Close_B.Name = "Close_B";
            this.Close_B.Size = new System.Drawing.Size(401, 33);
            this.Close_B.TabIndex = 24;
            this.Close_B.Text = "Подтвердить";
            this.Close_B.UseVisualStyleBackColor = false;
            // 
            // Report_DGV
            // 
            this.Report_DGV.AllowUserToAddRows = false;
            this.Report_DGV.AllowUserToDeleteRows = false;
            this.Report_DGV.AllowUserToResizeColumns = false;
            this.Report_DGV.AllowUserToResizeRows = false;
            this.Report_DGV.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.Report_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Report_DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DataMatrix,
            this.Discription});
            this.tableLayoutPanel1.SetColumnSpan(this.Report_DGV, 2);
            this.Report_DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Report_DGV.Location = new System.Drawing.Point(5, 55);
            this.Report_DGV.Margin = new System.Windows.Forms.Padding(5, 0, 5, 1);
            this.Report_DGV.Name = "Report_DGV";
            this.Report_DGV.ReadOnly = true;
            this.Report_DGV.RowHeadersVisible = false;
            this.Report_DGV.Size = new System.Drawing.Size(790, 299);
            this.Report_DGV.TabIndex = 27;
            // 
            // DataMatrix
            // 
            this.DataMatrix.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DataMatrix.DataPropertyName = "InfoView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DataMatrix.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataMatrix.FillWeight = 60F;
            this.DataMatrix.HeaderText = "Данные";
            this.DataMatrix.Name = "DataMatrix";
            this.DataMatrix.ReadOnly = true;
            // 
            // Discription
            // 
            this.Discription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Discription.DataPropertyName = "Error";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Discription.DefaultCellStyle = dataGridViewCellStyle4;
            this.Discription.HeaderText = "Ошибка";
            this.Discription.Name = "Discription";
            this.Discription.ReadOnly = true;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(223)))), ((int)(((byte)(253)))));
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(237)))), ((int)(((byte)(253)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.button1.Location = new System.Drawing.Point(416, 364);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 9, 5, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(379, 33);
            this.button1.TabIndex = 28;
            this.button1.Text = "Отменить";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // ReportWarnings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 403);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportWarnings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Отчет о несоответствии";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Report_DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Close_B;
        public System.Windows.Forms.DataGridView Report_DGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataMatrix;
        private System.Windows.Forms.DataGridViewTextBoxColumn Discription;
        private System.Windows.Forms.Button button1;
    }
}