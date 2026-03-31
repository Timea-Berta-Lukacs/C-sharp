using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS_PROIECT.Automat;

namespace TAS_PROIECT.Interfete
{
    public interface IProduse
    {
        List<Produse.Produs> GetListaProduse();
        void AdaugaProdus(string nume, decimal pret, int cantitate);
        decimal CalculeazaValoareaTotala();
    }

    public class Produs
    {
        public string Nume { get; set; }
        public decimal Pret { get; set; }
        public int Cantitate { get; set; }
    }
}
