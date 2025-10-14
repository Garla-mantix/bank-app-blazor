namespace bankapp.Services;

/// <summary>
/// Repository for saving and retrieving Transaction data.
/// </summary>
public class TransactionRepository : ITransactionRepository
{
    private readonly IStorageService _storageService;
    private const string TransactionsKey = "bankapp.transactions";

    public TransactionRepository(IStorageService storageService)
    {
        _storageService = storageService;
    }

    /// <summary>
    /// Saves a transaction to the list in storage.
    /// </summary>
    public async Task SaveTransactionAsync(Transaction transaction)
    {
        // Getting current list or creating a new one if it doesn't exist
        var transactions = await _storageService.GetItemAsync<List<Transaction>>(TransactionsKey)
                           ?? new List<Transaction>();

        // Adding the new transaction
        transactions.Add(transaction);

        // Saving the updated list back to storage
        await _storageService.SetItemAsync(TransactionsKey, transactions);
    }

    /// <summary>
    /// Returns all transactions for a specific account.
    /// </summary>
    public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId)
    {
        var transactions = await _storageService.GetItemAsync<List<Transaction>>(TransactionsKey)
                           ?? new List<Transaction>();

        return transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Timestamp)
            .ToList();
    }
}