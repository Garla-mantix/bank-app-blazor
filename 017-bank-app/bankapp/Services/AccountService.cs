namespace bankapp.Services;
/// <summary>
/// Service for managing accounts. Using repositories for persistence.
/// </summary>
public class AccountService : IAccountService
{
    private readonly IStorageService _storage;
    private const string AccountsKey = "bankapp_accounts";
    private const string TransactionsKey = "bankapp_transactions";

    public AccountService(IStorageService storage)
    {
        _storage = storage;
    }

    /// <summary>
    /// Creates a new account and saves it to local storage
    /// </summary>
    public async Task<IBankAccount> CreateAccountAsync(string name, AccountType accountType, CurrencyType currencyType, 
        decimal initialBalance)
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        var newAccount = new BankAccount(name, accountType, currencyType, initialBalance);
        accounts.Add(newAccount);
        await _storage.SetItemAsync(AccountsKey, accounts);
        return newAccount;
    }

    /// <summary>
    /// Lists all accounts
    /// </summary>
    public async Task<List<IBankAccount>> GetAccounts()
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        return accounts.Cast<IBankAccount>().ToList();
    }

    /// <summary>
    /// Gets a single account by ID.
    /// </summary>
    public async Task<IBankAccount?> GetAccountByIdAsync(Guid id)
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        return accounts.FirstOrDefault(a => a.Id == id);
    }
    
    /// <summary>
    /// Deposits money into an account and records the transaction.
    /// </summary>
    public async Task DepositAsync(Guid accountId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Deposit amount must be positive.", nameof(amount));
        }
        
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        var account = accounts.FirstOrDefault(a => a.Id == accountId);
        if (account == null) throw new ArgumentException("Account not found.");

        account.Deposit(amount);
        await SaveTransactionAsync(new Transaction(accountId, amount, TransactionType.Deposit));
        await _storage.SetItemAsync(AccountsKey, accounts);
    }

    /// <summary>
    /// Withdraws money from an account and records the transaction.
    /// </summary>
    public async Task WithdrawAsync(Guid accountId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));
        }
        
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        var account = accounts.FirstOrDefault(a => a.Id == accountId);
        
        if (account == null)
        {
            throw new ArgumentException("Account not found.");
        }
        if (amount > account.Balance)
        {
            throw new InvalidOperationException("Insufficient funds.");
        }
        
        account.Withdraw(amount);
        await SaveTransactionAsync(new Transaction(accountId, amount, TransactionType.Withdrawal));
        await _storage.SetItemAsync(AccountsKey, accounts);
    }

    /// <summary>
    /// Transfer money between two accounts and record two transactions.
    /// </summary>
    public async Task TransferAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        if (fromAccountId == toAccountId)
        {
            throw new ArgumentException("Cannot transfer to the same account.");
        }
        if (amount <= 0)
        {
            throw new ArgumentException("Transfer amount must be positive.", nameof(amount));
        }
        
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        var from = accounts.FirstOrDefault(a => a.Id == fromAccountId);
        var to = accounts.FirstOrDefault(a => a.Id == toAccountId);
        
        if (from == null || to == null)
            throw new ArgumentException("Invalid account(s).");

        if (amount > from.Balance)
        {
            throw new InvalidOperationException("Insufficient funds for transfer.");
        }
        
        from.Withdraw(amount);
        to.Deposit(amount);

        await SaveTransactionAsync(new Transaction(from.Id, amount, TransactionType.Transfer, to.Name, $"Transfer to {to.Name}"));
        await SaveTransactionAsync(new Transaction(to.Id, amount, TransactionType.Transfer, from.Name, $"Transfer from {from.Name}"));

        await _storage.SetItemAsync(AccountsKey, accounts);
    }

    /// <summary>
    /// Get transaction history for an account.
    /// </summary>
    public async Task<List<Transaction>> GetTransactionsAsync(Guid accountId)
    {
        var transactions = await _storage.GetItemAsync<List<Transaction>>(TransactionsKey) ?? new();
        return transactions.Where(t => t.AccountId == accountId).ToList();
    }
    
    /// <summary>
    /// Saves transactions
    /// </summary>
    /// <param name="transaction"></param>
    private async Task SaveTransactionAsync(Transaction transaction)
    {
        var transactions = await _storage.GetItemAsync<List<Transaction>>(TransactionsKey) ?? new();
        transactions.Add(transaction);
        await _storage.SetItemAsync(TransactionsKey, transactions);
    }
}
