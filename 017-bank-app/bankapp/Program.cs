using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Local storage
builder.Services.AddBlazoredLocalStorage();

// Account service
builder.Services.AddScoped<IAccountService, AccountService>();

// HTTP client
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Building
var host = builder.Build();

// Initializing AccountService to load saved accounts from local storage
try
{
    var accountService = host.Services.GetRequiredService<IAccountService>() as AccountService;
    if (accountService is not null)
    {
        await accountService.InitializeAsync();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to initialize AccountService: {ex.Message}");
}

// Running
await host.RunAsync();