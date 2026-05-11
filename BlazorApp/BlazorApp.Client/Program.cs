using BlazorApp.Client.Services;
using BlazorApp.Client.Services.Cloud;
using BlazorApp.Client.Settings;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });

builder.Services.AddTransient<ICloudNavigator, CloudClientNavigator>();

await builder.Build().RunAsync();
