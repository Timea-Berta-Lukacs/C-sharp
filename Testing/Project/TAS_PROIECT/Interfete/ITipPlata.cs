using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS_PROIECT.Proiect;

namespace TAS_PROIECT.Interfete
{
    public interface ITipPlata
    {
        TipPlata.MetodaPlata Tip { get; }
        decimal SumaPlatita { get; }

        void EfectueazaPlata(decimal suma);
        string ToString();
    }

}
