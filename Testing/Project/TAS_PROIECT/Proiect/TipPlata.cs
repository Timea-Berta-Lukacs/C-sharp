using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS_PROIECT.Interfete;

namespace TAS_PROIECT.Proiect
{
    public class TipPlata : ITipPlata
    {
        public enum MetodaPlata
        {
            Cash,
            Card
        }

        public MetodaPlata Tip { get; private set; }
        public decimal SumaPlatita { get; private set; }

        public TipPlata(MetodaPlata tip)
        {
            Tip = tip;
            SumaPlatita = 0;
        }

        public void EfectueazaPlata(decimal suma)
        {
            if (suma <= 0)
            {
                throw new ArgumentException("Suma trebuie să fie mai mare decât zero.");
            }

            SumaPlatita = suma;
        }

        public override string ToString()
        {
            return $"Tip de plată: {Tip}, Suma plătită: {SumaPlatita:C}";
        }
    }

}
