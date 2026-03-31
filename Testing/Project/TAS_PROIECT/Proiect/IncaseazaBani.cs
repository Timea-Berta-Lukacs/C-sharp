using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS_PROIECT.Automat
{
    public class IncaseazaBani
    {
        private decimal totalIncasat;

        public IncaseazaBani()
        {
            totalIncasat = 0;
        }

        public decimal TotalIncasat
        {
            get { return totalIncasat; }
        }

        public void Incaseaza(decimal suma)
        {
            if (suma < 0)
            {
                throw new ArgumentException("Suma încasată nu poate fi negativă.");
            }

            totalIncasat += suma;
        }
    }
}
