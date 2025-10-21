namespace bankapp.Domain;
/// <summary>
/// Funcionality of accounts
/// </summary>
public class BankAccount : IBankAccount
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public AccountType AccountType { get; private set; }
    public CurrencyType CurrencyType { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime LastUpdated { get; private set; }
    public List<Transaction> Transactions { get; private set; } = new();
    
    public BankAccount(string name, AccountType accountType, CurrencyType currencyType, decimal initialBalance)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty");
        }

        Name = name;
        AccountType = accountType;
        CurrencyType= currencyType;
        Balance = initialBalance;
        LastUpdated = DateTime.Now;
    }

    [JsonConstructor]
    public BankAccount(Guid id, string name, AccountType accountType, CurrencyType currencyType, decimal balance,
        DateTime lastUpdated)
    {
        Id = id;
        Name = name;
        AccountType = accountType;
        CurrencyType = currencyType;
        Balance = balance;
        LastUpdated = lastUpdated;
    }
    
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero", nameof(amount));
        }
        Balance += amount;
        /*Transactions.Add(new Transaction(Guid.NewGuid(), DateTime.Now, TransactionType.Deposit, amount));*/
        LastUpdated = DateTime.Now;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero", nameof(amount));
        }

        if (amount > Balance)
        {
            throw new ArgumentException("Insufficient balance", nameof(amount));
        }
        Balance -= amount;
        /*Transactions.Add(new Transaction(Guid.NewGuid(), DateTime.Now, TransactionType.Withdrawal, amount));*/
        LastUpdated = DateTime.Now;
    }
    
    /*public void TransferTo(BankAccount toAccount, decimal amount)
    {
        Från vilket konto
        
         Balance -= amount;
         LastUpdated = DateTime.Now
        _Transactions.Add(new Transaction
        {
            TransactionType = TransactionType.TransferOut,
            Amount = amount,
            BalanceAfter = Balance,
            FromAccountId = Id,
            ToAccountId = toAccount.Id,
        });
        
         Till vilket konto

        toAccount.Balance += amount;
        toAccount.LastUpdated = DateTime.Now;
        _transactions.Add(new Transaction
        {
            toAccount.Balance = 
        })
    }*/
}