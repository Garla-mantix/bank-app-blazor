namespace bankapp.Domain;

public class BankAccount : IBankAccount
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public AccountType AccountType { get; private set; }
    public string Currency { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime LastUpdated { get; private set; }
    
    public BankAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        AccountType = accountType;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        Balance = initialBalance;
        LastUpdated = DateTime.Now;
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