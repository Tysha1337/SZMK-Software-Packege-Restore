using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK.Desktop.Models
{
    public class Comment
    {
        public long ID { get; set; }
        public DateTime DateCreate { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
    }
}
