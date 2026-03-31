using System;
using System.Collections.Generic;
using NUnit.Framework;
using TAS_PROIECT.Automat;


namespace IntegrationTesting
{
    [TestFixture]
    public class SelectareProduseIntegrationTests
    {
        private Dictionary<string, decimal> catalogProduse;

        [SetUp]
        public void Setup()
        {
            catalogProduse = new Dictionary<string, decimal>
        {
            { "Produs1", 10.50m },
            { "Produs2", 20.75m },
        };
        }

        [Test]
        [Category("pass")]

        public void TestGetPretProdus_DictonarReal()
        {
            // arrage
            var selectareProduse = new SelectareProduse(catalogProduse);

            // act
            decimal pretProdus1 = selectareProduse.GetPretProdus("Produs1");
            decimal pretProdus2 = selectareProduse.GetPretProdus("Produs2");

            // assert
            Assert.AreEqual(10.50m, pretProdus1);
            Assert.AreEqual(20.75m, pretProdus2);
        }

        [Test]
        [Category("pass")]

        public void TestGetPretProdusProdusLipsa_DictonarReal()
        {
            // arrage
            var selectareProduse = new SelectareProduse(catalogProduse);

            // act and assert
            Assert.Throws<ArgumentException>(() => selectareProduse.GetPretProdus("Produs3"));
        }

       
    }
}
