using TAS_PROIECT.Automat;

namespace Coverage
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
        [TestCase("Produs2", 10.0, 6)]
        [TestCase("Produs3", -5.0, 2)]
        [TestCase("Produs4", 10.123, 3)]
        [TestCase("Produs5", 10.0, 0)]
        public void AdaugaProdus_ControlFlowTest1(string nume, decimal pret, int cantitate)
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());

            // Act & Assert
            Assert.Throws<ArgumentException>(() => produse.AdaugaProdus(nume, pret, cantitate));
        }

        [Test]
        [Category("pass")]
        [TestCase("Produs1", 10.0, 3)]
        public void AdaugaProdus_ControlFlowTest2(string nume, decimal pret, int cantitate)
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());

            // Act & Assert
            Assert.DoesNotThrow(() => produse.AdaugaProdus(nume, pret, cantitate));

        }

        [Test]
        public void CalculeazaValoareaProdusului_CalculCorect()
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());

            // Act
            var valoareProdus = produse.CalculeazaValoareaProdusului("Produs1", 5.0m, 4);

            // Assert
            Assert.AreEqual(20.0m, valoareProdus);
        }
    }
}