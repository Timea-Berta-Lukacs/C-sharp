using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS_PROIECT.Interfete;

namespace TAS_PROIECT.Automat
{
    public class GestioneazaVanzari : IGestioneazaVanzari
    {
        private readonly IProduse produse;

        public GestioneazaVanzari(IProduse produse)
        {
            this.produse = produse ?? throw new ArgumentNullException(nameof(produse));
        }

        public void GestioneazaVanzare(string numeProdus, decimal pret, int cantitate)
        {
            if (cantitate <= 0)
            {
                throw new ArgumentException("Cantitatea trebuie să fie mai mare decât zero.");
            }

            if (pret <= 0)
            {
                throw new ArgumentException("Prețul trebuie să fie mai mare decât zero.");
            }

            if (cantitate > 5)
            {
                throw new ArgumentException("Cantitatea nu poate fi mai mare de 5.");
            }

            produse.AdaugaProdus(numeProdus, pret, cantitate);
        }

    }
}
