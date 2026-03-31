using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema1
{
    public class CurrencyConvertorStub : ICurrencyConvertor
    {
        public decimal ConvertToEuro(decimal amountInEuro)
        {
            decimal conversionRate = 51.7m;
            decimal amountInLei = amountInEuro * conversionRate;
            return amountInLei;
        }
    }


}
