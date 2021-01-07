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

            foreach(var order in GroupOrder)
            {
                try
                {
                    if (Directory.Exists(Model.Path + @"\Чертежи\Детали DWG"))
                    {
                        orderPathDetails.Add(new OrderPathDetailsBindingModel {Order = order.Key, Path = Model.Path + @"\Чертежи\Детали DWG", Finded = true });
                    }
                    else if (Directory.Exists(Model.Path + @"\Чертежи\" + order.Key + @"\Детали DWG"))
                    {
                        orderPathDetails.Add(new OrderPathDetailsBindingModel { Order = order.Key, Path = Model.Path + @"\Чертежи\" + order.Key + @"\Детали DWG", Finded = true });
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
