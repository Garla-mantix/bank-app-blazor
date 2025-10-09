namespace bankapp.Services;

public class AccountService : IAccountService
{
    public IBankAccount CreateAccount(string name, string currency, decimal initialBalance)
    {
        BankAccount newAccount = new BankAccount();
    }

    public List<IBankAccount> GetAccounts()
    {
        throw new NotImplementedException();
    }
}