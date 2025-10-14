namespace bankapp.Interfaces;
/// <summary>
/// Interface for creating accounts and listing them
/// </summary>
public interface IAccountService
{
    Task<IBankAccount> CreateAccountAsync(string name, AccountType accountType, CurrencyType currencyType, decimal initialBalance);
    Task<List<IBankAccount>> GetAccounts();
    Task<IBankAccount?> GetAccountByIdAsync(Guid id);
    Task DepositAsync(Guid accountId, decimal amount);
    Task WithdrawAsync(Guid accountId, decimal amount);
}