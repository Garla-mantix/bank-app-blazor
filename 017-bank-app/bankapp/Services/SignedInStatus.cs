namespace bankapp.Services;

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