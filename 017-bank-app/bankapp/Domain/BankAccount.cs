namespace bankapp.Domain;

/// <summary>
/// Schematic of a bank account – with functionality for deposit, withdrawals and transfers.
/// </summary>
public class BankAccount : IBankAccount
{
    // Constants
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public AccountType AccountType { get; private set; }
    public CurrencyType CurrencyType { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime LastUpdated { get; private set; }
    public List<Transaction> Transactions { get; private set; } = new();
    public decimal InterestRate { get; private set; }
    public DateTime? LastInterestApplied { get; private set; }
    
    // Constructor for creating accounts with an initial deposit.
    public BankAccount(string name, AccountType accountType, CurrencyType currencyType, 
        decimal initialBalance, decimal? interestRate = null)
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
        // Applies interest rate only if account type is savings
        InterestRate = (accountType == AccountType.Savings)
            ? interestRate ?? 0.015m
            : 0m;
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
        DateTime lastUpdated, List<Transaction>? transactions = null, decimal interestRate = 0m, 
        DateTime? lastInterestApplied = null)
    {
        Id = id;
        Name = name;
        AccountType = accountType;
        CurrencyType = currencyType;
        Balance = balance;
        LastUpdated = lastUpdated;
        Transactions = transactions ?? new List<Transaction>();
        InterestRate = interestRate;
        LastInterestApplied = lastInterestApplied;
    }
    
    /// <summary>
    /// Deposits an amount to a bank account's balance
    /// </summary>
    /// <param name="amount">Amount to deposit</param>
    /// <exception cref="ArgumentException">Cannot deposit less than 1</exception>
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
    /// Withdraws an amount from a bank account's balance
    /// </summary>
    /// <param name="amount">Amount to withdraw</param>
    /// <exception cref="ArgumentException">Cannot withdraw less than 1</exception>
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
   /// Transfers funds between two accounts
   /// </summary>
   /// <param name="toAccount">Account to receive the transfer</param>
   /// <param name="amount">Amount to transfer</param>
   /// <exception cref="ArgumentNullException">Has to have a receiver account</exception>
   /// <exception cref="InvalidOperationException">Cannot transfer to same account as sender</exception>
   /// <exception cref="ArgumentException">Cannot transfer less than 1</exception>
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
   
       public void ApplyYearlyInterest()
       {
           if (AccountType != AccountType.Savings || InterestRate <= 0)
               return;

           var now = DateTime.Now;

           // Apply only if at least a year since last interest
           if (LastInterestApplied == null || (now - LastInterestApplied.Value).TotalDays >= 1)
           {
               var interest = Balance * InterestRate;
               if (interest > 0)
               {
                   Balance += interest;
                   LastUpdated = now;
                   LastInterestApplied = now;

                   Transactions.Add(new Transaction(
                       Id,
                       interest,
                       TransactionType.Deposit,
                       Balance,
                       null,
                       "Yearly interest rate"
                   ));
                   Console.WriteLine("Yearly interest rate applied: " + InterestRate);
               }
           }
       }
}