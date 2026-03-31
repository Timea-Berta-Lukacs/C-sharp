using TAS_PROIECT.Automat;
using TAS_PROIECT.Proiect;

namespace Spy
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

        public void ProceseazaVanzare_ApeleazaMetodaIProceseazaVanzare()
        {
            // Arrange
            var proceseazaVanzareSpy = new ProceseazaVanzareSpy();
            var automat = new Produse (new List<Produse.Produs>());

            // Act
            automat.ProceseazaVanzare(proceseazaVanzareSpy);

            // Assert
            Assert.IsTrue(proceseazaVanzareSpy.ProceseazaVanzareCalled);
        }

        [Test]
        [Category("pass")]

        public void CalculeazaPretFinalCuVoucher_AplicaVoucherApelat()
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());
            var voucherSpy = new VoucherSpy();

            // Act
            produse.CalculeazaPretFinalCuVoucher(voucherSpy, 50.0m);

            // Assert
            Assert.AreEqual(1, voucherSpy.AplicaVoucherCallCount, "Metoda AplicaVoucher ar trebui sã fie apelatã o singurã datã.");
        }

        [Test]
        [Category("fail")]

        public void CalculateCurrencyPrice_ConvertApelat()
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());
            var countryPriceSpy = new CountryPriceSpy();

            // Act
            produse.CalculateCurrencyPrice("euro", 50.0m, countryPriceSpy);

            // Assert
            Assert.AreEqual(2, countryPriceSpy.ConvertCallCount, "Metoda Convert ar trebui sã fie apelatã o singurã datã.");
        }
    }
}