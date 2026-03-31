using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS_PROIECT.Interfete;

namespace TAS_PROIECT.Proiect
{
    public class CountryPriceSpy : ICountryPrice
    {
        public int ConvertCallCount { get; private set; }

        public decimal Convert(decimal price)
        {
            ConvertCallCount++;
            return price * 0.9m; // de exemplu, o conversie fictivă la 90% din preț
        }
    }
}
