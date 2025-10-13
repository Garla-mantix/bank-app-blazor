using System.Text.Json.Serialization;

namespace bankapp.Domain;

public class BankAccount : IBankAccount
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public AccountType AccountType { get; private set; }
    public CurrencyType CurrencyType { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime LastUpdated { get; private set; }
    
    public BankAccount(string name, AccountType accountType, CurrencyType currencyType, decimal initialBalance)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        AccountType = accountType;
        CurrencyType= currencyType;
        Balance = initialBalance;
        LastUpdated = DateTime.Now;
    }

    [JsonConstructor]
    public BankAccount(Guid id, string name, AccountType accountType, CurrencyType currencyType, decimal balance,
        DateTime lastUpdated)
    {
        Id = id;
        Name = name;
        AccountType = accountType;
        CurrencyType = currencyType;
        Balance = balance;
        LastUpdated = lastUpdated;
    }
    
    public void Deposit(decimal amount)
    {
        try
        {
            Balance = Balance + amount;
        }
        catch (ArgumentOutOfRangeException e)
        {
            if (amount <= 0)
            {
                Console.WriteLine(e);
            }
        }
    }

    public void Withdraw(decimal amount)
    {
        try
        {
            Balance =  Balance - amount;
        }
        catch (ArgumentOutOfRangeException e)
        {
            if (amount <= 0)
            {
                Console.WriteLine(e);
            }
            else if (amount > Balance)
            {
                Console.WriteLine(e);
            }
        }
    }
}