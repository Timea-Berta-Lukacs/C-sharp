using NUnit.Framework;
using Moq;
using System.Security.Principal;
using Tema1;

namespace BankFeatures
{
    [TestFixture]
    public class AccountTests
    {
        private Account account;
        private Account recipientAccount;

        [SetUp]
        public void Setup()
        {
            //arrange
            account = new Account(0, "John");
            account.Deposit(1000);
        }

        [Test]
        [Category("fail")]
        [TestCase(10001, 11001)]
        [TestCase(0, 1000)]
        [TestCase(-1, 999)]
        public void DepositTestFail(decimal depositAmount, decimal expectedBalance)
        {
            //act
            account.Deposit(depositAmount);
            //assert
            Assert.AreEqual(expectedBalance, account.GetCurrentBalance());
        }

        [Test]
        [Category("pass")]
        [TestCase(10000, 11000)]
        [TestCase(500, 1500)]
        public void DepositTestPass(decimal depositAmount, decimal expectedBalance)
        {
            //act
            account.Deposit(depositAmount);
            //assert
            Assert.AreEqual(expectedBalance, account.GetCurrentBalance());
        }

        //Check if the Deposit method throws an InvalidOperationException
        [Test]
        [Category("exceptionTest")]
        [TestCase(10000, 11000)]
        [TestCase(10001, 11001)]
        [TestCase(0, 1000)]
        [TestCase(-1, 999)]
        [TestCase(500, 1500)]
        public void DepositValidAmountException(decimal depositAmount, decimal expectedBalance)
        {
            Assert.Throws<InvalidOperationException>(() => account.Deposit(depositAmount));
        }

        [Test]
        [Category("fail")]
        [TestCase(991, 9)]
        [TestCase(0, 1000)]
        [TestCase(-1, 1001)]
        [TestCase(1001, -1)]

        public void WithDrawTestFail(decimal withDrawAmount, decimal expectedBalance)
        {
            //act
            //Account initValue = new Account(InitValue, "John"); 
            account.Withdraw(withDrawAmount);
            //assert
            Assert.AreEqual(expectedBalance, account.GetCurrentBalance());
        }

        [Test]
        [Category("pass")]
        [TestCase(900, 100)]
        [TestCase(990, 10)]
        [TestCase(1, 999)]

        public void WithDrawTestPass(decimal withDrawAmount, decimal expectedBalance)
        {
            //act
            //Account initValue = new Account(InitValue, "John");
            account.Withdraw(withDrawAmount);
            //assert
            Assert.AreEqual(expectedBalance, account.GetCurrentBalance());
        }

        [Test]
        [Category("fail")]
        [TestCase(5000, -1, 4999)]
        [TestCase(5000, 0, 5000)]
        [TestCase(5000, 4991, 9991)]
        public void TransferTestFail1(decimal InitValue, decimal transferAmount, decimal expectedBalance)
        {
            //act
            Account account = new Account();
            account.Deposit(InitValue);
            Account recipientAccount = new Account();
            recipientAccount.Deposit(InitValue);

            account.Transfer(recipientAccount, transferAmount);

            //assert
            Assert.AreEqual(expectedBalance, recipientAccount.GetCurrentBalance());
        }

        [Test]
        [Category("fail")]
        [TestCase(5000, 200, 5200)]
        public void TransferTestFail2(decimal InitValue, decimal transferAmount, decimal expectedBalance)
        {
            //act
            Account account = new Account();
            account.Deposit(InitValue);

            account.Transfer(recipientAccount, transferAmount);
        }

        [Test]
        [Category("pass")]
        [TestCase(5000, 1, 5001)]
        [TestCase(5000, 4990, 9990)]
        [TestCase(5000, 2500, 7500)]
        public void TransferTestPass(decimal InitValue, decimal transferAmount, decimal expectedBalance)
        {
            //act
            Account account = new Account();
            account.Deposit(InitValue);
            Account recipientAccount = new Account();
            recipientAccount.Deposit(InitValue);

            account.Transfer(recipientAccount, transferAmount);

            //assert
            Assert.AreEqual(expectedBalance, recipientAccount.GetCurrentBalance());
        }

