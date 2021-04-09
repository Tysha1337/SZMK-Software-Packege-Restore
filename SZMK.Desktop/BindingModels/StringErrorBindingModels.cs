using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK.Desktop.BindingModels
{
    public class StringErrorBindingModels
    {
        public string Order { get; set; }
        public string List { get; set; }
        public string Error { get; set; }

        public string InfoView
        {
            get
            {
                return $"В заказе {Order}, Лист {List}";
            }
        }
    }
}
