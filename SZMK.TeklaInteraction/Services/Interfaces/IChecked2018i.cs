using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK.TeklaInteraction.Services.Interfaces
{
    interface IChecked2018i
    {
        event Action<string> Load;
        void Start();

        void CheckedAsync();
        void Checked();

        void Reset();

        void Stopped();

        void Error(string Message);
    }
}
