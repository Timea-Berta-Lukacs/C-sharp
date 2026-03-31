using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS_PROIECT.Interfete;

namespace TAS_PROIECT.Automat
{
    public class SelectareProduse : ISelectareProduse
    {
        private Dictionary<string, decimal> catalogProduse;

        public SelectareProduse(Dictionary<string, decimal> catalogProduse)
        {
            this.catalogProduse = catalogProduse;
        }
        public decimal GetPretProdus(string numeProdus)
        {
            if (catalogProduse.ContainsKey(numeProdus))
            {
                return catalogProduse[numeProdus];
            }
            else
            {
                throw new ArgumentException("Produsul nu există în catalog.");
            }
        }
    }

}
