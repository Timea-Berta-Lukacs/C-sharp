using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema1
{
    public class CurrencyConvertorStub : ICurrencyConvertor
    {
        private readonly decimal conversionRate;

        public CurrencyConvertorStub(decimal rate)
        {
            this.conversionRate = rate;
        }
        public decimal ConvertFromEuro(decimal amountInEuro)
        {
            return amountInEuro * conversionRate;
        }
    }


}
