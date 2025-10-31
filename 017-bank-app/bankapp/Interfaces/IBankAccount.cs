namespace bankapp.Interfaces;
/// <summary>
/// Interface containing the structure and behavior of bank accounts.
/// </summary>
public interface IBankAccount
{
    Guid Id { get; }
    string Name { get; }
    AccountType AccountType { get; }
    CurrencyType  CurrencyType{ get; }
    decimal Balance { get; }
    DateTime LastUpdated { get; }
    List<Transaction> Transactions { get; }
    decimal InterestRate { get; }
    
    void Deposit(decimal amount);
    void Withdraw(decimal amount, BudgetCategory category);
    void TransferTo(BankAccount toAccount, decimal amount, BudgetCategory category = BudgetCategory.None);
}