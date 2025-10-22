namespace bankapp.Domain;
/// <summary>
/// Schematic of a bank account – with functionality for deposit, withdrawals and transfers.
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
    
    /// <summary>
    /// Constructor for creating accounts with an initial deposit.
    /// </summary>
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
        // Logs initial balance as a first deposit
        Transactions.Add(new Transaction(Id, initialBalance, TransactionType.Deposit, 
            Balance, null, "Initial deposit"));
    }

    /// <summary>
    /// JSON constructor – after account is stored as JSON (in this case in local storage),
    /// it needs to reconstruct it from the JSON-format when loaded, using this constructor.
    /// </summary>
    [JsonConstructor]
    public BankAccount(Guid id, string name, AccountType accountType, CurrencyType currencyType, decimal balance,
        DateTime lastUpdated, List<Transaction>? transactions = null)
    {
        Id = id;
        Name = name;
        AccountType = accountType;
        CurrencyType = currencyType;
        Balance = balance;
        LastUpdated = lastUpdated;
        Transactions = transactions ?? new List<Transaction>();
    }
    
    /// <summary>
    /// Method for deposits to the account
    /// </summary>
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero", nameof(amount));
        }
        
        // Making the deposit
        Balance += amount;
        LastUpdated = DateTime.Now;
        // Recording the deposit
        Transactions.Add(new Transaction(Id, amount, TransactionType.Deposit, 
            Balance, null, $"Deposit of {amount:F2}"));
    }

    /// <summary>
    /// Method for withdrawals from the account
    /// </summary>
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
        
        // Making the withdrawal
        Balance -= amount;
        LastUpdated = DateTime.Now;
        // Recording the transaction
        Transactions.Add(new Transaction(Id, amount, TransactionType.Withdrawal, 
            Balance, null, $"Withdrawal of {amount:F2}"));
    }
    
    /// <summary>
    /// Method for transferring money between two accounts
    /// </summary>
    public void TransferTo(BankAccount toAccount, decimal amount)
       {
           if (toAccount == null)
           {
               throw new ArgumentNullException(nameof(toAccount));
           }
           if (toAccount.Id == Id)
           {
               throw new InvalidOperationException("Cannot transfer to the same account.");
           }
           if (amount <= 0)
           {
               throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
           }
           if (amount > Balance)
           {
               throw new InvalidOperationException("Insufficient funds.");
           }
           
           // Making the transfer
           Balance -= amount;
           toAccount.Balance += amount;

           // Recording the transfer on both accounts
           Transactions.Add(new Transaction(Id, amount, TransactionType.Transfer, 
               Balance, toAccount.Name, $"Transfer to {toAccount.Name}"));
           toAccount.Transactions.Add(new Transaction(toAccount.Id, amount, 
               TransactionType.Transfer, toAccount.Balance, Name, $"Transfer from {Name}"));
       }
}