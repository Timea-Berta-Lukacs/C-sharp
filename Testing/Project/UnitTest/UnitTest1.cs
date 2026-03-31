using TAS_PROIECT.Automat;

namespace UnitTest
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

        public void GasesteNumeProdusDupaPret_DacaProdusulExista_ReturneazaNumeleProdusului()
        {
            // arrange
            var produse = new Produse(new List<Produse.Produs>
            {
                new Produse.Produs { Nume = "ProdusA", Pret = 10.0m, Cantitate = 2 },
                new Produse.Produs { Nume = "ProdusB", Pret = 15.0m, Cantitate = 1 },
                new Produse.Produs { Nume = "ProdusC", Pret = 20.0m, Cantitate = 3 }
            });

            // act
            string numeProdusGasit = produse.GasesteNumeProdusDupaPret(15.0m);

            // assert
            Assert.AreEqual("ProdusB", numeProdusGasit);
        }

        [Test]
        [Category("fail")]

        public void GasesteNumeProdusDupaPret_DacaProdusulNuExista_ReturneazaMesajulCorect()
        {
            // arrange
            var produse = new Produse(new List<Produse.Produs>
            {
                new Produse.Produs { Nume = "ProdusA", Pret = 10.0m, Cantitate = 2 },
                new Produse.Produs { Nume = "ProdusB", Pret = 15.0m, Cantitate = 1 },
                new Produse.Produs { Nume = "ProdusC", Pret = 20.0m, Cantitate = 3 }
            });

            // act
            string numeProdusGasit = produse.GasesteNumeProdusDupaPret(25.0m);

            // assert
            Assert.AreEqual("Nu s-a găsit produs cu prețul 25.0.", numeProdusGasit);
        }
    }
}