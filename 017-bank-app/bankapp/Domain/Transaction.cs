namespace bankapp.Domain;

public class Transaction
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Timestamp { get; private set; } = DateTime.Now;
    public TransactionType Type { get; private set; }
    public string? RelatedAccountName { get; private set; }
    public decimal BalanceAfter {get; private set;}
    public string? Description { get; private set; }
   
    
    // public Guid? FromAccountId { get; private set; }
    // public Guid? ToAccountId { get; private set; }
   

    [JsonConstructor]
    public Transaction(Guid id, Guid accountId, decimal amount, DateTime timestamp, 
        TransactionType type, string? relatedAccountName, string? description, decimal balanceAfter)
    {
        Id = id;
        AccountId = accountId;
        Amount = amount;
        Timestamp = timestamp;
        Type = type;
        RelatedAccountName = relatedAccountName;
        Description = description;
        BalanceAfter = balanceAfter;
    }
    
    /// <summary>
    /// Full constructor, used for transfers
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="amount"></param>
    /// <param name="type"></param>
    /// <param name="relatedAccountName"></param>
    /// <param name="description"></param>
    /// <exception cref="ArgumentException"></exception>
    public Transaction(Guid accountId, decimal amount, TransactionType type, decimal balanceAfter, string? relatedAccountName, string? description)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Transaction amount must be positive.", nameof(amount));
        }
        
        Id = Guid.NewGuid();
        AccountId = accountId;
        Amount = amount;
        Type = type;
        Timestamp = DateTime.Now;
        RelatedAccountName = relatedAccountName;
        Description = description ?? $"{type} of {amount:C}";
        BalanceAfter = balanceAfter;
    }
    
    /// <summary>
    /// Shorter constructor for deposits and withdrawals
    /// </summary>
    public Transaction(Guid accountId, decimal amount, TransactionType type, decimal  balanceAfter)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Transaction amount must be positive.", nameof(amount));
        }

        Id = Guid.NewGuid();
        AccountId = accountId;
        Amount = amount;
        Type = type;
        Timestamp = DateTime.Now;
        BalanceAfter = balanceAfter;
        RelatedAccountName = null;
        Description = $"{type} of {amount:C}";
    }
}