namespace bankapp.Services;

/// <summary>
/// Service for saving and retrieving data from local storage.
/// </summary>
public class StorageService : IStorageService
{
    private readonly IJSRuntime _jsRuntime;

    private JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };
        
    public StorageService(IJSRuntime jsRuntime) => _jsRuntime = jsRuntime;
        
    /// <summary>
    /// Serializes an object to JSON-format and saves it to local storage.
    /// </summary>
    public async Task SetItemAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value, _jsonSerializerOptions);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        Console.WriteLine("Serialized the item and saved it to storage.");
    }

    /// <summary>
    /// De-serializes an object from local storage.
    /// </summary>
    public async Task<T> GetItemAsync<T>(string key)
    {
        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }
            
        Console.WriteLine("An item has been fetched from storage and de-serialized.");
        return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
    }
}