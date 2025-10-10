namespace bankapp.Services;

public class AccountService : IAccountService
{
    private List<IBankAccount> _accounts = new();
    public IBankAccount CreateAccount(string name, AccountType accountType, CurrencyType currencyType, decimal initialBalance)
    {
        var account = new BankAccount(name, accountType, currencyType, initialBalance);
        _accounts.Add(account);
        return account;
    }

    public List<IBankAccount> GetAccounts() => _accounts;
}