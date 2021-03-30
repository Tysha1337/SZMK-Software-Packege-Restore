using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SZMK.TeklaInteraction.Shared.BindingModels;

namespace SZMK.TeklaInteraction.Tekla2018.Views.Main
{
    public partial class ReportCheckDetails : Form
    {
        private BindingList<OrderPathDetailsBindingModel> pathDetails;

        public ReportCheckDetails(List<OrderPathDetailsBindingModel> pathDetails)
        {
            this.pathDetails = new BindingList<OrderPathDetailsBindingModel>(pathDetails);

            InitializeComponent();
        }

        private void ReportCheckDetails_Load(object sender, EventArgs e)
        {
            DGV_refresh();
        }
        private void DGV_refresh()
        {
            Report_DGV.Rows.Clear();

            for (int i = 0; i < pathDetails.Count; i++)
            {
                Report_DGV.Rows.Add(pathDetails[i].Order, "DWG", pathDetails[i].PathDWG, pathDetails[i].FindedDWG);
                Report_DGV.Rows.Add(pathDetails[i].Order, "PDF", pathDetails[i].PathPDF, pathDetails[i].FindedPDF);
                Report_DGV.Rows.Add(pathDetails[i].Order, "DXF", pathDetails[i].PathDXF, pathDetails[i].FindedDXF);
            }
        }
        private void Report_DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
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
                        OrderPathDetailsBindingModel Paths = pathDetails.FirstOrDefault(p => p.Order == Report_DGV[0, e.RowIndex].Value.ToString());

                        if (Paths != null)
                        {
                            string ModelPath = Fbd.SelectedPath;

                            if (ModelPath.Substring(0, 2) != @"\\")
                            {
                                using (var managementObject = new ManagementObject())
                                {
                                    managementObject.Path = new ManagementPath($"Win32_LogicalDisk='{ModelPath.Substring(0, 2)}'");
                                    var driveType = (DriveType)(uint)managementObject["DriveType"];
                                    var networkPath = Convert.ToString(managementObject["ProviderName"]);

                                    ModelPath = networkPath + ModelPath.Remove(0, 2);
                                }
                            }

                            ModelPath = ModelPath.Replace("tekla-fs", "10.0.7.249");

                            switch (Report_DGV[1, e.RowIndex].Value)
                            {
                                case "DWG":
                                    Paths.PathDWG = ModelPath;
                                    Paths.FindedDWG = true;
                                    break;
                                case "PDF":
                                    Paths.PathPDF = ModelPath;
                                    Paths.FindedPDF = true;
                                    break;
                                case "DXF":
                                    Paths.PathDXF = ModelPath;
                                    Paths.FindedDXF = true;
                                    break;
                            }

                            DGV_refresh();
                        }
                        else
                        {
                            throw new Exception("Ошибка изменения пути деталировки");
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
