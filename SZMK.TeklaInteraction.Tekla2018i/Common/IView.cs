using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK.TeklaInteraction.Tekla2018i.Common
{
    public interface IView
    {
        void Show();
        void Close();

        void Info(String Message);
        void Warning(String Message);
        void Error(String Message);
    }
}
