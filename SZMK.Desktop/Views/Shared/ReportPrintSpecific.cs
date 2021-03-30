using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SZMK.Desktop.Models;

namespace SZMK.Desktop.Views.Shared
{
    public partial class ReportPrintSpecific : Form
    {
        private List<Specific> specifics;

        public ReportPrintSpecific(List<Specific> specifics)
        {
            this.specifics = specifics;

            InitializeComponent();
        }

        private void Print_B_Click(object sender, EventArgs e)
        {
            try
            {
                if(File.Exists(Application.StartupPath + @"\TempPrint\combineselecteddetails.pdf"))
                {
                    Process.Start(Application.StartupPath + @"\TempPrint\combineselecteddetails.pdf");
                }
                else
                {
                    MessageBox.Show("Не найден файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception Ex)
            {
                SystemArgs.PrintLog(Ex.ToString());
                MessageBox.Show(Ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReportPrintSpecific_Load(object sender, EventArgs e)
        {
            try
            {
                Report_DGV.AutoGenerateColumns = false;
                Report_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Report_DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                var SpecificsOrderby = specifics.OrderBy(p => p.Number).ThenBy(p => p.NumberSpecific).ToList();

                for (int i = 0; i < SpecificsOrderby.Count(); i++)
                {
                    Report_DGV.Rows.Add();
                    Report_DGV[0, Report_DGV.Rows.Count - 1].Value = SpecificsOrderby[i].Number;
                    Report_DGV[1, Report_DGV.Rows.Count - 1].Value = SpecificsOrderby[i].PathDetails;
                    Report_DGV[2, Report_DGV.Rows.Count - 1].Value = SpecificsOrderby[i].NumberSpecific;
                    Report_DGV[3, Report_DGV.Rows.Count - 1].Value = SpecificsOrderby[i].Count;
                    if (SpecificsOrderby[i].Finded)
                    {
                        Report_DGV[4, Report_DGV.Rows.Count - 1].Style.BackColor = Color.Lime;
                        Report_DGV[4, Report_DGV.Rows.Count - 1].Value = "Найдено";
                    }
                    else
                    {
                        Report_DGV[4, Report_DGV.Rows.Count - 1].Style.BackColor = Color.Red;
                        Report_DGV[4, Report_DGV.Rows.Count - 1].Value = "Не найдено";
                    }
                }

                if (specifics.FindAll(p => p.Finded).Count() == 0)
                {
                    Print_B.Enabled = false;
                }
            }
            catch (Exception Ex)
            {
                SystemArgs.PrintLog(Ex.ToString());
                MessageBox.Show(Ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
