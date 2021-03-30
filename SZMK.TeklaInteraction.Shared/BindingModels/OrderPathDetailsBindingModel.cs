using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK.TeklaInteraction.Shared.BindingModels
{
    public class OrderPathDetailsBindingModel
    {
        public string Order { get; set; }
        public string PathDWG { get; set; }
        public string PathPDF { get; set; }
        public string PathDXF { get; set; }
        public bool FindedDWG { get; set; }
        public bool FindedPDF { get; set; }
        public bool FindedDXF { get; set; }
    }
}
