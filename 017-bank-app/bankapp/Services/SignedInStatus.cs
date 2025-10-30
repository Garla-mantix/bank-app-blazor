namespace bankapp.Services;

public class SignedInStatus
{
    public bool IsSignedIn { get; private set; } = false;

    public void SignIn() => IsSignedIn = true;
    public void SignOut() => IsSignedIn = false;
}