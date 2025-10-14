namespace bankapp.Interfaces;
/// <summary>
/// Interface for local storage
/// </summary>
public interface IStorageService
{
    Task SetItemAsync<T>(string key, T value);
    Task<T?> GetItemAsync<T>(string key);
}
