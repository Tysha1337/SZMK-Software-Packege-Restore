﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SZMK.Desktop.BindingModels;
using SZMK.Desktop.Models;
using SZMK.Desktop.Views.Shared;

namespace SZMK.Desktop.Services.Scan
{
    public class BaseScanOrder
    {
        public bool SetResult(Order Order, List<OrderScanSession> Orders, bool XML)
        {
            try
            {
                if (CheckedUniqueList(Order))
                {
                    String ReplaceMark = "";

                    String[] ExistingCharaterEnglish = new String[] { "A", "a", "B", "C", "c", "E", "e", "H", "K", "M", "O", "o", "P", "p", "T" };
                    String[] ExistingCharaterRussia = new String[] { "А", "а", "В", "С", "с", "Е", "е", "Н", "К", "М", "О", "о", "Р", "р", "Т" };

                    for (int i = 0; i < ExistingCharaterRussia.Length; i++)
                    {
                        ReplaceMark = Order.Mark.Replace(ExistingCharaterRussia[i], ExistingCharaterEnglish[i]);
                    }

                    String[] Splitter = Order.List.Split('и');

                    while (Splitter[0][0] == '0')
                    {
                        Splitter[0] = Splitter[0].Remove(0, 1);
                    }

                    if (Splitter.Length != 1)
                    {
                        Order.List = Splitter[0] + "и" + Splitter[1];
                    }
                    else
                    {
                        Order.List = Splitter[0];
                    }

                    if (!CheckedExecutor(Order.Executor))
                    {
                        Orders.Add(new OrderScanSession(Order, 0, $"Пустое имя исполнителя"));
                        return true;
                    }
                    //else
                    //{
                    //    Order.Executor = FormingExecutor(Order.Executor);
                    //}

                    if (SystemArgs.SettingsProgram.CheckMarks)
                    {
                        if (!CheckedLowerRegistery(ReplaceMark))
                        {
                            Orders.Add(new OrderScanSession(Order, 0, $"Строчные буквы в префиксе марки «{ReplaceMark}» не допускаются"));
                            return true;
                        }
                    }

                    if (ReplaceMark.IndexOf("(?)") != -1)
                    {
                        Orders.Add(new OrderScanSession(Order, 0, $"В заказе {Order.Number}, марка {ReplaceMark} уже существует."));
                        return true;
                    }

                    Int32 IndexException = SystemArgs.Request.CheckedNumberAndList(Order, XML);

                    switch (IndexException)
                    {
                        case 0:
                            if (SystemArgs.Request.CheckedNumberAndMark(Order.Number, ReplaceMark))
                            {
                                Orders.Add(new OrderScanSession(Order, 1, "Добавление чертежа необходимо проводить через Инженера конструктора"));
                            }
                            else
                            {
                                Orders.Add(new OrderScanSession(Order, 0, $"В заказе {Order.Number}, марка {ReplaceMark} уже существует."));
                            }
                            break;
                        case 1:
                            Orders.Add(new OrderScanSession(Order, 0, $"В заказе {Order.Number}, номер листа {Order.List} уже существует."));
                            break;
                        case 2:
                            Orders.Add(new OrderScanSession(Order, 1, "Добавление чертежа необходимо проводить через Инженера конструктора"));
                            break;
                        case 3:
                            Orders.Add(new OrderScanSession(Order, 2, "-"));
                            break;
                        case 4:
                            Order OldOrder = SystemArgs.Request.GetOrder(Order.List, Order.Number);

                            ALL_UpdateOrder_F UpdateOrder = new ALL_UpdateOrder_F();

                            UpdateOrder.Height = 358;

                            UpdateOrder.NewList_TB.Text = Order.List;
                            UpdateOrder.NewMark_TB.Text = Order.Mark;
                            UpdateOrder.NewExecutor_TB.Text = Order.Executor;
                            UpdateOrder.NewWeight_TB.Text = Order.Weight.ToString();
                            UpdateOrder.NewDetails_DGV.Visible = false;
                            UpdateOrder.NewDetails_L.Visible = false;

                            UpdateOrder.OldList_TB.Text = OldOrder.List;
                            UpdateOrder.OldMark_TB.Text = OldOrder.Mark;
                            UpdateOrder.OldExecutor_TB.Text = OldOrder.Executor;
                            UpdateOrder.OldWeight_TB.Text = OldOrder.Weight.ToString();
                            UpdateOrder.OldDetails_DGV.Visible = false;
                            UpdateOrder.OldDetails_L.Visible = false;

                            string statusForOrder = SystemArgs.Statuses.Find(p => p.ID == SystemArgs.Request.GetIdStatusOrder(OldOrder.ID)).Name;

                            if (statusForOrder != "Добавлен инженером конструктором")
                            {
                                UpdateOrder.OldStatus_TB.Text = $"Чертёж будет обновлён. Чертёж находится на статусе \"{statusForOrder}\".\n\r Необходимо передать чертёж на дальнейшее сканирование.";
                            }
                            else
                            {
                                UpdateOrder.OldStatus_TB.Text = $"Чертёж будет обновлён. Чертёж находится на статусе \"{statusForOrder}\".";
                            }

                            if (UpdateOrder.ShowDialog() == DialogResult.OK)
                            {
                                Orders.Add(new OrderScanSession(Order, 3, "-"));
                            }
                            else
                            {
                                Orders.Add(new OrderScanSession(Order, 0, $"В заказе {Order.Number}, номер листа {Order.List} уже существует."));
                            }
                            break;
                        case 5:
                            Order OldOrderDetails = SystemArgs.Request.GetOrder(Order.List, Order.Number);

                            ALL_UpdateOrder_F UpdateDetails = new ALL_UpdateOrder_F();

                            UpdateDetails.NewDetails_DGV.AutoGenerateColumns = false;
                            UpdateDetails.NewList_TB.Text = Order.List;
                            UpdateDetails.NewMark_TB.Text = Order.Mark;
                            UpdateDetails.NewExecutor_TB.Text = Order.Executor;
                            UpdateDetails.NewWeight_TB.Text = Order.Weight.ToString();
                            UpdateDetails.NewDetails_DGV.DataSource = Order.Details;

                            UpdateDetails.OldDetails_DGV.AutoGenerateColumns = false;
                            UpdateDetails.OldList_TB.Text = OldOrderDetails.List;
                            UpdateDetails.OldMark_TB.Text = OldOrderDetails.Mark;
                            UpdateDetails.OldExecutor_TB.Text = OldOrderDetails.Executor;
                            UpdateDetails.OldWeight_TB.Text = OldOrderDetails.Weight.ToString();
                            UpdateDetails.OldDetails_DGV.DataSource = SystemArgs.Request.GetDetails(OldOrderDetails.ID);

                            string statusForDetails = SystemArgs.Statuses.Find(p => p.ID == SystemArgs.Request.GetIdStatusOrder(OldOrderDetails.ID)).Name;

                            if (statusForDetails != "Добавлен инженером конструктором")
                            {
                                UpdateDetails.OldStatus_TB.Text = $"Чертёж будет обновлён. Чертёж находится на статусе \"{statusForDetails}\".\n\r Необходимо передать чертёж на дальнейшее сканирование.";
                            }
                            else
                            {
                                UpdateDetails.OldStatus_TB.Text = $"Чертёж будет обновлён. Чертёж находится на статусе \"{statusForDetails}\".";
                            }

                            if (UpdateDetails.ShowDialog() == DialogResult.OK)
                            {
                                Orders.Add(new OrderScanSession(Order, 3, "-"));
                            }
                            else
                            {
                                Orders.Add(new OrderScanSession(Order, 0, $"В заказе {Order.Number}, номер листа {Order.List} уже существует."));
                            }
                            break;
                        case 6:
                            Order OLDOrder = SystemArgs.Request.GetOrder(Order.List, Order.Number);
                            Orders.Add(new OrderScanSession(Order, 0, $"Чертёж находится на статусе \"{OLDOrder.Status.Name}\", необходимо выдать изменённый чертёж"));
                            break;
                        default:
                            Orders.Add(new OrderScanSession(Order, 0, "Ошибка добавления чертежа"));
                            break;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //private string FormingExecutor(string Executor)
        //{
        //    try
        //    {
        //        Executor = Executor.Insert(Executor.IndexOf('.') - 1, " ");

        //        return Executor;
        //    }
        //    catch
        //    {
        //        throw new Exception("Имя исполнителя должно быть в формате: Иванов И.И.");
        //    }
        //}
        private bool CheckedExecutor(string Executor)
        {
            try
            {
                if (String.IsNullOrEmpty(Executor))
                {
                    return false;
                }
                return true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private bool CheckedLowerRegistery(String Mark)
        {
            String LowerCharacters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяabcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < Mark.Length; i++)
            {
                if (LowerCharacters.IndexOf(Mark[i]) != -1)
                {
                    return false;
                }
            }
            return true;
        }

        readonly List<Order> UniqueList = new List<Order>();
        private bool CheckedUniqueList(Order Order)
        {
            if (UniqueList.FindAll(p => p.Number == Order.Number && p.List == Order.List).Count != 0)
            {
                return false;
            }
            else
            {
                UniqueList.Add(new Order(Order.ID, Order.DateCreate, Order.Number, Order.Executor, Order.ExecutorWork, Order.List, Order.Mark, Order.Lenght, Order.Weight, Order.WeightDifferent, Order.Status, Order.StatusDate, Order.TypeAdd, Order.Model, Order.PathDetails, Order.PathArhive, Order.Revision, Order.Comment, Order.User, Order.BlankOrder, Order.Hide, Order.Canceled, Order.Finished));
                return true;
            }
        }
        public Order FormingOrder(string DataMatrix)
        {
            string[] ValidationDataMatrix = DataMatrix.Replace(" ", "").Split('_');

            if (ValidationDataMatrix.Length != 6)
            {
                throw new Exception($"В DataMatrix менее 6 полей");
            }

            return new Order(0, DateTime.Now, ValidationDataMatrix[0], ValidationDataMatrix[3], "Исполнитель не определен", ValidationDataMatrix[1], ValidationDataMatrix[2], Convert.ToDouble(ValidationDataMatrix[4].Replace(".", ",")), Convert.ToDouble(ValidationDataMatrix[5].Replace(".", ",")), 0, null, DateTime.Now, null, null, null, null, null, null, null, new BlankOrder(), false, false, false);
        }
    }
}
