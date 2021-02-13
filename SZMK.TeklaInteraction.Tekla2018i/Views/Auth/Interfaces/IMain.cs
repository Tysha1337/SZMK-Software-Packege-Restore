using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SZMK.TeklaInteraction.Tekla2018i.Common;

namespace SZMK.TeklaInteraction.Tekla2018i.Views.Auth.Interfaces
{
    interface IMain : IView
    {
        void Hide();

        event Action StartedProgram;
    }
}
