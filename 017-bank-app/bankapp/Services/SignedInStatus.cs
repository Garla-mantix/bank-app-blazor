namespace bankapp.Services;

/// <summary>
/// Tracks whether the user is logged in or not. Used for UI-lock.
/// </summary>
public class SignedInStatus
{
    public bool IsSignedIn { get; private set; } = false;

    public void SignIn()
    {
        IsSignedIn = true;
        Console.WriteLine($"Signed in.");
    }

    public void SignOut()
    {
        IsSignedIn = false;
        Console.WriteLine($"Signed out.");
    }
}