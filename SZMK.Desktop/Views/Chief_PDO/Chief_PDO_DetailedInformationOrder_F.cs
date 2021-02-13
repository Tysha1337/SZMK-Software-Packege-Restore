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
using SZMK.Desktop.Models;

namespace SZMK.Desktop.Views.Chief_PDO
{
    public partial class Chief_PDO_DetailedInformationOrder_F : Form
    {
        private Order Order;

        public Chief_PDO_DetailedInformationOrder_F(Order Order)
        {
            this.Order = Order;

            InitializeComponent();
        }

        private void Chief_PDO_DetailedInformationOrder_F_Load(object sender, EventArgs e)
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

                PathDetails_TB.Text = Order.PathDetails.Path;
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangePathDetails_B_Click(object sender, EventArgs e)
        {
            try
            {
                if (Order.PathDetails.Path == PathDetails_TB.Text)
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

                        Order.PathDetails = new PathDetails { DateCreate = DateTime.Now, Path = ModelPath };

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

                        PathDetails_TB.Text = ModelPath;

                        MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    PathDetails oldpath = Order.PathDetails;

                    string ModelPath = PathDetails_TB.Text;

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

                    Order.PathDetails = new PathDetails { DateCreate = DateTime.Now, Path = ModelPath };

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

                    MessageBox.Show("Путь успешно изменен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
