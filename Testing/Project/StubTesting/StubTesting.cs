using TAS_PROIECT.Automat;
using TAS_PROIECT.Interfete;

namespace StubTesting
{
    public class Tests
    {
        private Produse produse;
        [SetUp]
        public void Setup()
        {
            List<Produse.Produs> produseInitiale = new List<Produse.Produs>
        {
            new Produse.Produs { Nume = "Biscuiti", Pret = 10.99m, Cantitate = 2 }
        };
            produse = new Produse(produseInitiale);
        }

        [Test]
        [Category("pass")]
        [TestCase(120.0, 102.0)]
        public void VoucherTestPass(decimal pretInit, decimal expect)
        {
            //arrage
            var produse = new Produse(new List<Produse.Produs>());

            IVoucher voucher = new VoucherStub(15);

            //act
            decimal pretFinal = produse.CalculeazaPretFinalCuVoucher(voucher, pretInit);
            produse.AdaugaProdus("prod1", pretFinal, 1);
            List<Produse.Produs> listaProduse = produse.GetListaProduse();


            //assert
            Assert.AreEqual(expect, listaProduse[0].Pret);
        }

        [Test]
        [Category("fail")]
        [TestCase(0, 0)]
        public void VoucherTestFail1(decimal pretInit, decimal expect)
        {
            //arrage
            var produse = new Produse(new List<Produse.Produs>());

            IVoucher voucher = new VoucherStub(15);

            //act
            decimal pretFinal = produse.CalculeazaPretFinalCuVoucher(voucher, pretInit);
            produse.AdaugaProdus("prod1", pretFinal, 1);
            List<Produse.Produs> listaProduse = produse.GetListaProduse();


            //assert
            Assert.AreEqual(expect, listaProduse[0].Pret);
        }

        [Test]
        [Category("fail")]
        [TestCase(120.0, 102.0, 0)]
        [TestCase(120.0, 102.0, -1)]
        public void VoucherTestFail2(decimal pretInit, decimal expect, decimal ProcentVoucher)
        {
            //arrage
            var produse = new Produse(new List<Produse.Produs>());

            IVoucher voucher = new VoucherStub(ProcentVoucher);

            //act
            decimal pretFinal = produse.CalculeazaPretFinalCuVoucher(voucher, pretInit);
            produse.AdaugaProdus("prod1", pretFinal, 1);
            List<Produse.Produs> listaProduse = produse.GetListaProduse();

            //assert
            Assert.AreEqual(expect, listaProduse[0].Pret);
        }

        [Test]
        [Category("pass")]
        [TestCase(4.97, "euro", 12.50, 2.52)]
        public void TestConversionPass(decimal rataConversie, string moneyType, decimal pretInLei, decimal expect)
        {
            //arrage
            var produse = new Produse(new List<Produse.Produs>());
            ICountryPrice ContryPrice = new CountryPriceStub(rataConversie);

            //act
            decimal pret = produse.CalculateCurrencyPrice(moneyType, pretInLei, ContryPrice);
            produse.AdaugaProdus("prod1", pret, 1);
            List<Produse.Produs> listaProduse = produse.GetListaProduse();

            //assert
            Assert.AreEqual(expect, listaProduse[0].Pret);
        }

        [Test]
        [Category("fail")]
        [TestCase(4.97, "forint", 12.50, 2.52)]
        public void TestConversionFail(decimal rataConversie, string moneyType, decimal pretInLei, decimal expect)
        {
            //arrage
            var produse = new Produse(new List<Produse.Produs>());
            ICountryPrice ContryPrice = new CountryPriceStub(rataConversie);

            //act
            decimal pret = produse.CalculateCurrencyPrice(moneyType, pretInLei, ContryPrice);
            produse.AdaugaProdus("prod1", pret, 1);
            List<Produse.Produs> listaProduse = produse.GetListaProduse();

            //assert
            Assert.AreEqual(expect, listaProduse[0].Pret);
        }
    }
}