using TAS_PROIECT.Interfete;

namespace TAS_PROIECT.Automat
{
    public class Produse : IProduse
    {
        private const int MaxProduse = 20;

        public class Produs
        {
            public string Nume { get; set; }
            public decimal Pret { get; set; }
            public int Cantitate { get; set; }
        }
        public List<Produs> ListaProduse { get; set; } = new List<Produs>();

        public Produse(List<Produs> produseInitiale)
        {
            ListaProduse = produseInitiale ?? new List<Produs>();
        }
        public List<Produs> GetListaProduse()
        {
            return ListaProduse;
        }

        public void AdaugaProdus(string nume, decimal pret, int cantitate)
        {
            if (ListaProduse.Count + cantitate > MaxProduse)
            {
                throw new InvalidOperationException($"Nu se poate adăuga produsul. Numărul maxim de produse permise este {MaxProduse}.");
            }

            if (cantitate <= 5 && cantitate > 0)
            {
                if (pret > 0)
                {
                    if (CountDecimalPlaces(pret) > 2)
                    {
                        throw new ArgumentException("Prețul nu poate avea mai mult de 2 cifre după virgulă.");
                    }

                    ListaProduse.Add(new Produs { Nume = nume, Pret = pret, Cantitate = cantitate });
                }
                else
                {
                    throw new ArgumentException("Prețul trebuie să fie mai mare decât zero.");
                }
            }
            else
            {
                throw new ArgumentException("Cantitatea nu poate fi mai mare de 5 sau mai mică sau egală cu 0.");
            }
        }

        private int CountDecimalPlaces(decimal value)
        {
            return BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
        }

        public decimal CalculeazaValoareaTotala()
        {
            return ListaProduse.Sum(produs => produs.Pret * produs.Cantitate);
        }

        public string GasesteNumeProdusDupaPret(decimal pretCautat)
        {
            Produs produsGasit = ListaProduse.FirstOrDefault(produs => produs.Pret == pretCautat);

            return produsGasit != null ? produsGasit.Nume : $"Nu s-a găsit produs cu prețul {pretCautat}.";
        }
        
        public decimal CalculeazaPretFinalCuVoucher(IVoucher voucher, decimal pretInitial)
        {
            if (pretInitial < 0)
            {
                throw new ArgumentException("Prețul inițial nu poate fi negativ.");
            }

            decimal valoareTotala = CalculeazaValoareaTotala();
            decimal pretFinal = valoareTotala + voucher.AplicaVoucher(pretInitial - valoareTotala);

            return Math.Round(pretFinal, 2);
        }

        public void ProceseazaVanzare(IProceseazaVanzare proceseazaVanzare)
        {
            proceseazaVanzare.ProceseazaVanzarea("ProdusExemplu", 15.0m, 3);
        }

        public void IncaseazaBani(IIncaseazaBani incaseazaBani, decimal suma)
        {
            incaseazaBani.Incaseaza(suma);
        }

        public void ProceseazaVanzareCuGestioneazaVanzari(IGestioneazaVanzari gestioneazaVanzari, string numeProdus, decimal pret, int cantitate)
        {
            gestioneazaVanzari.GestioneazaVanzare(numeProdus, pret, cantitate);
        }

        public decimal CalculeazaValoareaProdusului(string numeProdus, decimal pretUnitar, int cantitate)
        {
            decimal valoareProdus = pretUnitar * cantitate;
            return valoareProdus;
        }

        public decimal CalculateCurrencyPrice(string moneyType, decimal price, ICountryPrice CountryPrice) 
        {
            if(price< 0)
            {
                throw new ArgumentException("Pretul trebuie sa fie mai mare decat 0.");
            }

            if (string.Equals(moneyType, "euro", StringComparison.OrdinalIgnoreCase) || string.Equals(moneyType, "dollar", StringComparison.OrdinalIgnoreCase) || string.Equals(moneyType, "pound", StringComparison.OrdinalIgnoreCase))
            {
                return CountryPrice.Convert(price);
            }
            else
            {
                throw new ArgumentException("Tipul de monedă nu este acceptat.", nameof(moneyType));
            }
        }

        public void ProceseazaPlata(ITipPlata tipPlata, decimal suma)
        {
            if (suma <= 0)
            {
                throw new ArgumentException("Suma trebuie să fie mai mare decât zero pentru a efectua plata.");
            }

            tipPlata.EfectueazaPlata(suma);
        }
    }
}