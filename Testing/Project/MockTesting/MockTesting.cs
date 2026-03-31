using TAS_PROIECT.Automat;
using Moq;
using TAS_PROIECT.Interfete;

namespace MockTesting
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
        public void ProceseazaVanzare_CuMockIProceseazaVanzare_AplicaCorect()
        {
            // Arrange
            var mockProceseazaVanzare = new Mock<IProceseazaVanzare>();
            var produse = new Produse(new List<Produse.Produs>());

            // Act
            produse.ProceseazaVanzare(mockProceseazaVanzare.Object);

            // Assert
            mockProceseazaVanzare.Verify(
            mock => mock.ProceseazaVanzarea(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>()),Times.Once);
        }

        [Test]
        [Category("pass")]

        public void IncaseazaBani_UtilizareMockIIncaseazaBani_AplicaCorect1()
        {
            // Arrange
            var mockIncaseazaBani = new Mock<IIncaseazaBani>();
            var produse = new Produse(new List<Produse.Produs>());

            // Act
            produse.IncaseazaBani(mockIncaseazaBani.Object, 50.0m);

            // Assert
            mockIncaseazaBani.Verify(
                mock => mock.Incaseaza(It.IsAny<decimal>()),
                Times.Once);

            mockIncaseazaBani.Verify(
                mock => mock.Incaseaza(It.Is<decimal>(suma => suma == 50.0m)),Times.Once);
        }

        [Test]
        [Category("pass")]
        public void IncaseazaBani_UtilizareMockIIncaseazaBani_AplicaCorect2()
        {
            // Arrange
            var mockIncaseazaBani = new Mock<IIncaseazaBani>();
            var produse = new Produse(new List<Produse.Produs>());

            // Act
            produse.IncaseazaBani(mockIncaseazaBani.Object, 50.0m);

            // Assert         
            mockIncaseazaBani.Verify(
            mock => mock.Incaseaza(It.Is<decimal>(suma => suma == 50.0m)),Times.Once);
        }

        [Test]
        [Category("pass")]

        public void TestProceseazaPlata_Cash_SumaValida()
        {
            // arrage
            var mockTipPlata = new Mock<ITipPlata>();
            var produse = new Produse(new List<Produse.Produs>());

            // act
            produse.ProceseazaPlata(mockTipPlata.Object, 2.00m);

            // assert
            mockTipPlata.Verify(tp => tp.EfectueazaPlata(2.00m), Times.Once);
        }

        [Test]
        [Category("pass")]

        public void TestProceseazaPlata_Card_SumaValida()
        {
            // arrage
            var mockTipPlata = new Mock<ITipPlata>();
            var produse = new Produse(new List<Produse.Produs>());

            // act
            produse.ProceseazaPlata(mockTipPlata.Object, 3.00m);

            // assert
            mockTipPlata.Verify(tp => tp.EfectueazaPlata(3.00m), Times.Once);
        }

        [Test]
        [Category("pass")]

        public void TestProceseazaPlata_SumaZero_AruncareExceptie()
        {
            // arrage
            var mockTipPlata = new Mock<ITipPlata>();
            var produse = new Produse(new List<Produse.Produs>());

            // act and assert
            Assert.Throws<ArgumentException>(() => produse.ProceseazaPlata(mockTipPlata.Object, 0));
        }

        [Test]
        [Category("pass")]

        public void TestProceseazaPlata_SumaNegativa_AruncareExceptie()
        {
            // arrage
            var mockTipPlata = new Mock<ITipPlata>();
            var produse = new Produse(new List<Produse.Produs>());

            // act and assert
            Assert.Throws<ArgumentException>(() => produse.ProceseazaPlata(mockTipPlata.Object, -1.00m));
        }
    }
}