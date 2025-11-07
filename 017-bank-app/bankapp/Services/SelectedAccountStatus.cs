namespace bankapp.Services;

/// <summary>
/// Remembers selected account when clicked from My Accounts-page,
/// in order to instantly show its history.
/// </summary>
public class SelectedAccountStatus
{ 
    public Guid? SelectedAccountId { get; set; }
}