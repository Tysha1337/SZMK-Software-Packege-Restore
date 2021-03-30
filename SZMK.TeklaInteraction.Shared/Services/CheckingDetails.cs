using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SZMK.TeklaInteraction.Shared.BindingModels;
using SZMK.TeklaInteraction.Shared.Models;

namespace SZMK.TeklaInteraction.Shared.Services
{
    public class CheckingDetails
    {
        public List<OrderPathDetailsBindingModel> Check(Model Model)
        {
            List<OrderPathDetailsBindingModel> orderPathDetails = new List<OrderPathDetailsBindingModel>();

            var GroupOrder = Model.Drawings.GroupBy(p => p.Order);

            foreach (var order in GroupOrder)
            {
                orderPathDetails.Add(new OrderPathDetailsBindingModel { Order = order.Key });

                try
                {
                    if (Directory.Exists(Model.Path + @"\Чертежи\Детали DWG"))
                    {
                        orderPathDetails.Last().PathDWG = Model.Path + @"\Чертежи\Детали DWG";
                        orderPathDetails.Last().FindedDWG = true;
                    }
                    else if (Directory.Exists(Model.Path + @"\Чертежи\" + order.Key + @"\Детали DWG"))
                    {
                        orderPathDetails.Last().PathDWG = Model.Path + @"\Чертежи\" + order.Key + @"\Детали DWG";
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
                    if (Directory.Exists(Model.Path + @"\Чертежи\Детали PDF"))
                    {
                        orderPathDetails.Last().PathPDF = Model.Path + @"\Чертежи\Детали PDF";
                        orderPathDetails.Last().FindedPDF = true;
                    }
                    else if (Directory.Exists(Model.Path + @"\Чертежи\" + order.Key + @"\Детали PDF"))
                    {
                        orderPathDetails.Last().PathPDF = Model.Path + @"\Чертежи\" + order.Key + @"\Детали PDF";
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
                    if (Directory.Exists(Model.Path + @"\Чертежи\Детали DXF"))
                    {
                        orderPathDetails.Last().PathDXF = Model.Path + @"\Чертежи\Детали DXF";
                        orderPathDetails.Last().FindedDXF = true;
                    }
                    else if (Directory.Exists(Model.Path + @"\Чертежи\" + order.Key + @"\Детали DXF"))
                    {
                        orderPathDetails.Last().PathDXF = Model.Path + @"\Чертежи\" + order.Key + @"\Детали DXF";
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
