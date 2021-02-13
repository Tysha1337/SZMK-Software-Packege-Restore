﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SZMK.Desktop.BindingModels;

namespace SZMK.Desktop.Views.KB
{
    public partial class KB_ReportCheckDetail : Form
    {
        private BindingList<OrderPathDetailsBindingModel> pathDetails;
        public KB_ReportCheckDetail(List<OrderPathDetailsBindingModel> pathDetails)
        {
            this.pathDetails = new BindingList<OrderPathDetailsBindingModel>(pathDetails);

            InitializeComponent();
        }
        private void KB_ReportCheckDetail_Load(object sender, EventArgs e)
        {
            Report_DGV.AutoGenerateColumns = false;
            Report_DGV.DataSource = pathDetails;
        }
        private void DGV_refresh()
        {
            Report_DGV.DataSource = null;
            Report_DGV.DataSource = pathDetails;
        }

        private void Report_DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Report_DGV.Columns[e.ColumnIndex].Name == "Change" && e.RowIndex >= 0)
            {
                FolderBrowserDialog Fbd = new FolderBrowserDialog()
                {
                    ShowNewFolderButton = false,
                    Description = "Выберите папку с деталями"
                };

                if (Fbd.ShowDialog() == DialogResult.OK)
                {
                    pathDetails[e.RowIndex].Path = Fbd.SelectedPath;
                    pathDetails[e.RowIndex].Finded = true;

                    DGV_refresh();
                }
            }
        }
    }
}