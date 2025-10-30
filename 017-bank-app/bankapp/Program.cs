using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Account service
builder.Services.AddScoped<IAccountService, AccountService>();

// Storage service
builder.Services.AddScoped<IStorageService, StorageService>();

// Signed-in status
builder.Services.AddScoped<SignedInStatus>();

// HTTP client
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Running
await builder.Build().RunAsync();