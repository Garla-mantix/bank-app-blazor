namespace bankapp.Services;
/// <summary>
/// Service for accounts
/// </summary>
public class AccountService : IAccountService
{
    private const string StorageKey = "bankapp.accounts";
    private readonly List<IBankAccount> _accounts = new();
    private readonly IStorageService _storageService;
    private bool isLoaded;
    public AccountService(IStorageService storageService) => _storageService = storageService;

    private async Task IsInitialized()
    {
        if (isLoaded)
        {
            return;
        }
        var fromStorage = await _storageService.GetItemAsync <List<BankAccount>>(StorageKey);
        _accounts.Clear();
        if (fromStorage is { Count: > 0 })
        {
            _accounts.AddRange(fromStorage);
        }
        isLoaded = true;
    }
    
    private Task SaveAsync() => _storageService.SetItemAsync(StorageKey, _accounts);

    public async Task<IBankAccount> CreateAccountAsync(string name, AccountType accountType, CurrencyType currencyType,
        decimal initialBalance)
    {
        await IsInitialized();
        var account = new BankAccount(name, accountType, currencyType, initialBalance);
        _accounts.Add(account);
        await SaveAsync();
        return account;
    }

    /// <summary>
    /// Lists all accounts
    /// </summary>
    /// <returns></returns>
    public async Task<List<IBankAccount>> GetAccounts()
    {
        await IsInitialized();
        return _accounts.Cast<IBankAccount>().ToList();
    }
}
