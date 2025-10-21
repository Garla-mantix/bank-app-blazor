namespace bankapp.Services;
/// <summary>
/// Service for managing accounts. Using repositories for persistence.
/// </summary>
public class AccountService : IAccountService
{
    private readonly IStorageService _storage;
    private const string AccountsKey = "bankapp_accounts";

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
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        var account = accounts.FirstOrDefault(a => a.Id == accountId);
        if (account == null) throw new ArgumentException("Account not found.");

        account.Deposit(amount);
        await _storage.SetItemAsync(AccountsKey, accounts);
    }

    /// <summary>
    /// Withdraws money from an account and records the transaction.
    /// </summary>
    public async Task WithdrawAsync(Guid accountId, decimal amount)
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        var account = accounts.FirstOrDefault(a => a.Id == accountId);
        if (account == null) throw new ArgumentException("Account not found.");

        account.Withdraw(amount);
        await _storage.SetItemAsync(AccountsKey, accounts);
    }

    /// <summary>
    /// Transfer money between two accounts and record two transactions.
    /// </summary>
    public async Task TransferAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        var from = accounts.FirstOrDefault(a => a.Id == fromAccountId);
        var to = accounts.FirstOrDefault(a => a.Id == toAccountId);
        
        if (from == null || to == null)
            throw new ArgumentException("Invalid account(s).");
        
        from.TransferTo(to, amount);

        await _storage.SetItemAsync(AccountsKey, accounts);
    }

    /// <summary>
    /// Get transaction history for an account.
    /// </summary>
    public async Task<List<Transaction>> GetTransactionsAsync(Guid accountId)
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        var account = accounts.FirstOrDefault(a => a.Id == accountId);
        return account?.Transactions ?? new List<Transaction>();
    }
}
