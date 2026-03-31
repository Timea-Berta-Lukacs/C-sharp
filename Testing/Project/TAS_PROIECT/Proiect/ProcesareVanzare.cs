using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS_PROIECT.Interfete;

namespace TAS_PROIECT.Automat
{
    public class ProceseazaVanzare : IProceseazaVanzare
    {
        private readonly Produse produse;

        public ProceseazaVanzare(Produse produse)
        {
            this.produse = produse ?? throw new ArgumentNullException(nameof(produse));
        }

        public void ProceseazaVanzarea(string numeProdus, decimal pret, int cantitate)
        {
            produse.AdaugaProdus(numeProdus, pret, cantitate);
        }
    }
}
