using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS_PROIECT.Interfete;

namespace TAS_PROIECT.Automat
{
    public class VoucherStub : IVoucher
    {
        private decimal discountPercent;

        public VoucherStub(decimal discountPercent)
        {
            if (discountPercent <= 0 || discountPercent > 100)
            {
                throw new ArgumentException("Procentul de reducere trebuie să fie între 0 și 100.");
            }

            this.discountPercent = discountPercent;
        }

        public decimal AplicaVoucher(decimal pretInitial)
        {
            if (pretInitial < 0)
            {
                throw new ArgumentException("Prețul inițial nu poate fi negativ.");
            }

            decimal reducere = pretInitial * (discountPercent / 100);
            decimal pretFinal = pretInitial - reducere;

            return Math.Round(pretFinal, 2);
        }
    }
}
