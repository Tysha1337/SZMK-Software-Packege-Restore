using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SZMK.TeklaInteraction.Shared.BindingModels;

namespace SZMK.TeklaInteraction.Tekla2017.Views.Main
{
    public partial class ReportCheckDetails : Form
    {
        private List<OrderPathDetailsBindingModel> pathDetails;

        public ReportCheckDetails(List<OrderPathDetailsBindingModel> pathDetails)
        {
            this.pathDetails = pathDetails;

            InitializeComponent();
        }

        private void ReportCheckDetails_Load(object sender, EventArgs e)
        {
            Report_DGV.DataSource = pathDetails;
        }

        private void Report_DGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex >= 0)
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
        private void DGV_refresh()
        {
            Report_DGV.DataSource = null;
            Report_DGV.DataSource = pathDetails;
        }
    }
}
