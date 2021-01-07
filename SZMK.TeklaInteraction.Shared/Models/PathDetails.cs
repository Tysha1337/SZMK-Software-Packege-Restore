using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK.TeklaInteraction.Shared.Models
{
    public class PathDetails
    {
        public long Id { get; set; }
        public DateTime DateCreate { get; set; }
        public string Path { get; set; }
        public List<Drawing> Drawings { get; set; }
    }
}
