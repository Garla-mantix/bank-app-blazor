namespace bankapp.Interfaces;
/// <summary>
/// Interface for creating accounts, listing them and performing banking operations.
/// </summary>
public interface IAccountService
{
    Task<IBankAccount> CreateAccountAsync(string name, AccountType accountType, CurrencyType currencyType, decimal initialBalance);
    Task<List<IBankAccount>> GetAccounts();
    Task DepositAsync(Guid accountId, decimal amount);
    Task WithdrawAsync(Guid accountId, decimal amount);
    Task TransferAsync(Guid fromAccountId, Guid toAccountId, decimal amount); 
    Task ApplyYearlyInterestAsync();
}