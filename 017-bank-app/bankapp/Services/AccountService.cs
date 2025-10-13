using Blazored.LocalStorage;

namespace bankapp.Services;

public class AccountService : IAccountService
{
    // For local storage service
    private const string StorageKey = "bank_accounts";
    private readonly ILocalStorageService _localStorage;
    
    // List for accounts
    private List<IBankAccount> _accounts = new();
    
    public AccountService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    
   /// <summary>
   /// Load saved accounts from local storage
   /// </summary>
    public async Task InitializeAsync()
    {
        var saved = await _localStorage.GetItemAsync<List<BankAccount>>(StorageKey);
        if (saved != null)
            _accounts = saved.Cast<IBankAccount>().ToList();
    }
    
    /// <summary>
    /// Returns current list of accounts in memory
    /// </summary>
    /// <returns>_accounts in memory</returns>
    public List<IBankAccount> GetAccounts() => _accounts;
    
    /// <summary>
    /// Creates new accounts, add them to list and local storage
    /// </summary>
    /// <param name="name"></param>
    /// <param name="accountType"></param>
    /// <param name="currencyType"></param>
    /// <param name="initialBalance"></param>
    /// <returns></returns>
    public IBankAccount CreateAccount(string name, AccountType accountType, CurrencyType currencyType, decimal initialBalance)
    {
        var account = new BankAccount(name, accountType, currencyType, initialBalance);
        _accounts.Add(account);
        _ = SaveAccountsAsync();
        return account;
    }
    
   /// <summary>
   /// Saves list to local storage
   /// </summary>
    private async Task SaveAccountsAsync()
    {
        var concreteList = _accounts.Cast<BankAccount>().ToList();
        await _localStorage.SetItemAsync(StorageKey, concreteList);
    }
}