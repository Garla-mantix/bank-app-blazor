namespace bankapp.Services;

/// <summary>
/// Service for managing accounts using local storage for persistence.
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
    /// Creates a new account, saves it to local storage and returns it.
    /// </summary>
    public async Task<IBankAccount> CreateAccountAsync(string name, AccountType accountType, CurrencyType currencyType, 
        decimal initialBalance)
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        
        if (accounts.Any(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException($"An account with that name already exists.");
        }
        
        var newAccount = new BankAccount(name, accountType, currencyType, initialBalance);
        accounts.Add(newAccount);
        Console.WriteLine("A new account has been created.");
        await _storage.SetItemAsync(AccountsKey, accounts);
        return newAccount;
    }

    /// <summary>
    /// Retrieves all saved accounts.
    /// </summary>
    public async Task<List<IBankAccount>> GetAccounts()
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        return accounts.Cast<IBankAccount>().ToList();
    }
    
    /// <summary>
    /// Deposits money into an account and records the transaction.
    /// </summary>
    public async Task DepositAsync(Guid accountId, decimal amount)
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        var account = accounts.FirstOrDefault(a => a.Id == accountId);
        if (account == null)
        {
            throw new ArgumentException("Account not found.");
        }

        Console.WriteLine($"Depositing {amount:F2} to {account.Name}");
        account.Deposit(amount);
        await _storage.SetItemAsync(AccountsKey, accounts);
    }

    /// <summary>
    /// Withdraws money from an account and records the transaction.
    /// </summary>
    public async Task WithdrawAsync(Guid accountId, decimal amount, BudgetCategory category)
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        var account = accounts.FirstOrDefault(a => a.Id == accountId);
        if (account == null)
        {
            throw new ArgumentException("Account not found.");
        }

        Console.WriteLine($"Withdrawing {amount:F2} from {account.Name}");
        account.Withdraw(amount, category);
        await _storage.SetItemAsync(AccountsKey, accounts);
    }

    /// <summary>
    /// Transfers money between two accounts and records two transactions.
    /// </summary>
    public async Task TransferAsync(Guid fromAccountId, Guid toAccountId, decimal amount, BudgetCategory category)
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();
        var from = accounts.FirstOrDefault(a => a.Id == fromAccountId);
        var to = accounts.FirstOrDefault(a => a.Id == toAccountId);
        
        if (from == null || to == null)
        {
            throw new ArgumentException("Invalid account(s).");
        }
        
        Console.WriteLine($"Transfering {amount:F2} from {from.Name} to {to.Name}");
        from.TransferTo(to, amount, category);
        await _storage.SetItemAsync(AccountsKey, accounts);
    }
    
    /// <summary>
    /// Applies interest rate of savings accounts once every 365 days.
    /// </summary>
    public async Task ApplyYearlyInterestAsync()
    {
        var accounts = await _storage.GetItemAsync<List<BankAccount>>(AccountsKey) ?? new();

        foreach (var account in accounts)
        {
            account.ApplyYearlyInterest();
        }

        await _storage.SetItemAsync(AccountsKey, accounts);
    }
}
