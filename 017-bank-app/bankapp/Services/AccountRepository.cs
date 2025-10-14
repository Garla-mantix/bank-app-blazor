namespace bankapp.Services;

/// <summary>
/// Repository for saving and retrieving BankAccount data.
/// </summary>
public class AccountRepository : IAccountRepository
{
    private readonly IStorageService _storageService;
    private const string AccountsKey = "bankapp.accounts";

    public AccountRepository(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<List<BankAccount>> GetAllAccountsAsync()
    { 
        var accounts = await _storageService.GetItemAsync<List<BankAccount>>(AccountsKey);
        return accounts ?? new List<BankAccount>();
    }

    public async Task<BankAccount?> GetAccountByIdAsync(Guid id)
    {
        var accounts = await GetAllAccountsAsync();
        return accounts.FirstOrDefault(a => a.Id == id);
    }

    public async Task SaveAccountAsync(BankAccount account)
    {
        var accounts = await GetAllAccountsAsync();

        // Removing existing account and replacing it when updating it with new values
        var existing = accounts.FirstOrDefault(a => a.Id == account.Id);
        if (existing != null)
            accounts.Remove(existing);

        // Adding the new account
        accounts.Add(account);

        // Saving it
        await _storageService.SetItemAsync(AccountsKey, accounts);
    }
}
