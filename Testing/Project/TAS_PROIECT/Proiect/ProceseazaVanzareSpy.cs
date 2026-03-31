using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS_PROIECT.Interfete;

namespace TAS_PROIECT.Automat
{
    public class ProceseazaVanzareSpy : IProceseazaVanzare
    {
        public bool ProceseazaVanzareCalled { get; private set; } = false;

        public void ProceseazaVanzarea(string numeProdus, decimal pret, int cantitate)
        {
            ProceseazaVanzareCalled = true;
        }
    }

}
