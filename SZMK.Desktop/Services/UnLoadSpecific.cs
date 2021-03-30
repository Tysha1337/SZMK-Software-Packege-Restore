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
            string pathDetails = order.PathDetails.PathDWG;

            if (details.GroupBy(p => p.Position).Count() > 1)
            {
                for (int j = 0; j < details.Count; j++)
                {
                    if (details[j] != null)
                    {
                        if (CheckedDetail(pathDetails, details[j]))
                        {
                            if (ExecutorMails.Where(p => p.Executor.Equals(Executor)).Count() != 0)
                            {
                                foreach (var item in SystemArgs.UnLoadSpecific.ExecutorMails)
                                {
                                    if (Executor.Equals(item.Executor))
                                    {
                                        if (item.GetSpecifics().FindAll(p => p.Number == Number && p.NumberSpecific == details[j].Position).Count == 0)
                                        {
                                            item.GetSpecifics().Add(new Specific(Number, List, details[j].Position, pathDetails, true));
                                        }
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
                                        if (item.GetSpecifics().FindAll(p => p.Number == Number && p.NumberSpecific == details[j].Position).Count == 0)
                                        {
                                            item.GetSpecifics().Add(new Specific(Number, List, details[j].Position, pathDetails, false));
                                        }
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
        private bool CheckedDetail(string pathDetails, Detail detail)
        {
            try
            {
                if (!String.IsNullOrEmpty(detail.Name))
                {
                    if (File.Exists(pathDetails + @"\" + detail.Name + ".dwg"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (File.Exists(pathDetails + @"\" + "Дет." + detail.Position + ".dwg"))
                    {
                        return true;
                    }
                    else if (Directory.Exists(pathDetails) && Directory.GetFiles(pathDetails, detail.Position + " - Дет.*.dwg", SearchOption.TopDirectoryOnly).Length != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
                orderPathDetails.Add(new OrderPathDetailsBindingModel { Order = order.Key });

                try
                {
                    if (Directory.Exists(Orders[0].Model.Path + @"\Чертежи\Детали DWG"))
                    {
                        orderPathDetails.Last().PathDWG = Orders[0].Model.Path + @"\Чертежи\Детали DWG";
                        orderPathDetails.Last().FindedDWG = true;
                    }
                    else if (Directory.Exists(Orders[0].Model.Path + @"\Чертежи\" + order.Key + @"\Детали DWG"))
                    {
                        orderPathDetails.Last().PathDWG = Orders[0].Model.Path + @"\Чертежи\" + order.Key + @"\Детали DWG";
                        orderPathDetails.Last().FindedDWG = true;
                    }
                    else
                    {
                        orderPathDetails.Last().PathDWG = "Не найдена папка деталировки";
                        orderPathDetails.Last().FindedDWG = false;
                    }
                }
                catch
                {
                    orderPathDetails.Last().PathDWG = "Ошибка прав доступа к папке с деталировкой";
                    orderPathDetails.Last().FindedDWG = false;
                }

                try
                {
                    if (Directory.Exists(Orders[0].Model.Path + @"\Чертежи\Детали PDF"))
                    {
                        orderPathDetails.Last().PathPDF = Orders[0].Model.Path + @"\Чертежи\Детали PDF";
                        orderPathDetails.Last().FindedPDF = true;
                    }
                    else if (Directory.Exists(Orders[0].Model.Path + @"\Чертежи\" + order.Key + @"\Детали PDF"))
                    {
                        orderPathDetails.Last().PathPDF = Orders[0].Model.Path + @"\Чертежи\" + order.Key + @"\Детали PDF";
                        orderPathDetails.Last().FindedPDF = true;
                    }
                    else
                    {
                        orderPathDetails.Last().PathPDF = "Не найдена папка деталировки";
                        orderPathDetails.Last().FindedPDF = false;
                    }
                }
                catch
                {
                    orderPathDetails.Last().PathPDF = "Ошибка прав доступа к папке с деталировкой";
                    orderPathDetails.Last().FindedPDF = false;
                }

                try
                {
                    if (Directory.Exists(Orders[0].Model.Path + @"\Чертежи\Детали DXF"))
                    {
                        orderPathDetails.Last().PathDXF = Orders[0].Model.Path + @"\Чертежи\Детали DXF";
                        orderPathDetails.Last().FindedDXF = true;
                    }
                    else if (Directory.Exists(Orders[0].Model.Path + @"\Чертежи\" + order.Key + @"\Детали DXF"))
                    {
                        orderPathDetails.Last().PathDXF = Orders[0].Model.Path + @"\Чертежи\" + order.Key + @"\Детали DXF";
                        orderPathDetails.Last().FindedDXF = true;
                    }
                    else
                    {
                        orderPathDetails.Last().PathDXF = "Не найдена папка деталировки";
                        orderPathDetails.Last().FindedDXF = false;
                    }
                }
                catch
                {
                    orderPathDetails.Last().PathDXF = "Ошибка прав доступа к папке с деталировкой";
                    orderPathDetails.Last().FindedDXF = false;
                }
            }

            return orderPathDetails;
        }
    }
}
