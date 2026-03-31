using System.Runtime.CompilerServices;
using TAS_PROIECT.Automat;
using System.Collections.Generic;


namespace Domain
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
        [TestCase("Produs1", 1, 1.2, 1)]
        [TestCase("Produs2", 5, 3.0, 5)]
        [TestCase("Produs3", 3, 15.8, 3)]
        public void TestCantitatePass(string numeProd, int cantitateAdaugata, decimal pretul, int expected)
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());

            //act
            produse.AdaugaProdus(numeProd, pretul, cantitateAdaugata);
            List<Produse.Produs> listaProduse = produse.GetListaProduse();

            //assert
            Assert.AreEqual(expected, listaProduse[0].Cantitate);
        }

        [Test]
        [Category("fail")]
        [TestCase("Produs1", 0, 1.2, 0)]
        [TestCase("Produs2", 6, 3.0, 6)]
        public void TestCantitateFail(string numeProd, int cantitateAdaugata, decimal pretul, int expected)
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());

            //act
            produse.AdaugaProdus(numeProd, pretul, cantitateAdaugata);
            List<Produse.Produs> listaProduse = produse.GetListaProduse();

            //assert
            Assert.AreEqual(expected, listaProduse[0].Cantitate);
        }

        [Test]
        [Category("pass")]
        [TestCase("Produs1", 1, 1.23, 1.23)]
        [TestCase("Produs2", 2, 100.14, 100.14)]
        public void TestPretPass(string numeProd, int cantitateAdaugata, decimal pretul, decimal expected)
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());

            //act
            produse.AdaugaProdus(numeProd, pretul, cantitateAdaugata);
            List<Produse.Produs> listaProduse = produse.GetListaProduse();

            //assert
            Assert.AreEqual(expected, listaProduse[0].Pret);
        }

        [Test]
        [Category("fail")]
        [TestCase("Produs1", 1, 0, 0)]
        [TestCase("Produs2", 2, 12.234, 12.234)]
        public void TestPretFail(string numeProd, int cantitateAdaugata, decimal pretul, decimal expected)
        {
            // Arrange
            var produse = new Produse(new List<Produse.Produs>());

            //act
            produse.AdaugaProdus(numeProd, pretul, cantitateAdaugata);
            List<Produse.Produs> listaProduse = produse.GetListaProduse();

            //assert
            Assert.AreEqual(expected, listaProduse[0].Pret);
        }
    }
}