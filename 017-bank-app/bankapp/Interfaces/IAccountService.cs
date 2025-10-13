namespace bankapp.Interfaces;
/// <summary>
/// Services for creating accounts and listing them
/// </summary>
public interface IAccountService
{
    Task<IBankAccount> CreateAccountAsync(string name, AccountType accountType, CurrencyType currencyType, decimal initialBalance);
    Task<List<IBankAccount>> GetAccounts();
}