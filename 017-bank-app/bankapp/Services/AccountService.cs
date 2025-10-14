namespace bankapp.Services;
/// <summary>
/// Service for managing accounts. Using repositories for persistence.
/// </summary>
public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
    }

    /// <summary>
    /// Creates a new account and saves it via the repository.
    /// </summary>
    public async Task<IBankAccount> CreateAccountAsync(string name, AccountType accountType, CurrencyType currencyType, 
        decimal initialBalance)
    {
        var account = new BankAccount(name, accountType, currencyType, initialBalance);
        await _accountRepository.SaveAccountAsync(account);
        return account;
    }

    /// <summary>
    /// Lists all accounts.
    /// </summary>
    public async Task<List<IBankAccount>> GetAccounts()
    {
        var accounts = await _accountRepository.GetAllAccountsAsync();
        return accounts.Cast<IBankAccount>().ToList();
    }

    /// <summary>
    /// Gets a single account by ID.
    /// </summary>
    public async Task<IBankAccount?> GetAccountByIdAsync(Guid id)
    {
        return await _accountRepository.GetAccountByIdAsync(id);
    }
    
    /// <summary>
    /// Deposits money into an account and records the transaction.
    /// </summary>
    public async Task DepositAsync(Guid accountId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Deposit amount must be positive.", nameof(amount));
        }
        
        var account = await _accountRepository.GetAccountByIdAsync(accountId)
                      ?? throw new InvalidOperationException("Account not found.");

        account.Deposit(amount);
        await _accountRepository.SaveAccountAsync(account);

        var transaction = new Transaction(account.Id, amount, TransactionType.Deposit);
        await _transactionRepository.SaveTransactionAsync(transaction);
    }

    /// <summary>
    /// Withdraws money from an account and records the transaction.
    /// </summary>
    public async Task WithdrawAsync(Guid accountId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));
            
        }
        
        var account = await _accountRepository.GetAccountByIdAsync(accountId)
                      ?? throw new InvalidOperationException("Account not found.");

        if (amount > account.Balance)
        {
            throw new InvalidOperationException("Insufficient funds.");
        }
        
        account.Withdraw(amount);
        await _accountRepository.SaveAccountAsync(account);

        var transaction = new Transaction(account.Id, amount, TransactionType.Withdrawal);
        await _transactionRepository.SaveTransactionAsync(transaction);
    }

    /// <summary>
    /// Transfer money between two accounts and record two transactions.
    /// </summary>
    public async Task TransferAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        if (fromAccountId == toAccountId)
        {
            throw new ArgumentException("Cannot transfer to the same account.");
        }
        if (amount <= 0)
        {
            throw new ArgumentException("Transfer amount must be positive.", nameof(amount));
        }
        
        var from = await _accountRepository.GetAccountByIdAsync(fromAccountId)
                   ?? throw new InvalidOperationException("Source account not found.");

        var to = await _accountRepository.GetAccountByIdAsync(toAccountId)
                 ?? throw new InvalidOperationException("Destination account not found.");

        if (amount > from.Balance)
        {
            throw new InvalidOperationException("Insufficient funds for transfer.");
        }
        
        // Performs transfer
        from.Withdraw(amount);
        to.Deposit(amount);

        // Saves accounts
        await _accountRepository.SaveAccountAsync(from);
        await _accountRepository.SaveAccountAsync(to);

        // Records transactions
        var outgoing = new Transaction(from.Id, amount, TransactionType.Transfer, relatedAccountName: to.Name, 
            description: $"Transfer to {to.Name}");
        var incoming = new Transaction(to.Id, amount, TransactionType.Transfer, relatedAccountName: from.Name, 
            description: $"Transfer from {from.Name}");

        await _transactionRepository.SaveTransactionAsync(outgoing);
        await _transactionRepository.SaveTransactionAsync(incoming);
    }

    /// <summary>
    /// Get transaction history for an account.
    /// </summary>
    public async Task<List<Transaction>> GetTransactionsAsync(Guid accountId)
    {
        return await _transactionRepository.GetTransactionsByAccountIdAsync(accountId);
    }
}