        [Test]
        [Category("fail")]
        [TestCase()]
        public void CloseAccountTestPass()
        {
            // Setup
            Account account = new Account(1000, "John"); // Cont cu sold nenul
            account.Deposit(1000);

            // Act & Assert (Expecting an exception)
            Assert.Throws<InvalidOperationException>(() => account.CloseAccount());
        }

        [Test]
        [Category("pass")]
        public void CloseAccountTestFail()
        {
            // Setup
            Account account = new Account(0, "John"); // Cont cu sold nul
            account.Deposit(0);

            // Act & Assert (No exception expected)
            Assert.DoesNotThrow(() => account.CloseAccount());
        }

        [Test]
        [Category("pass")]
        public void ApplyInterestTestPass()
        {
            // Setup
            Account account = new Account(1000, "John"); // Cont cu un sold de 1000
            decimal interestRate = 0.05M; // Rata dobânzii de 5%

            // Act
            account.ApplyInterest(interestRate);

            // Assert
            decimal expectedBalance = 1000 * (1 + interestRate); // Calculăm soldul așteptat cu dobânda aplicată
            Assert.AreEqual(expectedBalance, account.GetCurrentBalance());
        }

        [Test]
        [Category("pass")]
        public void ChangeAccountHolderNameTestPass()
        {
            // Setup
            Account account = new Account(1000, "John");
            string newAccountHolderName = "Alice";

            // Act
            account.ChangeAccountHolderName(newAccountHolderName);

            // Assert
            Assert.AreEqual(newAccountHolderName, account.GetAccountHolderName());
        }

        [Test]
        [Category("fail")]
        public void ChangeAccountHolderNameTestFail()
        {
            // Setup
            Account account = new Account(1000, "John");
            string invalidAccountHolderName = null;

            //act
            account.ChangeAccountHolderName(invalidAccountHolderName);
        }

        [Test]
        [Category("pass")]
        [TestCase(5000, 10, 4951)]
        public void TransferFundsFromEuroAmount_ValidAmount_TransferSuccessful(decimal InitValue, decimal transferAmountInEuro, decimal expectedBalance)
        {
            // Arrange
            Account sourceAccount = new Account(InitValue, "John");
            Account destinationAccount = new Account(InitValue, "Mary");

            // Utilizăm stub-ul pentru conversie valutară
            ICurrencyConvertor convertor = new CurrencyConvertorStub(4.9m);

            //  sourceAccount.CurrencyConvertor = convertor;


            // Act
            sourceAccount.TransferFundsFromEuroAmount(destinationAccount, transferAmountInEuro, convertor);

            // Assert
            Assert.AreEqual(expectedBalance, sourceAccount.GetCurrentBalance());
            // Assert.AreEqual(expectedBalance, destinationAccount.GetCurrentBalance());
        }



        [Test]
        [Category("pass")]
        [TestCase(5000, 10, 4951)]
        public void TransferFundsFromEuroAmount_ValidAmount_TransferSuccessful_Mock(decimal InitValue, decimal transferAmountInEuro, decimal expectedBalance)
        {
            // Arrange
            Account sourceAccount = new Account(InitValue, "John");
            Account destinationAccount = new Account(InitValue, "Mary");

            // Creează un obiect Mock pentru ICurrencyConvertor
            var convertorMock = new Mock<ICurrencyConvertor>();
            convertorMock.Setup(c => c.ConvertFromEuro(It.IsAny<decimal>())).Returns((decimal amountInEuro) => amountInEuro * 4.9m);

            // Act
            sourceAccount.TransferFundsFromEuroAmount(destinationAccount, transferAmountInEuro, convertorMock.Object);

            // Assert
            Assert.AreEqual(expectedBalance, sourceAccount.GetCurrentBalance());
        }

    }
}