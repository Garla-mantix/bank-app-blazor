namespace bankapp.Interfaces;

public interface IAccountRepository
{
    Task<List<BankAccount>> GetAllAccountsAsync();
    Task<BankAccount?> GetAccountByIdAsync(Guid id);
    Task SaveAccountAsync(BankAccount account);
}