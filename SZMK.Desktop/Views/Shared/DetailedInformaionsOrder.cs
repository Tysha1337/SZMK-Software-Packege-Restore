using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SZMK.Desktop.Models;

namespace SZMK.Desktop.Views.Shared
{
    public partial class DetailedInformaionsOrder : Form
    {
        private Order Order;
        private List<Order> HistoryOrders;

        public DetailedInformaionsOrder(Order Order)
        {
            this.Order = Order;
            HistoryOrders = new List<Order>();

            InitializeComponent();
        }

        private void DetailedInformaionsOrder_Load(object sender, EventArgs e)
        {
            try
            {
                List<StatusOfOrder> Statuses = SystemArgs.StatusOfOrders.Where(p => p.IDOrder == Order.ID).OrderBy(p => p.DateCreate).ToList();

                for (int i = 0; i < Statuses.Count; i++)
                {
                    Statuses_DGV.Rows.Add();
                    Statuses_DGV[0, i].Value = SystemArgs.Statuses.Where(p => p.ID == Statuses[i].IDStatus).Select(p => p.Name).Single();
                    Statuses_DGV[1, i].Value = Statuses[i].DateCreate;
                    Models.User TempUser = SystemArgs.Users.Where(p => p.ID == Statuses[i].IDUser).Single();
                    Statuses_DGV[2, i].Value = TempUser.Surname + " " + TempUser.Name.First() + "." + TempUser.MiddleName.First() + ".";
                }

                PathDetailsDWG_TB.Text = Order.PathDetails.PathDWG;
                PathDetailsPDF_TB.Text = Order.PathDetails.PathPDF;
                PathDetailsDXF_TB.Text = Order.PathDetails.PathDXF;
                PathModel_TB.Text = Order.Model.Path;
                PathArhive_TB.Text = Order.PathArhive.Path;

                RevisionDate_TB.Text = Order.Revision.DateCreate.ToShortDateString();
                RevisionCreatedBy_TB.Text = Order.Revision.CreatedBy;
                RevisionInfo_TB.Text = Order.Revision.Information;
                RevisionDiscription_TB.Text = Order.Revision.Description;
                RevisionLastApproved_TB.Text = Order.Revision.LastApptovedBy;
                Weight_TB.Text = Order.Weight.ToString();
                WeightDifferent_TB.Text = Order.WeightDifferent.ToString();

                HistoryOrders = SystemArgs.Request.GetHistoryOrders(Order.Number, Order.List.Split('и')[0]);

                History_DGV.AutoGenerateColumns = false;
                History_DGV.DataSource = HistoryOrders;
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangePathDetailsDWG_B_Click(object sender, EventArgs e)
        {
            try
            {
                if (Order.PathDetails.PathDWG == PathDetailsDWG_TB.Text)
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        PathDetails oldpath = Order.PathDetails;

                        string ModelPath = dialog.SelectedPath;

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

                        Order.PathDetails = new PathDetails { DateCreate = DateTime.Now, PathDWG = ModelPath, PathPDF = Order.PathDetails.PathPDF, PathDXF = Order.PathDetails.PathDXF };

                        SetPathDetails(oldpath);

                        PathDetailsDWG_TB.Text = ModelPath;

                        MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (!Directory.Exists(PathDetailsDWG_TB.Text))
                    {
                        throw new Exception("Не найдена папка");
                    }

                    PathDetails oldpath = Order.PathDetails;

                    string ModelPath = PathDetailsDWG_TB.Text;

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

                    Order.PathDetails = new PathDetails { DateCreate = DateTime.Now, PathDWG = ModelPath, PathPDF = Order.PathDetails.PathPDF, PathDXF = Order.PathDetails.PathDXF };

                    SetPathDetails(oldpath);

                    PathDetailsDWG_TB.Text = ModelPath;

                    MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangePathDetailsPDF_B_Click(object sender, EventArgs e)
        {
            try
            {
                if (Order.PathDetails.PathPDF == PathDetailsPDF_TB.Text)
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        PathDetails oldpath = Order.PathDetails;

                        string ModelPath = dialog.SelectedPath;

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

                        Order.PathDetails = new PathDetails { DateCreate = DateTime.Now, PathDWG = Order.PathDetails.PathDWG, PathPDF = ModelPath, PathDXF = Order.PathDetails.PathDXF };

                        SetPathDetails(oldpath);

                        PathDetailsPDF_TB.Text = ModelPath;

                        MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (!Directory.Exists(PathDetailsPDF_TB.Text))
                    {
                        throw new Exception("Не найдена папка");
                    }

                    PathDetails oldpath = Order.PathDetails;

                    string ModelPath = PathDetailsPDF_TB.Text;

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

                    Order.PathDetails = new PathDetails { DateCreate = DateTime.Now, PathDWG = Order.PathDetails.PathDWG, PathPDF = ModelPath, PathDXF = Order.PathDetails.PathDXF };

                    SetPathDetails(oldpath);

                    PathDetailsPDF_TB.Text = ModelPath;

                    MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangePathDetailsDXF_B_Click(object sender, EventArgs e)
        {
            try
            {
                if (Order.PathDetails.PathDXF == PathDetailsDXF_TB.Text)
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        PathDetails oldpath = Order.PathDetails;

                        string ModelPath = dialog.SelectedPath;

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

                        Order.PathDetails = new PathDetails { DateCreate = DateTime.Now, PathDWG = Order.PathDetails.PathDWG, PathPDF = Order.PathDetails.PathPDF, PathDXF = ModelPath };

                        SetPathDetails(oldpath);

                        PathDetailsDXF_TB.Text = ModelPath;

                        MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (!Directory.Exists(PathDetailsDXF_TB.Text))
                    {
                        throw new Exception("Не найдена папка");
                    }

                    PathDetails oldpath = Order.PathDetails;

                    string ModelPath = PathDetailsDXF_TB.Text;

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

                    Order.PathDetails = new PathDetails { DateCreate = DateTime.Now, PathDWG = Order.PathDetails.PathDWG, PathPDF = Order.PathDetails.PathPDF, PathDXF = ModelPath };

                    SetPathDetails(oldpath);

                    PathDetailsDXF_TB.Text = ModelPath;

                    MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangePathModel_B_Click(object sender, EventArgs e)
        {
            try
            {
                if (Order.Model.Path == PathModel_TB.Text)
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Model oldpath = Order.Model;

                        string ModelPath = dialog.SelectedPath;

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

                        Order.Model = new Model { DateCreate = DateTime.Now, Path = ModelPath };

                        SetPathModel(oldpath);

                        PathModel_TB.Text = ModelPath;

                        MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (!Directory.Exists(PathModel_TB.Text))
                    {
                        throw new Exception("Не найдена папка");
                    }

                    Model oldpath = Order.Model;

                    string ModelPath = PathModel_TB.Text;

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

                    Order.Model = new Model { DateCreate = DateTime.Now, Path = ModelPath };

                    SetPathModel(oldpath);

                    PathModel_TB.Text = ModelPath;

                    MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangePathArhive_B_Click(object sender, EventArgs e)
        {
            try
            {
                if (Order.PathArhive.Path == PathArhive_TB.Text)
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        PathArhive oldpath = Order.PathArhive;

                        string ModelPath = dialog.SelectedPath;

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

                        Order.PathArhive = new PathArhive { DateCreate = DateTime.Now, Path = ModelPath };

                        SetPathArhive(oldpath);

                        PathArhive_TB.Text = ModelPath;

                        MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (!Directory.Exists(PathArhive_TB.Text))
                    {
                        throw new Exception("Не найдена папка");
                    }

                    PathArhive oldpath = Order.PathArhive;

                    string ModelPath = PathArhive_TB.Text;

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

                    Order.PathArhive = new PathArhive { DateCreate = DateTime.Now, Path = ModelPath };

                    SetPathArhive(oldpath);

                    PathArhive_TB.Text = ModelPath;

                    MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PathDetailsDWG_TB_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Directory.Exists(Order.PathDetails.PathDWG))
                {
                    Process.Start("explorer", Order.PathDetails.PathDWG);
                }
                else
                {
                    throw new Exception("Не найдена папка");
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PathDetailsPDF_TB_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Directory.Exists(Order.PathDetails.PathPDF))
                {
                    Process.Start("explorer", Order.PathDetails.PathPDF);
                }
                else
                {
                    throw new Exception("Не найдена папка");
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PathDetailsDXF_TB_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Directory.Exists(Order.PathDetails.PathDXF))
                {
                    Process.Start("explorer", Order.PathDetails.PathDXF);
                }
                else
                {
                    throw new Exception("Не найдена папка");
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PathModel_TB_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Directory.Exists(Order.Model.Path))
                {
                    Process.Start("explorer", Order.Model.Path);
                }
                else
                {
                    throw new Exception("Не найдена папка");
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PathArhive_TB_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Directory.Exists(Order.PathArhive.Path))
                {
                    Process.Start("explorer", Order.PathArhive.Path);
                }
                else
                {
                    throw new Exception("Не найдена папка");
                }

            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetPathDetails(PathDetails oldpath)
        {
            try
            {
                if (!SystemArgs.Request.PathDetailsExist(Order.PathDetails))
                {
                    SystemArgs.Request.InsertPathDetails(Order.PathDetails);
                }

                Order.PathDetails = SystemArgs.Request.GetPathDetails(Order.PathDetails);

                SystemArgs.Request.SetPathDetailsAllOrder(Order);

                if (SystemArgs.Request.CheckedNeedRemovePathDetails(oldpath))
                {
                    SystemArgs.Request.DeletePathDetails(oldpath);
                }

                var updateOrders = SystemArgs.Orders.FindAll(p => p.Number == Order.Number);

                foreach (var order in updateOrders)
                {
                    order.PathDetails = Order.PathDetails;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private void SetPathModel(Model oldpath)
        {
            try
            {
                if (!SystemArgs.Request.ModelExist(Order.Model))
                {
                    SystemArgs.Request.InsertModel(Order.Model);
                }

                Order.Model = SystemArgs.Request.GetModel(Order.Model);

                SystemArgs.Request.SetModelAllOrder(Order);

                if (SystemArgs.Request.CheckedNeedRemoveModel(oldpath))
                {
                    SystemArgs.Request.DeleteModel(oldpath);
                }

                var updateOrders = SystemArgs.Orders.FindAll(p => p.Number == Order.Number);

                foreach (var order in updateOrders)
                {
                    order.Model = Order.Model;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private void SetPathArhive(PathArhive oldpath)
        {
            try
            {
                if (!SystemArgs.Request.PathArhiveExist(Order.PathArhive))
                {
                    SystemArgs.Request.InsertPathArhive(Order.PathArhive);
                }

                Order.PathArhive = SystemArgs.Request.GetPathArhive(Order.PathArhive);

                SystemArgs.Request.SetPathArhiveAllOrder(Order);

                if (SystemArgs.Request.CheckedNeedRemovePathArhive(oldpath))
                {
                    SystemArgs.Request.DeletePathArhive(oldpath);
                }

                var updateOrders = SystemArgs.Orders.FindAll(p => p.Number == Order.Number);

                foreach (var order in updateOrders)
                {
                    order.PathArhive = Order.PathArhive;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }

        private void History_DGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (History_DGV.CurrentCell != null && History_DGV.CurrentCell.RowIndex < HistoryOrders.Count() && e.RowIndex >= 0)
                {
                    Order Temp = (Order)HistoryOrders[History_DGV.CurrentCell.RowIndex];
                    DetailedInformaionsOrder Dialog = new DetailedInformaionsOrder(Temp);
                    Dialog.ShowDialog();
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
