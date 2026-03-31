using TAS_PROIECT.Automat;

namespace Dummy
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
        public void AdaugaProdus_LaProduse_DummyTest()
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());
            var dummyProduct = new Produse.Produs { Nume = "DummyProduct", Pret = 10.0m, Cantitate = 1 };

            // Act
            produse.AdaugaProdus(dummyProduct.Nume, dummyProduct.Pret, dummyProduct.Cantitate);
            var listaProduse = produse.GetListaProduse();

            // Assert
            Assert.NotNull(listaProduse);
            Assert.AreEqual(1, listaProduse.Count);

            var produsAdaugat = listaProduse[0];
            Assert.AreEqual(dummyProduct.Nume, produsAdaugat.Nume);
            Assert.AreEqual(dummyProduct.Pret, produsAdaugat.Pret);
            Assert.AreEqual(dummyProduct.Cantitate, produsAdaugat.Cantitate);
        }

        [Test]
        [Category("pass")]
        public void AdaugaProdus_LaProduse_CantitateMaximaPermis()
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());

            // Act
            produse.AdaugaProdus("Produs1", 10.0m, 5);
            produse.AdaugaProdus("Produs2", 20.0m, 5);
            produse.AdaugaProdus("Produs3", 30.0m, 5);

            var listaProduse = produse.GetListaProduse();

            // Assert
            Assert.AreEqual(3, listaProduse.Count);
            Assert.Pass("Testul a fost executat cu succes.");
        }

        [Test]
        [Category("pass")]
        public void AdaugaProdus_LaProduse_CantitateMaiMareDecatPermis()
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());

            // Act și Assert
            Assert.Throws<ArgumentException>(() => produse.AdaugaProdus("Produs1", 10.0m, 6));
        }
    }
}