namespace bankapp.Interfaces;

public interface ITransactionRepository
{
    Task SaveTransactionAsync(Transaction transaction);
    Task<List<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId);
}