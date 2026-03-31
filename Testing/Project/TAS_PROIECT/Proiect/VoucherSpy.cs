using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS_PROIECT.Interfete;

namespace TAS_PROIECT.Proiect
{
    public class VoucherSpy : IVoucher
    {
        public int AplicaVoucherCallCount { get; private set; }

        public decimal AplicaVoucher(decimal pretInitial)
        {
            AplicaVoucherCallCount++;
            return 5.0m;
        }
    }
}
