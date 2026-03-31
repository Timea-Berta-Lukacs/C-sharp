using NUnit.Framework;
using Moq;
using System.Security.Principal;
using Tema1;
using System.Diagnostics.Eventing.Reader;

namespace BankFeatures
{
    [TestFixture]
    public class AccountTests
    {
        private Account account;
        bool check = false;

        [SetUp]
        public void Setup()
        {
            //arrange
            account = new Account(0, "John");
            account.Deposit(1000, check);
        }

        [Test]
        [Category("pass")]
        [TestCase(5000, 10, 4951)]
        public void TransferFundsFromEuroAmount_ValidAmount_TransferSuccessful_Mock(decimal InitValue, decimal transferAmountInEuro, decimal expectedBalance)
        {
            // Arrange
            Account sourceAccount = new Account(InitValue, "John");
            Account destinationAccount = new Account(InitValue, "Mary");

            // Creeazã un obiect Mock pentru ICurrencyConvertor
            var convertorMock = new Mock<ICurrencyConvertor>();
            convertorMock.Setup(c => c.ConvertFromEuro(It.IsAny<decimal>())).Returns((decimal amountInEuro) => amountInEuro * 4.9m);

            // Act
            sourceAccount.TransferFundsFromEuroAmount(destinationAccount, transferAmountInEuro, convertorMock.Object);

            // Assert
            Assert.AreEqual(expectedBalance, sourceAccount.GetCurrentBalance());
        }


        [Test]
        public void TestLogTransaction()
        {
            // arrage
            var mockLogger = new Mock<ITransactionLogger>();
            var account = new Account(initialBalance: 500, holderName: "John", logger: mockLogger.Object);
            bool check = true;

            // act
            account.Deposit(100, check);

            mockLogger.Verify(logger => logger.LogTransaction("Deposited 100 units"), Times.Once);
        }
    }
}