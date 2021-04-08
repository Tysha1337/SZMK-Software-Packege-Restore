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

namespace SZMK.TeklaInteraction.Tekla21_1.Views.Main
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
                    OrderPathDetailsBindingModel Paths = pathDetails.FirstOrDefault(p => p.Order == Report_DGV[0, e.RowIndex].Value.ToString());

                    if (Paths != null)
                    {
                        switch (Report_DGV[1, e.RowIndex].Value)
                        {
                            case "DWG":
                                if (Report_DGV[2, e.RowIndex].Value.ToString() == Paths.PathDWG)
                                {
                                    FolderBrowserDialog Fbd = new FolderBrowserDialog()
                                    {
                                        ShowNewFolderButton = false,
                                        Description = "Выберите папку с деталями"
                                    };

                                    if (Fbd.ShowDialog() == DialogResult.OK)
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

                                        Paths.PathDWG = ModelPath;
                                        Paths.FindedDWG = true;

                                        DGV_refresh();
                                    }
                                }
                                else
                                {
                                    if (Directory.Exists(Report_DGV[2, e.RowIndex].Value.ToString()))
                                    {
                                        string ModelPath = Report_DGV[2, e.RowIndex].Value.ToString();

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

                                        Paths.PathDWG = ModelPath;
                                        Paths.FindedDWG = true;

                                        DGV_refresh();
                                    }
                                    else
                                    {
                                        MessageBox.Show("По указанному пути папка не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                break;
                            case "PDF":
                                if (Report_DGV[2, e.RowIndex].Value.ToString() == Paths.PathPDF)
                                {
                                    FolderBrowserDialog Fbd = new FolderBrowserDialog()
                                    {
                                        ShowNewFolderButton = false,
                                        Description = "Выберите папку с деталями"
                                    };

                                    if (Fbd.ShowDialog() == DialogResult.OK)
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

                                        Paths.PathPDF = ModelPath;
                                        Paths.FindedPDF = true;

                                        DGV_refresh();
                                    }
                                }
                                else
                                {
                                    if (Directory.Exists(Report_DGV[2, e.RowIndex].Value.ToString()))
                                    {
                                        string ModelPath = Report_DGV[2, e.RowIndex].Value.ToString();

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

                                        Paths.PathPDF = ModelPath;
                                        Paths.FindedPDF = true;

                                        DGV_refresh();
                                    }
                                    else
                                    {
                                        MessageBox.Show("По указанному пути папка не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                break;
                            case "DXF":
                                if (Report_DGV[2, e.RowIndex].Value.ToString() == Paths.PathDXF)
                                {
                                    FolderBrowserDialog Fbd = new FolderBrowserDialog()
                                    {
                                        ShowNewFolderButton = false,
                                        Description = "Выберите папку с деталями"
                                    };

                                    if (Fbd.ShowDialog() == DialogResult.OK)
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

                                        Paths.PathDXF = ModelPath;
                                        Paths.FindedDXF = true;

                                        DGV_refresh();
                                    }
                                }
                                else
                                {
                                    if (Directory.Exists(Report_DGV[2, e.RowIndex].Value.ToString()))
                                    {
                                        string ModelPath = Report_DGV[2, e.RowIndex].Value.ToString();

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

                                        Paths.PathDXF = ModelPath;
                                        Paths.FindedDXF = true;

                                        DGV_refresh();
                                    }
                                    else
                                    {
                                        MessageBox.Show("По указанному пути папка не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("Ошибка изменения пути деталировки");
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
