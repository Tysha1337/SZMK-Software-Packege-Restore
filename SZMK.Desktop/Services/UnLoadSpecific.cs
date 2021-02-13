using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SZMK.Desktop.Models;
using SZMK.Desktop.BindingModels;

namespace SZMK.Desktop.Services
{
    /*Класс для определения выгрузки деталей с методом проверки а также структурой для хранения исполнителя деталей и все детали в ней*/
    public class UnLoadSpecific
    {
        public struct ExecutorMail
        {
            private String _Executor;
            private List<Specific> _Specifics;
            public ExecutorMail(String Executor)
            {
                if (!String.IsNullOrEmpty(Executor))
                {
                    _Executor = Executor;
                }
                else
                {
                    throw new Exception("Не задан исполнитель");
                }
                _Specifics = new List<Specific>();
            }
            public String Executor
            {
                get
                {
                    return _Executor;
                }
                set
                {
                    if (!String.IsNullOrEmpty(Executor))
                    {
                        _Executor = value;
                    }
                }
            }
            public Specific this[Int32 Index]
            {
                get
                {
                    return _Specifics[Index];
                }
                set
                {
                    if (value != null)
                    {
                        _Specifics[Index] = value;
                    }
                }
            }
            public List<Specific> GetSpecifics()
            {
                return _Specifics;
            }
        }

        public List<ExecutorMail> ExecutorMails;
        public UnLoadSpecific()
        {
            ExecutorMails = new List<ExecutorMail>();
        }

        public void ChekedUnloading(String Number, String List, String Executor)
        {
            Order order = SystemArgs.Request.GetOrder(List, Number);
            Int64 IDOrder = order.ID;
            List<Detail> details = SystemArgs.Request.GetDetails(IDOrder);
            string pathDetails = order.PathDetails.Path;

            if (details.GroupBy(p => p.Position).Count() > 1)
            {
                for (int j = 0; j < details.Count; j++)
                {
                    if (details[j] != null)
                    {
                        if (CheckedDetail(pathDetails, details[j].Position.ToString()))
                        {
                            if (ExecutorMails.Where(p => p.Executor.Equals(Executor)).Count() != 0)
                            {
                                foreach (var item in SystemArgs.UnLoadSpecific.ExecutorMails)
                                {
                                    if (Executor.Equals(item.Executor))
                                    {
                                        item.GetSpecifics().Add(new Specific(Number, List, details[j].Position, pathDetails, true));
                                    }
                                }
                            }
                            else
                            {
                                ExecutorMails.Add(new ExecutorMail(Executor));
                                ExecutorMails[ExecutorMails.Count() - 1].GetSpecifics().Add(new Specific(Number, List, details[j].Position, pathDetails, true));
                            }
                        }
                        else
                        {
                            if (ExecutorMails.Where(p => p.Executor.Equals(Executor)).Count() != 0)
                            {
                                foreach (var item in ExecutorMails)
                                {
                                    if (Executor.Equals(item.Executor))
                                    {
                                        item.GetSpecifics().Add(new Specific(Number, List, details[j].Position, pathDetails, false));
                                    }
                                }
                            }
                            else
                            {
                                ExecutorMails.Add(new ExecutorMail(Executor));
                                ExecutorMails[ExecutorMails.Count() - 1].GetSpecifics().Add(new Specific(Number, List, details[j].Position, pathDetails, false));
                            }
                        }
                    }
                }
            }
        }
        private bool CheckedDetail(string pathDetails, string Position)
        {
            try
            {
                if (File.Exists(pathDetails + @"\" + "Дет." + Position + ".dwg"))
                {
                    return true;
                }
                else if (Directory.Exists(pathDetails) && Directory.GetFiles(pathDetails, Position + " - Дет.*.dwg", SearchOption.TopDirectoryOnly).Length != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<OrderPathDetailsBindingModel> CheckDetails(List<Order> Orders)
        {
            List<OrderPathDetailsBindingModel> orderPathDetails = new List<OrderPathDetailsBindingModel>();

            var GroupOrder = Orders.GroupBy(p => p.Number);

            foreach (var order in GroupOrder)
            {
                try
                {
                    if (Directory.Exists(Orders[0].Model.Path + @"\Чертежи\Детали DWG"))
                    {
                        orderPathDetails.Add(new OrderPathDetailsBindingModel { Order = order.Key, Path = Orders[0].Model.Path + @"\Чертежи\Детали DWG", Finded = true });
                    }
                    else if (Directory.Exists(Orders[0].Model.Path + @"\Чертежи\" + order.Key + @"\Детали DWG"))
                    {
                        orderPathDetails.Add(new OrderPathDetailsBindingModel { Order = order.Key, Path = Orders[0].Model.Path + @"\Чертежи\" + order.Key + @"\Детали DWG", Finded = true });
                    }
                    else
                    {
                        orderPathDetails.Add(new OrderPathDetailsBindingModel { Order = order.Key, Path = "Не найдена папка деталировки", Finded = false });
                    }
                }
                catch
                {
                    orderPathDetails.Add(new OrderPathDetailsBindingModel { Order = order.Key, Path = "Ошибка прав доступа к папке с деталировкой", Finded = false });
                }
            }

            return orderPathDetails;
        }
    }
}
