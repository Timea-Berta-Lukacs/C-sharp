using System.Diagnostics;
using TAS_PROIECT.Automat;

namespace PerformanceTesting.cs
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

        public void PerformanceTest_AdaugaProduse()
        {
            //arrage
            var produse = new Produse(new List<Produse.Produs>());
            var stopwatch = new Stopwatch();

            //act
            stopwatch.Start();

            for (int i = 0; i < 5; i++)
            {
                produse.AdaugaProdus($"Produs{i}", 10.0m, 1);
            }

            stopwatch.Stop();

            //assert
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 10, "Adăugarea de produse a depășit limita de timp acceptată.");
        }

        [Test]
        [Category("pass")]

        public void PerformanceTest_CalculeazaValoareaTotala()
        {
            var produse = new Produse(new List<Produse.Produs>());

            // arrage
            for (int i = 0; i < 5; i++)
            {
                produse.AdaugaProdus($"Produs{i}", 10.0m, 1);
            }

            //act
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            decimal valoareTotala = produse.CalculeazaValoareaTotala();

            stopwatch.Stop();

            //assert
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 50, "Calculul valorii totale a depășit limita de timp acceptată.");
        }

    }
}