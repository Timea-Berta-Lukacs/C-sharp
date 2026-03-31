using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema1
{
    public class CurrencyConvertor : ICurrencyConvertor
    {
        public decimal ConvertToEuro(decimal amountInLei)
        {
            // Implementarea reală pentru conversia din Lei în Euro
            // Utilizați ratele de schimb valutar reale aici
            decimal conversionRate = 4.97m; // Exemplu de rată de conversie
            return amountInLei * conversionRate;
        }
    }
}
