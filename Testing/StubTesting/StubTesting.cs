using BankFeatures;
using Tema1;
using NUnit.Framework;
using System.Security.Principal;
using System.Diagnostics.Eventing.Reader;

namespace BankFeatures
{
    [TestFixture]
    public class AccountTests
    {

        [Test]
        [Category("pass")]
        [TestCase(5000, 10, 4951)]
        public void TransferFundsFromEuroAmount_ValidAmount_TransferSuccessful(decimal InitValue, decimal transferAmountInEuro, decimal expectedBalance)
        {
            // Arrange
            Account sourceAccount = new Account(InitValue, "John");
            Account destinationAccount = new Account(InitValue, "Mary");

            // Utilizãm stub-ul pentru conversie valutarã
            ICurrencyConvertor convertor = new CurrencyConvertorStub(4.9m);

            // Act
            sourceAccount.TransferFundsFromEuroAmount(destinationAccount, transferAmountInEuro, convertor);

            // Assert
            Assert.AreEqual(expectedBalance, sourceAccount.GetCurrentBalance());
        }
    }
}