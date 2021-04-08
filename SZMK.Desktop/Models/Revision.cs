using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK.Desktop.Models
{
    public class Revision
    {
        public long ID { get; set; }
        public DateTime DateCreate { get; set; }
        public string CreatedBy { get; set; }
        public string Information { get; set; }
        public string Description { get; set; }
        public string LastApptovedBy { get; set; }
    }
}
