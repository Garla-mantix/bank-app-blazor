namespace bankapp.Interfaces;
/// <summary>
/// Interface containing the BankAccount properties and methods
/// </summary>
public interface IBankAccount
{
    Guid Id { get; }
    string Name { get; }
    AccountType AccountType { get; }
    CurrencyType  CurrencyType{ get; }
    decimal Balance { get; }
    DateTime LastUpdated { get; }
    
    void Deposit(decimal amount);
    void Withdraw(decimal amount);
}