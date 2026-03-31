using Tema1;

namespace BankFeatures
{
    public class Account
    {
        private decimal CurrentBalance;
        private string AccountHolderName;
        private decimal maxDepositable=10000;
        private decimal minBalance = 10;
        string newName;

        public Account(decimal initialBalance, string holderName)
        {
            if (initialBalance < 0)
            {
                throw new ArgumentException("Initial balance cannot be negative.");
            }
            CurrentBalance = initialBalance;
            AccountHolderName = holderName;
        }

        public Account()
        {
        }

        public decimal GetCurrentBalance()
        {
            return CurrentBalance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to deposit cannot be negative or zero.");
            }
            if (amount > maxDepositable)
            {
                throw new InvalidOperationException("The added amount exceeds the maximum allowed.");
            }
            CurrentBalance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to withdraw cannot be negative or zero.");
            }
            if (amount > CurrentBalance - 10)
            {
                throw new InvalidOperationException("Insufficient funds for withdrawal.");
            }           
            CurrentBalance -= amount;
        }


        public void Transfer(Account recipient, decimal amount)
        {
            if (recipient == null)
            {
                throw new ArgumentNullException("Recipient account cannot be null.");
            }
            if (amount <= 0)
            {
                throw new ArgumentException("Transfer amount cannot be negative or zero.");
            }
            if (amount > CurrentBalance - 10)
            {
                throw new InvalidOperationException("Insufficient funds for transfer.");
            }

            Withdraw (amount);
            recipient.Deposit(amount);
        }

        public void CloseAccount()
        {
            if (CurrentBalance != 0)
            {
                throw new InvalidOperationException("Cannot close an account with a non-zero balance.");
            }
        }

        public void ApplyInterest(decimal interestRate)
        {
            decimal interest = CurrentBalance * interestRate;
            CurrentBalance += interest;
        }

        public void ChangeAccountHolderName(string newName)
        {
            if (newName == null)
            {
                throw new ArgumentException("New name cannot be null.", nameof(newName));
            }
            AccountHolderName = newName;           
        }

        public string GetAccountHolderName()
        {
            
            return AccountHolderName;
        }

        public ICurrencyConvertor CurrencyConvertor { get; set; }

        public void TransferFundsFromEuroAmount(Account destination, decimal amountInEuro, ICurrencyConvertor convertor)
        {
            if (amountInEuro <= 0)
            {
                throw new ArgumentException("Amount in Euro must be greater than zero.");
            }

            decimal amountInLei = convertor.ConvertFromEuro(amountInEuro);

            if (CurrentBalance < amountInLei)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }

            CurrentBalance -= amountInLei;
            destination.CurrentBalance += amountInLei;
        }


    }

}