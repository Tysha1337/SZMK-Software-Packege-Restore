﻿using System;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Data;
using Equin.ApplicationFramework;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using SZMK.Desktop.Models;
using SZMK.Desktop.Views.Shared;
using SZMK.Desktop.Services.Scan;
using SZMK.Desktop.Services.Setting;
using SZMK.Desktop.Services;
using SZMK.Desktop.Views.Chief_PDO;
using SZMK.Desktop.Views.Admin;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SZMK.Desktop.BindingModels;
using SZMK.Desktop.Services.DataGridView;
using SZMK.Desktop.Services.DataGridView.Sort;

namespace SZMK.Desktop.Views.Arhive
{
    public partial class AR_Arhive_F : Form
    {
        public AR_Arhive_F()
        {
            InitializeComponent();
        }

        SortableBindingList<Order> View;

        private bool UsedSearch = false;

        private void ARArhive_F_Load(object sender, EventArgs e)
        {
            try
            {
                Order_DGV.AutoGenerateColumns = false;
                Order_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Order_DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                SetDoubleBuffered(Order_DGV);

                View = new SortableBindingList<Order>(new List<Order>());

                SystemArgs.ByteScout = new ByteScout();
                SystemArgs.MobileApplication = new MobileApplication();
                SystemArgs.Orders = new List<Order>();
                SystemArgs.BlankOrders = new List<BlankOrder>();
                SystemArgs.BlankOrderOfOrders = new List<BlankOrderOfOrder>();
                SystemArgs.StatusOfOrders = new List<StatusOfOrder>();
                SystemArgs.Excel = new Excel();
                SystemArgs.Template = new Template();
                SystemArgs.SelectedColumn = new SelectedColumn();
                SystemArgs.ServerMail = new ServerMail();
                SystemArgs.UnLoadSpecific = new UnLoadSpecific();

                ItemsFilter();
                SelectedColumnDGV();
                FilterCB_TSB.SelectedIndex = 0;
            }
            catch (Exception E)
            {
                if (MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }
        private void AddOrder_TSB_Click(object sender, EventArgs e)
        {
            LockedButtonForLoadData(false);

            if (AddOrder())
            {
                RefreshOrderAsync(FilterCB_TSB.SelectedIndex);
            }

            LockedButtonForLoadData(true);
        }

        private void ChangeOrder_TSB_Click(object sender, EventArgs e)
        {
            LockedButtonForLoadData(false);

            if (ChangeOrder())
            {
                RefreshOrderAsync(FilterCB_TSB.SelectedIndex);
            }

            LockedButtonForLoadData(true);
        }

        private void AddOrder_TSM_Click(object sender, EventArgs e)
        {
            LockedButtonForLoadData(false);

            if (AddOrder())
            {
                RefreshOrderAsync(FilterCB_TSB.SelectedIndex);
            }

            LockedButtonForLoadData(true);
        }

        private void ChangeOrder_TSM_Click(object sender, EventArgs e)
        {
            LockedButtonForLoadData(false);

            if (ChangeOrder())
            {
                RefreshOrderAsync(FilterCB_TSB.SelectedIndex);
            }

            LockedButtonForLoadData(true);
        }

        private void ReportDate_TSM_Click(object sender, EventArgs e)
        {
            ReportOrderOfDate();
        }

        private void Search_TSB_Click(object sender, EventArgs e)
        {
            if (Search())
            {
                Display(SystemArgs.Orders);
            }
        }

        private void Reset_TSB_Click(object sender, EventArgs e)
        {
            ResetSearch();
        }

        private void AdvancedSearch_TSB_Click(object sender, EventArgs e)
        {
            SearchParamAsync();
        }

        private Boolean AddOrder()
        {
            try
            {
                AR_Decode_F Dialog = new AR_Decode_F();

                if (SystemArgs.ByteScout.CheckConnect())
                {
                    Dialog.ServerStatus_TB.Text = "Подключено";
                    Dialog.ServerStatus_TB.BackColor = Color.FromArgb(233, 245, 255);
                    Dialog.Status_TB.AppendText($"Ожидание распознования" + Environment.NewLine);

                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        AR_DecodeReport_F Report = new AR_DecodeReport_F();
                        Report.Report_DGV.RowCount = 0;

                        for (int i = 0; i < SystemArgs.ByteScout.GetDecodeSession().Count; i++)
                        {
                            if (SystemArgs.ByteScout[i].Unique == 1)
                            {
                                Int64 PositionID = SystemArgs.User.GetPosition().ID;

                                Status TempStatus = (from p in SystemArgs.Statuses
                                                     where p.IDPosition == PositionID
                                                     select p).Single();
                                String[] ValidationDataMatrix = SystemArgs.ByteScout[i].DataMatrix.Split('_');

                                if (SystemArgs.Request.InsertStatus(ValidationDataMatrix[0], ValidationDataMatrix[1], TempStatus.ID, SystemArgs.User))
                                {
                                    Report.Report_DGV.RowCount++;
                                    Report.Report_DGV[0, Report.Report_DGV.Rows.Count - 1].Value = SystemArgs.ByteScout[i].DataMatrix;
                                    Report.Report_DGV[1, Report.Report_DGV.Rows.Count - 1].Value = CopyFileToArhive(SystemArgs.ByteScout[i].DataMatrix, Dialog.FileNames[i]);
                                }
                                else
                                {
                                    MessageBox.Show("Ошибка при добавлении в базу данных статуса для: " + SystemArgs.ByteScout[i].DataMatrix, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }

                                SystemArgs.UnLoadSpecific.ChekedUnloading(SystemArgs.ByteScout[i].DataMatrix.Split('_')[0], SystemArgs.ByteScout[i].DataMatrix.Split('_')[1], SystemArgs.ByteScout[i].DataMatrix.Split('_')[3]);
                            }
                        }

                        if (SystemArgs.UnLoadSpecific.ExecutorMails.Count != 0)
                        {
                            SystemArgs.ServerMail.SendMail(true, SystemArgs.User.StatusesUser[0].Name);
                        }

                        SystemArgs.UnLoadSpecific.ExecutorMails.Clear();

                        if (SystemArgs.ByteScout.GetDecodeSession().Where(p => p.Unique == 1).Count() != 0)
                        {
                            Report.ShowDialog();
                        }
                        else
                        {
                            Report.Dispose();
                        }
                    }

                    SystemArgs.ByteScout.ClearData();
                    return true;
                }
                else
                {
                    throw new Exception("Ошибка при подключении к серверу распознавнаия");
                }
            }
            catch (Exception E)
            {
                SystemArgs.UnLoadSpecific.ExecutorMails.Clear();
                SystemArgs.ByteScout.ClearData();
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

        }
        private String CopyFileToArhive(String DataMatrix, String FileName)
        {
            try
            {
                String[] Temp = DataMatrix.Split('_');
                String TextReport = "";
                if (Directory.Exists($@"{SystemArgs.SettingsUser.ArchivePath}\{Temp[0]}"))
                {
                    if (!File.Exists($@"{SystemArgs.SettingsUser.ArchivePath}\{Temp[0]}\{DataMatrix}.tiff"))
                    {
                        File.Copy(FileName, $@"{SystemArgs.SettingsUser.ArchivePath}\{Temp[0]}\{DataMatrix}.tiff");
                        TextReport = $"Файл {DataMatrix}.tiff помещен в директорию {Temp[0]}" + Environment.NewLine;
                        try
                        {
                            File.Delete(FileName);
                        }
                        catch
                        {
                            MessageBox.Show("Ошибка доступа к файлу по пути " + FileName, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    }
                    else
                    {
                        TextReport = $"Файл {DataMatrix}.tiff уже сущетвует в директории {Temp[0]}" + Environment.NewLine;
                    }
                }
                else
                {
                    Directory.CreateDirectory($@"{SystemArgs.SettingsUser.ArchivePath}\{Temp[0]}");

                    if (!File.Exists($@"{SystemArgs.SettingsUser.ArchivePath}\{Temp[0]}\{DataMatrix}.tiff"))
                    {
                        File.Copy(FileName, $@"{SystemArgs.SettingsUser.ArchivePath}\{Temp[0]}\{DataMatrix}.tiff");
                        TextReport = $"Директория {Temp[0]} создана. Файл {DataMatrix}.tiff помещен в директорию" + Environment.NewLine;
                        try
                        {
                            File.Delete(FileName);
                        }
                        catch
                        {
                            MessageBox.Show("Ошибка доступа к файлу по пути " + FileName, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                    else
                    {
                        TextReport = $"Файл {DataMatrix}.tiff уже сущетвует в директории {Temp[0]}" + Environment.NewLine;
                    }
                }
                return TextReport;
            }
            catch
            {
                throw new Exception("Ошибка перемещения файлов по директории");
            }
        }
        private bool ChangeOrder()
        {
            try
            {
                if (Order_DGV.CurrentCell != null && Order_DGV.CurrentCell.RowIndex >= 0)
                {
                    AR_ChangeOrder_F Dialog = new AR_ChangeOrder_F();
                    if (Order_DGV.SelectedRows.Count > 1)
                    {
                        Dialog.Comment_TB.Enabled = false;
                        Dialog.Hide_CB.Enabled = false;
                    }
                    else
                    {
                        Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];
                        if (Temp.Comment != null)
                        {
                            Dialog.Comment_TB.Text = Temp.Comment.ToString();
                        }
                        Dialog.Hide_CB.Checked = Temp.Hide;
                        Dialog.Comment_TB.Enabled = false;
                        Dialog.Hide_CB.Enabled = false;
                    }
                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        for (int i = 0; i < Order_DGV.SelectedRows.Count; i++)
                        {
                            Order ChangedOrder = (Order)(View[Order_DGV.SelectedRows[i].Index]);
                            if (Dialog.Comment_CB.Checked)
                            {
                                ChangedOrder.Comment = new Comment { DateCreate = DateTime.Now, User = SystemArgs.User, Text = Dialog.Comment_TB.Text.Trim() };

                                if (!SystemArgs.Request.CommentExist(ChangedOrder.Comment))
                                {
                                    SystemArgs.Request.InsertComment(ChangedOrder.Comment);
                                }

                                ChangedOrder.Comment = SystemArgs.Request.GetComment(ChangedOrder.Comment);

                                SystemArgs.Request.SetCommentOrder(ChangedOrder);
                            }
                            if (Dialog.HideRewrite_CB.Checked)
                            {
                                ChangedOrder.Hide = Dialog.Hide_CB.Checked;

                                if (ChangedOrder.Hide)
                                {
                                    if (ChangedOrder.Comment != null)
                                    {
                                        if (!SystemArgs.Request.UpdateOrder(ChangedOrder))
                                        {
                                            throw new Exception($"Ошибка обновления данных чертежа\n\rЗаказ:{ChangedOrder.Number} Лист:{ChangedOrder.List}");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception($"Не заполнен комментарий чертежа\n\rЗаказ:{ChangedOrder.Number} Лист:{ChangedOrder.List}");
                                    }
                                }
                                else
                                {
                                    if (!SystemArgs.Request.UpdateOrder(ChangedOrder))
                                    {
                                        throw new Exception($"Ошибка обновления данных чертежа\n\rЗаказ:{ChangedOrder.Number} Лист:{ChangedOrder.List}");
                                    }
                                }
                            }
                        }
                        MessageBox.Show("Чертеж(ы) успешно изменены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private void Display(List<Order> List)
        {
            try
            {
                Order_DGV.Invoke((MethodInvoker)delegate ()
                {
                    int Index = -1;

                    Index = FilterCB_TSB.SelectedIndex;

                    if (Index >= 0)
                    {
                        View = null;
                        View = new SortableBindingList<Order>(List);

                        Order_DGV.DataSource = View;

                        if (View.Count > 0)
                        {
                            CountOrder_TB.Text = View.Count.ToString();
                            if (Index == 0)
                            {
                                VisibleButton(true);
                            }
                        }
                        else
                        {
                            CountOrder_TB.Text = "0";
                            SelectedOrder_TB.Text = "0";
                            if (Index == 0)
                            {
                                VisibleButton(false);
                            }
                        }
                        if (!UsedSearch)
                        {
                            if (Index == 0)
                            {
                                ForgetOrder();
                            }
                            else
                            {
                                VisibleButton(false);
                            }
                        }
                    }
                });
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.ToString());
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ForgetOrder()
        {
            for (int i = 0; i < Order_DGV.RowCount; i++)
            {
                if ((DateTime.Now - Convert.ToDateTime(Order_DGV["StatusDate", i].Value)).TotalDays >= SystemArgs.SettingsProgram.VisualRow_N2)
                {
                    Order_DGV.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(236, 0, 6);
                }
                else if ((DateTime.Now - Convert.ToDateTime(Order_DGV["StatusDate", i].Value)).TotalDays >= SystemArgs.SettingsProgram.VisualRow_N1)
                {
                    Order_DGV.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                }
            }
        }

        private void Order_DGV_SelectionChanged(object sender, EventArgs e)
        {
            if (Order_DGV.CurrentCell != null && Order_DGV.CurrentCell.RowIndex < View.Count())
            {
                Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];

                Selection(Temp, true);
            }
            else
            {
                Selection(null, false);
            }
        }

        private void ARArhive_F_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < SystemArgs.SelectedColumn.GetColumns().Count; i++)
            {
                if (Order_DGV.Columns[i].Visible)
                {
                    SystemArgs.SelectedColumn[i].DisplayIndex = Order_DGV.Columns[SystemArgs.SelectedColumn[i].Name].DisplayIndex;
                    SystemArgs.SelectedColumn[i].FillWeight = Order_DGV.Columns[SystemArgs.SelectedColumn[i].Name].FillWeight;
                }
            }
            SystemArgs.SelectedColumn.SetParametrColumnDisplayIndex();
            SystemArgs.SelectedColumn.SetParametrColumnFillWeight();
            Application.Exit();
        }

        private void Order_DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionBackColor = Color.FromArgb(112, 238, 226);
            e.CellStyle.SelectionForeColor = Color.Black;
        }

        private void FilterCB_TSB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetSearch();
        }

        private void ItemsFilter()
        {
            FilterCB_TSB.Items.Add("Текущий статус");
            FilterCB_TSB.Items.Add("Все статусы");
            FilterCB_TSB.Items.Add("Аннулированные");
            FilterCB_TSB.Items.Add("Завершенные");
        }

        private List<Order> ResultSearch(String TextSearch)
        {
            List<Order> Result = new List<Order>();

            if (!String.IsNullOrEmpty(TextSearch))
            {
                foreach (Order Temp in SystemArgs.Orders)
                {
                    if (Temp.SearchString().IndexOf(TextSearch) != -1)
                    {
                        Result.Add(Temp);
                    }
                }
            }

            SystemArgs.PrintLog("Перебор значений по заданным параметрам успешно завершен");

            return Result;
        }

        private bool Search()
        {
            try
            {
                if (!String.IsNullOrEmpty(Search_TSTB.Text))
                {
                    String SearchText = Search_TSTB.Text.Trim();

                    SystemArgs.Orders = ResultSearch(SearchText);

                    if (SystemArgs.Orders.Count <= 0)
                    {
                        Search_TSTB.Focus();
                        MessageBox.Show("Поиск не дал результатов", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        SystemArgs.PrintLog("Количество объектов по параметрам поиска 0");
                        return false;
                    }

                    return true;
                }
                else
                {
                    ResetSearch();
                    SystemArgs.PrintLog("Получено пустое значение параметра поиска");
                    return false;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void ResetSearch()
        {
            UsedSearch = false;

            if (SystemArgs.Orders != null)
            {
                SystemArgs.Orders.Clear();
            }

            Search_TSTB.Text = String.Empty;

            RefreshOrderAsync(FilterCB_TSB.SelectedIndex);
        }
        private void GetDataForSearch(ForLongOperations_F Load, bool Finished, bool Hide)
        {
            try
            {
                SystemArgs.Orders.Clear();
                SystemArgs.BlankOrders.Clear();
                SystemArgs.StatusOfOrders.Clear();
                SystemArgs.BlankOrderOfOrders.Clear();

                SystemArgs.RequestLinq.GetOrdersForSearch(Load, Finished, Hide);
            }
            catch (Exception Ex)
            {
                SystemArgs.PrintLog(Ex.ToString());
                throw new Exception(Ex.Message, Ex);
            }
        }

        private async void SearchParamAsync()
        {
            try
            {
                AR_SearchParam_F Dialog = new AR_SearchParam_F();

                List<Status> Statuses = new List<Status>
                {
                    new Status(-1, 0, "Не задан")
                };
                Statuses.AddRange(SystemArgs.Statuses);
                Dialog.Status_CB.DataSource = Statuses;

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    bool Finished = true;
                    bool Hide = Dialog.Hide_CB.Checked;

                    if (Dialog.Finished_CB.Checked && Dialog.Number_TB.Text == String.Empty && Dialog.List_TB.Text == String.Empty)
                    {
                        if (MessageBox.Show("Вы уверены в выводе всех завершенных чертежей?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                        {
                            Finished = false;
                        }
                    }
                    else
                    {
                        if (!Dialog.Finished_CB.Checked)
                        {
                            Finished = false;
                        }
                    }

                    ForLongOperations_F Load = new ForLongOperations_F();
                    Load.Show();

                    LockedButtonForLoadData(false);

                    await Task.Run(() => GetDataForSearch(Load, Finished, Hide));

                    LockedButtonForLoadData(true);

                    Load.Close();

                    if (!Dialog.Cancelled_CB.Checked)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => !p.Canceled).ToList();
                    }

                    if (Dialog.DateEnable_CB.Checked && Dialog.Status_CB.SelectedIndex != 0)
                    {
                        Status Status = (Status)Dialog.Status_CB.SelectedItem;
                        var Orders = SystemArgs.StatusOfOrders.Where(p => p.DateCreate >= Dialog.First_DP.Value.Date && p.DateCreate <= Dialog.Second_DP.Value.Date.AddSeconds(86399) && p.IDStatus == Status.ID);
                        List<Order> Temp = new List<Order>();
                        foreach (var item in Orders)
                        {
                            List<Order> Order = SystemArgs.Orders.Where(p => p.ID == item.IDOrder).ToList();
                            if (Order.Count > 0)
                            {
                                Temp.Add(new Order(Order[0]));
                            }
                        }
                        SystemArgs.Orders = Temp;
                    }
                    else if (Dialog.DateEnable_CB.Checked)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => (p.DateCreate >= Dialog.First_DP.Value.Date) && (p.DateCreate <= Dialog.Second_DP.Value.Date.AddSeconds(86399))).ToList();
                    }
                    else if (Dialog.Status_CB.SelectedIndex > 0)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => p.Status == (Status)Dialog.Status_CB.SelectedItem).ToList();
                    }

                    if (Dialog.Executor_TB.Text.Trim() != String.Empty)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => p.Executor.IndexOf(Dialog.Executor_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.ExecutorWork_TB.Text.Trim() != String.Empty)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => p.ExecutorWork.IndexOf(Dialog.ExecutorWork_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.Number_TB.Text.Trim() != String.Empty)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => p.Number.IndexOf(Dialog.Number_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.List_TB.Text.Trim() != String.Empty)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => p.List.IndexOf(Dialog.List_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.Mark_TB.Text.Trim() != String.Empty)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => p.Mark.IndexOf(Dialog.Mark_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.Lenght_TB.Text.Trim() != String.Empty)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => p.Lenght.ToString().IndexOf(Dialog.Lenght_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.Weight_TB.Text.Trim() != String.Empty)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => p.Weight.ToString().IndexOf(Dialog.Weight_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.NumberBlankOrder_TB.Text.Trim() != String.Empty)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => p.BlankOrderView.IndexOf(Dialog.NumberBlankOrder_TB.Text.Trim()) != -1).ToList();
                    }

                    if (Dialog.User_CB.SelectedIndex > 0)
                    {
                        SystemArgs.Orders = SystemArgs.Orders.Where(p => p.User == (Models.User)Dialog.User_CB.SelectedItem).ToList();
                    }

                    UsedSearch = true;

                    Display(SystemArgs.Orders);
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool ReportOrderOfDate()
        {
            try
            {
                AR_ReportOrderOfDate_F Dialog = new AR_ReportOrderOfDate_F();
                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    if (SystemArgs.Excel.ReportOrderOfDate(Dialog.First_MC.SelectionStart, Dialog.Second_MC.SelectionStart))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void VisibleButton(Boolean Enable)
        {
            if (Enable)
            {
                ChangeOrder_TSB.Visible = true;
                ChangeOrder_TSM.Visible = true;
                CanceledOrder_TSB.Visible = true;
            }
            else
            {
                ChangeOrder_TSB.Visible = false;
                ChangeOrder_TSM.Visible = false;
                CanceledOrder_TSB.Visible = false;
            }
        }

        private void Selection(Order Temp, bool flag)
        {
            if (flag)
            {
                DateCreate_TB.Text = Temp.DateCreate.ToString();
                Executor_TB.Text = Temp.Executor;
                ExecutorWork_TB.Text = Temp.ExecutorWork;
                Number_TB.Text = Temp.Number;
                List_TB.Text = Temp.List.ToString();
                Mark_TB.Text = Temp.Mark;
                Lenght_TB.Text = Temp.Lenght.ToString();
                Weight_TB.Text = Temp.Weight.ToString();
                if (Temp.Canceled)
                {
                    Canceled_TB.BackColor = Color.Orange;
                    Canceled_TB.Text = "Да";
                }
                else
                {
                    Canceled_TB.BackColor = Color.Lime;
                    Canceled_TB.Text = "Нет";
                }
                if (Temp.Finished)
                {
                    Finished_TB.BackColor = Color.Orange;
                    Finished_TB.Text = "Да";
                }
                else
                {
                    Finished_TB.BackColor = Color.Lime;
                    Finished_TB.Text = "Нет";
                }
                if (Temp.Hide)
                {
                    Hide_TB.BackColor = Color.Orange;
                    Hide_TB.Text = "Да";
                }
                else
                {
                    Hide_TB.BackColor = Color.Lime;
                    Hide_TB.Text = "Нет";
                }
                BlankOrder_TB.Text = Temp.BlankOrder.QR;
                Status_TB.Text = Temp.Status.Name;

                SelectedOrder_TB.Text = Order_DGV.SelectedRows.Count.ToString();

            }
            else
            {
                DateCreate_TB.Text = String.Empty;
                Executor_TB.Text = String.Empty;
                ExecutorWork_TB.Text = String.Empty;
                Number_TB.Text = String.Empty;
                List_TB.Text = String.Empty;
                Mark_TB.Text = String.Empty;
                Lenght_TB.Text = String.Empty;
                Weight_TB.Text = String.Empty;
                Canceled_TB.BackColor = Color.FromArgb(233, 245, 255);
                Canceled_TB.Text = String.Empty;
                Finished_TB.BackColor = Color.FromArgb(233, 245, 255);
                Finished_TB.Text = String.Empty;
                Hide_TB.BackColor = Color.FromArgb(233, 245, 255);
                Hide_TB.Text = String.Empty;
                BlankOrder_TB.Text = String.Empty;
                Status_TB.Text = String.Empty;
                SelectedOrder_TB.Text = "0";
            }

        }

        private async void RefreshOrderAsync(int ForViews)
        {
            ForLongOperations_F Dialog = new ForLongOperations_F();
            Dialog.Show();

            LockedButtonForLoadData(false);

            await Task.Run(() => RefreshOrder(ForViews, Dialog));

            LockedButtonForLoadData(true);

            Dialog.Close();
        }

        private void LockedButtonForLoadData(bool flag)
        {
            AddOrder_TSB.Enabled = flag;
            AddOrder_TSM.Enabled = flag;
            ChangeOrder_TSB.Enabled = flag;
            ChangeOrder_TSM.Enabled = flag;
            Search_TSB.Enabled = flag;
            Reset_TSB.Enabled = flag;
            AdvancedSearch_TSB.Enabled = flag;
            FilterCB_TSB.Enabled = flag;
            RefreshStatus_B.Enabled = flag;
            CanceledOrder_TSB.Enabled = flag;
            ReportDate_TSM.Enabled = flag;
            SelectionReport_TSM.Enabled = flag;
            Time_Day_Report_TSM.Enabled = flag;
            Time_Week_Report_TSM.Enabled = flag;
            Time_Month_Report_TSM.Enabled = flag;
            Time_SelectionDate_Report_TSM.Enabled = flag;
            ViewSelected_B.Enabled = flag;
        }
        private void RefreshOrder(int ForViews, ForLongOperations_F Dialog)
        {
            try
            {
                SystemArgs.Orders.Clear();
                SystemArgs.BlankOrders.Clear();
                SystemArgs.StatusOfOrders.Clear();
                SystemArgs.BlankOrderOfOrders.Clear();

                GetOrders(ForViews, Dialog);

                Display(SystemArgs.Orders);
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());

                MessageBox.Show("Ошибка получения данных для обновления информации", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Environment.Exit(0);
            }
        }
        private void GetOrders(int ForViews, ForLongOperations_F Dialog)
        {
            switch (ForViews)
            {
                case 0:
                    SystemArgs.RequestLinq.GetOrdersUser(Dialog);
                    break;
                case 1:
                    SystemArgs.RequestLinq.GetOrdersAll(Dialog);
                    break;
                case 2:
                    SystemArgs.RequestLinq.GetOrdersCancelled(Dialog);
                    break;
                case 3:
                    SystemArgs.RequestLinq.GetOrdersFinished(Dialog);
                    break;
            }
        }

        private void RefreshStatus_B_Click(object sender, EventArgs e)
        {
            try
            {
                ResetSearch();
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Order_DGV_Sorted(object sender, EventArgs e)
        {
            try
            {
                if (FilterCB_TSB.SelectedIndex == 0)
                {
                    ForgetOrder();
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SettingMobile_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                Settings_MobileApp Dialog = new Settings_MobileApp();

                if (SystemArgs.MobileApplication.GetParametersConnect())
                {
                    String MyIP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();

                    Dialog.IP_TB.Text = MyIP;
                    Dialog.Port_TB.Text = SystemArgs.MobileApplication.Port;

                    Zen.Barcode.CodeQrBarcodeDraw QrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                    Dialog.QR_PB.Image = QrCode.Draw($"{MyIP}_{SystemArgs.MobileApplication.Port}", 100);
                }

                if (Dialog.ShowDialog() == DialogResult.OK)
                {

                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CanceledOrder_TSB_Click(object sender, EventArgs e)
        {
            try
            {
                if (Order_DGV.CurrentCell.RowIndex >= 0 && Order_DGV.SelectedRows.Count >= 0)
                {
                    if (MessageBox.Show("Изменить статус аннулирования чертежа(ей)?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        for (int i = 0; i < Order_DGV.SelectedRows.Count; i++)
                        {
                            Order Temp = (Order)(View[Order_DGV.SelectedRows[i].Index]);

                            try
                            {
                                Temp.Canceled = !Temp.Canceled;
                                SystemArgs.Request.CanceledOrder(Temp.Canceled, Temp.ID);
                                SystemArgs.Orders.Remove(Temp);
                            }
                            catch
                            {
                                MessageBox.Show("Произошла ошибка при аннулировании чертежа: Номер-" + Temp.Number + " Лист-" + Temp.List, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        Display(SystemArgs.Orders);
                    }
                }
                else
                {
                    throw new Exception("Необходимо выбрать один объект(ы)");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectionReport_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                List<Order> Report = new List<Order>();
                if (Order_DGV.CurrentCell != null && Order_DGV.CurrentCell.RowIndex >= 0)
                {
                    for (int i = 0; i < Order_DGV.SelectedRows.Count; i++)
                    {
                        Report.Add((Order)(View[Order_DGV.SelectedRows[i].Index]));
                    }
                    SystemArgs.Excel.ReportOrderOfSelect(Report);
                }
                else
                {
                    throw new Exception("Необходимо выбрать чертежи");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenameOrder_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                AR_RenameOrder_F Dialog = new AR_RenameOrder_F();

                if (SystemArgs.ByteScout.CheckConnect())
                {
                    Dialog.ServerStatus_TB.Text = "Подключено";
                    Dialog.ServerStatus_TB.BackColor = Color.FromArgb(233, 245, 255);
                    Dialog.Status_TB.AppendText($"Ожидание распознования" + Environment.NewLine);

                    if (Dialog.ShowDialog() == DialogResult.OK)
                    {
                        AR_DecodeReport_F Report = new AR_DecodeReport_F();
                        Report.Report_DGV.RowCount = 0;

                        for (int i = 0; i < Dialog.FileNames.Count; i++)
                        {
                            Report.Report_DGV.RowCount++;
                            Report.Report_DGV[0, Report.Report_DGV.Rows.Count - 1].Value = Dialog.DecodeNames[i];
                            Report.Report_DGV[1, Report.Report_DGV.Rows.Count - 1].Value = CopyFileToArhive(Dialog.DecodeNames[i], Dialog.FileNames[i]);
                        }
                        if (Dialog.DecodeNames.Count > 0)
                        {
                            Report.ShowDialog();
                        }
                        else
                        {
                            Report.Dispose();
                        }
                    }
                }
                else
                {
                    throw new Exception("Ошибка при подключении к серверу распознавнаия");
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Time_Day_Report_TSM_Click(object sender, EventArgs e)
        {
            ReportTimeofOrderPeriod(new TimeSpan(1, 0, 0, 1));
        }

        private void Time_Week_Report_TSM_Click(object sender, EventArgs e)
        {
            ReportTimeofOrderPeriod(new TimeSpan(7, 0, 0, 1));
        }

        private void Time_Month_Report_TSM_Click(object sender, EventArgs e)
        {
            ReportTimeofOrderPeriod(new TimeSpan(30, 0, 0, 1));
        }

        private void Time_SelectionDate_Report_TSM_Click(object sender, EventArgs e)
        {
            ReportTimeofOrder();
        }

        private void ReportTimeofOrderPeriod(object aInterval)
        {
            try
            {
                SaveFileDialog SaveReport = new SaveFileDialog();
                String date = DateTime.Now.ToString();
                date = date.Replace(".", "_");
                date = date.Replace(":", "_");
                SaveReport.FileName = "Отчет по времени за выбранный период от " + date;
                SaveReport.Filter = "Excel Files .xlsx|*.xlsx";
                if (SaveReport.ShowDialog() == DialogResult.OK)
                {
                    ALL_FormingReportForAllPosition_F FormingF = new ALL_FormingReportForAllPosition_F();
                    FormingF.Show();
                    List<StatusOfOrder> Report = SystemArgs.StatusOfOrders.Where(p => p.DateCreate <= DateTime.Now && p.DateCreate >= DateTime.Now.Subtract((TimeSpan)aInterval)).ToList();
                    Task<Boolean> task = ReportPastTimeAsync(Report, SaveReport.FileName);
                    task.ContinueWith(t =>
                    {
                        if (t.Result)
                        {
                            FormingF.Invoke((MethodInvoker)delegate ()
                            {
                                FormingF.Close();
                            });
                            if (MessageBox.Show("Отчет сформирован успешно." + Environment.NewLine + "Открыть его?", "Информация", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                if (File.Exists(SaveReport.FileName))
                                {
                                    Process.Start(SaveReport.FileName);
                                }
                                else
                                {
                                    MessageBox.Show("Отчет по пути не обнаружен." + Environment.NewLine + "Ошибка открытия отчета!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            FormingF.Invoke((MethodInvoker)delegate ()
                            {
                                FormingF.Close();
                            });
                            MessageBox.Show("Ошибка фомирования отчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ReportTimeofOrder()
        {
            try
            {
                AR_ReportOrderOfDate_F Dialog = new AR_ReportOrderOfDate_F();
                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    SaveFileDialog SaveReport = new SaveFileDialog();
                    String date = DateTime.Now.ToString();
                    date = date.Replace(".", "_");
                    date = date.Replace(":", "_");
                    SaveReport.FileName = "Отчет по времени за выбранный период от " + date;
                    SaveReport.Filter = "Excel Files .xlsx|*.xlsx";
                    if (SaveReport.ShowDialog() == DialogResult.OK)
                    {
                        ALL_FormingReportForAllPosition_F FormingF = new ALL_FormingReportForAllPosition_F();
                        FormingF.Show();
                        List<StatusOfOrder> Report = SystemArgs.StatusOfOrders.Where(p => p.DateCreate >= Dialog.First_MC.SelectionStart && p.DateCreate <= Dialog.Second_MC.SelectionStart).ToList();
                        Task<Boolean> task = ReportPastTimeAsync(Report, SaveReport.FileName);
                        task.ContinueWith(t =>
                        {
                            if (t.Result)
                            {
                                FormingF.Invoke((MethodInvoker)delegate ()
                                {
                                    FormingF.Close();
                                });
                                if (MessageBox.Show("Отчет сформирован успешно." + Environment.NewLine + "Открыть его?", "Информация", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    if (File.Exists(SaveReport.FileName))
                                    {
                                        Process.Start(SaveReport.FileName);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Отчет по пути не обнаружен." + Environment.NewLine + "Ошибка открытия отчета!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else
                            {
                                FormingF.Invoke((MethodInvoker)delegate ()
                                {
                                    FormingF.Close();
                                });
                                MessageBox.Show("Ошибка фомирования отчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        });
                    }
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<Boolean> ReportPastTimeAsync(List<StatusOfOrder> Report, String filename)
        {
            return await Task.Run(() => SystemArgs.Excel.ReportPastTimeofDate(Report, filename));
        }

        private void AboutProgram_TSM_Click(object sender, EventArgs e)
        {
            ALL_AboutProgram_F Dialog = new ALL_AboutProgram_F();
            if (Dialog.ShowDialog() == DialogResult.OK)
            {

            }
        }

        public static void SetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }
        private void SelectedColumnDGV()
        {
            try
            {
                List<Column> Temp = SystemArgs.SelectedColumn.GetColumns().OrderBy(p => p.DisplayIndex).ToList();
                for (int i = 0; i < Temp.Count(); i++)
                {
                    Order_DGV.Columns[Temp[i].Name].DisplayIndex = Temp[i].DisplayIndex;
                    Order_DGV.Columns[Temp[i].Name].Visible = Temp[i].Visible;
                    Order_DGV.Columns[Temp[i].Name].FillWeight = Temp[i].FillWeight;
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                throw new Exception(E.Message);
            }
        }

        private void SelectedColumn_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                AR_SelectedColumnDGV_F Dialog = new AR_SelectedColumnDGV_F();

                Dialog.DataMatrix_CB.Checked = SystemArgs.SelectedColumn[0].Visible;
                Dialog.DateCreate_CB.Checked = SystemArgs.SelectedColumn[1].Visible;
                Dialog.Number_CB.Checked = SystemArgs.SelectedColumn[2].Visible;
                Dialog.Executor_CB.Checked = SystemArgs.SelectedColumn[3].Visible;
                Dialog.ExecutorWork_CB.Checked = SystemArgs.SelectedColumn[4].Visible;
                Dialog.List_CB.Checked = SystemArgs.SelectedColumn[5].Visible;
                Dialog.Mark_CB.Checked = SystemArgs.SelectedColumn[6].Visible;
                Dialog.Lenght_CB.Checked = SystemArgs.SelectedColumn[7].Visible;
                Dialog.Height_CB.Checked = SystemArgs.SelectedColumn[8].Visible;
                Dialog.Status_CB.Checked = SystemArgs.SelectedColumn[9].Visible;
                Dialog.User_CB.Checked = SystemArgs.SelectedColumn[10].Visible;
                Dialog.BlankOrder_CB.Checked = SystemArgs.SelectedColumn[11].Visible;
                Dialog.Cancelled_CB.Checked = SystemArgs.SelectedColumn[12].Visible;
                Dialog.StatusDate_CB.Checked = SystemArgs.SelectedColumn[13].Visible;
                Dialog.Comment_CB.Checked = SystemArgs.SelectedColumn[14].Visible;
                Dialog.Finished_CB.Checked = SystemArgs.SelectedColumn[15].Visible;
                Dialog.Hide_CB.Checked = SystemArgs.SelectedColumn[16].Visible;

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    SystemArgs.SelectedColumn[0].Visible = Dialog.DataMatrix_CB.Checked;
                    SystemArgs.SelectedColumn[1].Visible = Dialog.DateCreate_CB.Checked;
                    SystemArgs.SelectedColumn[2].Visible = Dialog.Number_CB.Checked;
                    SystemArgs.SelectedColumn[3].Visible = Dialog.Executor_CB.Checked;
                    SystemArgs.SelectedColumn[4].Visible = Dialog.ExecutorWork_CB.Checked;
                    SystemArgs.SelectedColumn[5].Visible = Dialog.List_CB.Checked;
                    SystemArgs.SelectedColumn[6].Visible = Dialog.Mark_CB.Checked;
                    SystemArgs.SelectedColumn[7].Visible = Dialog.Lenght_CB.Checked;
                    SystemArgs.SelectedColumn[8].Visible = Dialog.Height_CB.Checked;
                    SystemArgs.SelectedColumn[9].Visible = Dialog.Status_CB.Checked;
                    SystemArgs.SelectedColumn[10].Visible = Dialog.User_CB.Checked;
                    SystemArgs.SelectedColumn[11].Visible = Dialog.BlankOrder_CB.Checked;
                    SystemArgs.SelectedColumn[12].Visible = Dialog.Cancelled_CB.Checked;
                    SystemArgs.SelectedColumn[13].Visible = Dialog.StatusDate_CB.Checked;
                    SystemArgs.SelectedColumn[14].Visible = Dialog.Comment_CB.Checked;
                    SystemArgs.SelectedColumn[15].Visible = Dialog.Finished_CB.Checked;
                    SystemArgs.SelectedColumn[16].Visible = Dialog.Hide_CB.Checked;
                    SystemArgs.SelectedColumn.SetParametrColumnVisible();
                    SystemArgs.SelectedColumn.GetParametrColumn();
                    MessageBox.Show("Настройки успешно сохранены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SelectedColumnDGV();
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SettingConfig_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                AR_SettingConfig_F Dialog = new AR_SettingConfig_F();

                if (SystemArgs.SettingsUser.GetParametersConnect())
                {
                    Dialog.ArchivePath_TB.Text = SystemArgs.SettingsUser.ArchivePath;
                }

                if (Dialog.ShowDialog() == DialogResult.OK)
                {

                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Order_DGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (Order_DGV.CurrentCell != null && Order_DGV.CurrentCell.RowIndex < View.Count() && e.RowIndex >= 0)
                {
                    Order Temp = (Order)View[Order_DGV.CurrentCell.RowIndex];
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

        private void CompleteStatusReport_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog SaveReport = new SaveFileDialog();
                String date = DateTime.Now.ToString();
                date = date.Replace(".", "_");
                date = date.Replace(":", "_");
                SaveReport.FileName = "Отчет прохождения статусов от " + date;
                SaveReport.Filter = "Excel Files .xlsx|*.xlsx";

                if (SaveReport.ShowDialog() == DialogResult.OK)
                {

                    ALL_FormingReportForAllPosition_F FormingF = new ALL_FormingReportForAllPosition_F();
                    FormingF.Show();
                    Task<Boolean> task = ReportCompleteStatuses(SaveReport.FileName);
                    task.ContinueWith(t =>
                    {
                        if (t.Result)
                        {
                            FormingF.Invoke((MethodInvoker)delegate ()
                            {
                                FormingF.Close();
                            });
                            if (MessageBox.Show("Отчет сформирован успешно." + Environment.NewLine + "Открыть его?", "Информация", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                if (File.Exists(SaveReport.FileName))
                                {
                                    Process.Start(SaveReport.FileName);
                                }
                                else
                                {
                                    MessageBox.Show("Отчет по пути не обнаружен." + Environment.NewLine + "Ошибка открытия отчета!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            FormingF.Invoke((MethodInvoker)delegate ()
                            {
                                FormingF.Close();
                            });
                            MessageBox.Show("Ошибка фомирования отчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<Boolean> ReportCompleteStatuses(String filename)
        {
            return await Task.Run(() => SystemArgs.Excel.ReportCompleteStatuses(filename));
        }

        private void ViewSelected_B_Click(object sender, EventArgs e)
        {
            try
            {
                SystemArgs.Orders = new OperationsDisplayDrawings().ViewSeletedDrawing(ref Order_DGV);

                Display(SystemArgs.Orders);
            }
            catch (Exception ex)
            {
                SystemArgs.PrintLog(ex.ToString());
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Order_DGV_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (Order_DGV.Columns[e.ColumnIndex] == Order_DGV.Columns["List"])
                {
                    ListFieldSort listFieldSort = new ListFieldSort();

                    if (listFieldSort.IsSort)
                    {
                        Display(listFieldSort.SortAsc(View).ToList());
                    }
                    else
                    {
                        Display(listFieldSort.SortDesc(View).ToList());
                    }

                    listFieldSort.IsSort = !listFieldSort.IsSort;
                }
            }
            catch (Exception ex)
            {
                SystemArgs.PrintLog(ex.ToString());
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReportSteelStandart_TSB_Click(object sender, EventArgs e)
        {
            try
            {
                if (Order_DGV.CurrentCell != null && Order_DGV.CurrentCell.RowIndex >= 0)
                {
                    SaveFileDialog SaveReport = new SaveFileDialog();
                    String date = DateTime.Now.ToString();
                    date = date.Replace(".", "_");
                    date = date.Replace(":", "_");
                    SaveReport.FileName = "Отчет выборки металла от " + date;
                    SaveReport.Filter = "Excel Files .xlsx|*.xlsx";
                    List<Order> Report = new List<Order>();

                    for (int i = 0; i < Order_DGV.SelectedRows.Count; i++)
                    {
                        Report.Add((Order)(View[Order_DGV.SelectedRows[i].Index]));
                    }

                    if (SaveReport.ShowDialog() == DialogResult.OK)
                    {

                        ALL_FormingReportForAllPosition_F FormingF = new ALL_FormingReportForAllPosition_F();
                        FormingF.Show();
                        Task<Boolean> task = ReportSteelStandartAsync(Report, SaveReport.FileName);
                        task.ContinueWith(t =>
                        {
                            if (t.Result)
                            {
                                FormingF.Invoke((MethodInvoker)delegate ()
                                {
                                    FormingF.Close();
                                });
                                if (MessageBox.Show("Отчет сформирован успешно." + Environment.NewLine + "Открыть его?", "Информация", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    if (File.Exists(SaveReport.FileName))
                                    {
                                        Process.Start(SaveReport.FileName);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Отчет по пути не обнаружен." + Environment.NewLine + "Ошибка открытия отчета!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else
                            {
                                FormingF.Invoke((MethodInvoker)delegate ()
                                {
                                    FormingF.Close();
                                });
                                MessageBox.Show("Ошибка фомирования отчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        });
                    }
                }
                else
                {
                    throw new Exception("Необходимо выбрать чертежи");
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<Boolean> ReportSteelStandartAsync(List<Order> Report, String filename)
        {
            return await Task.Run(() => SystemArgs.Excel.ReportSteelOfDate(Report, filename));
        }

        private void ReportSteelMark_TSB_Click(object sender, EventArgs e)
        {
            try
            {
                if (Order_DGV.CurrentCell != null && Order_DGV.CurrentCell.RowIndex >= 0)
                {
                    SaveFileDialog SaveReport = new SaveFileDialog();
                    String date = DateTime.Now.ToString();
                    date = date.Replace(".", "_");
                    date = date.Replace(":", "_");
                    SaveReport.FileName = "Отчет выборки металла по маркам от " + date;
                    SaveReport.Filter = "Excel Files .xlsx|*.xlsx";
                    List<Order> Report = new List<Order>();

                    for (int i = 0; i < Order_DGV.SelectedRows.Count; i++)
                    {
                        Report.Add((Order)(View[Order_DGV.SelectedRows[i].Index]));
                    }

                    if (SaveReport.ShowDialog() == DialogResult.OK)
                    {

                        ALL_FormingReportForAllPosition_F FormingF = new ALL_FormingReportForAllPosition_F();
                        FormingF.Show();
                        Task<Boolean> task = ReportSteelMarkAsync(Report, SaveReport.FileName);
                        task.ContinueWith(t =>
                        {
                            if (t.Result)
                            {
                                FormingF.Invoke((MethodInvoker)delegate ()
                                {
                                    FormingF.Close();
                                });
                                if (MessageBox.Show("Отчет сформирован успешно." + Environment.NewLine + "Открыть его?", "Информация", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    if (File.Exists(SaveReport.FileName))
                                    {
                                        Process.Start(SaveReport.FileName);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Отчет по пути не обнаружен." + Environment.NewLine + "Ошибка открытия отчета!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else
                            {
                                FormingF.Invoke((MethodInvoker)delegate ()
                                {
                                    FormingF.Close();
                                });
                                MessageBox.Show("Ошибка фомирования отчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        });
                    }
                }
                else
                {
                    throw new Exception("Необходимо выбрать чертежи");
                }
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<Boolean> ReportSteelMarkAsync(List<Order> Report, String filename)
        {
            return await Task.Run(() => SystemArgs.Excel.ReportMarkSelected(Report, filename));
        }

        private void CheckDetails_TSMI_Click(object sender, EventArgs e)
        {
            ForLongOperations_F Dialog = new ForLongOperations_F();

            try
            {
                List<Order> selected = new List<Order>();

                if (Order_DGV.CurrentCell != null && Order_DGV.CurrentCell.RowIndex >= 0)
                {
                    Dialog.Show();

                    Dialog.SetMaximum(Order_DGV.SelectedRows.Count);

                    for (int i = 0; i < Order_DGV.SelectedRows.Count; i++)
                    {
                        selected.Add((Order)(View[Order_DGV.SelectedRows[i].Index]));

                        Dialog.Notify(i + 1, $"Получение чертежа {i + 1} из {Order_DGV.SelectedRows.Count}");
                    }

                    Task<Boolean> task = CheckedDetailsAsync(selected, Dialog);
                    task.ContinueWith(t =>
                    {
                        if (t.Result)
                        {
                            Dialog.Invoke((MethodInvoker)delegate ()
                            {
                                Dialog.Close();
                            });
                            if (SystemArgs.UnLoadSpecific.ExecutorMails.Count != 0)
                            {
                                ReportUnloadingSpecific unloadSpecific = new ReportUnloadingSpecific();
                                unloadSpecific.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Все детали найдены!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            SystemArgs.UnLoadSpecific.ExecutorMails.Clear();
                        }
                        else
                        {
                            Dialog.Invoke((MethodInvoker)delegate ()
                            {
                                Dialog.Close();
                            });
                            MessageBox.Show("Ошибка проверки деталировки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
                else
                {
                    throw new Exception("Необходимо выбрать чертежи");
                }
            }
            catch (Exception E)
            {
                Dialog.Close();
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<Boolean> CheckedDetailsAsync(List<Order> selected, ForLongOperations_F dialog)
        {
            return await Task.Run(() =>
            {
                try
                {
                    dialog.SetMaximum(selected.Count);

                    for (int i = 0; i < selected.Count; i++)
                    {
                        SystemArgs.UnLoadSpecific.ChekedUnloading(selected[i].Number, selected[i].List, selected[i].Executor);
                        dialog.Notify(i + 1, $"Проверка чертежа {i + 1} из {selected.Count}");
                    }

                    return true;
                }
                catch (Exception E)
                {
                    SystemArgs.PrintLog(E.ToString());
                    return false;
                }
            });
        }
    }
}
