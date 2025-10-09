namespace bankapp.Interfaces;
/// <summary>
/// Services for creating accounts etc
/// </summary>
public interface IAccountService
{
    IBankAccount CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance);
    List<IBankAccount> GetAccounts();
}