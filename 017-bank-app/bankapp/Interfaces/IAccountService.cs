namespace bankapp.Interfaces;
/// <summary>
/// Services for creating accounts etc
/// </summary>
public interface IAccountService
{
    IBankAccount CreateAccount(string name, string currency, decimal initialBalance);
    List<IBankAccount> GetAccounts();
    // TODO
// add account type
}