using BlazorApp.API;
using BlazorApp.Client.Services.Cloud;
using BlazorApp.Client.Settings;
using BlazorApp.Pages;
using BlazorApp.Pages.Account;
using BlazorApp.Services;
using BlazorApp.Services.Cloud;
using DataContext;
using DataContext.Configuration;
using DataContext.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); //api

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.UseDB();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddHttpClient();

builder.Services.AddOpenApi();

builder.Services.Configure<FileUploadSettings>(builder.Configuration.GetSection(nameof(FileUploadSettings))); 
builder.Services.Configure<CloudSettings>(builder.Configuration.GetSection(nameof(CloudSettings)));

builder.Services.AddScoped<ICloudNavigator, CloudServerNavigator>();
builder.Services.AddScoped<CloudProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//MS openAPI (/openapi/v1.json)
app.MapOpenApi();
//Scalar (/scalar/v1)
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Info API")
        .WithTheme(ScalarTheme.Kepler)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
        .EnableDarkMode();
});

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseRouting(); //mvc
app.UseAuthorization(); //mvc + api

app.UseAntiforgery();

app.UseStaticFiles();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp.Client._Imports).Assembly);

app.MapControllers(); //api
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets(); //mvc

// Добавьте дополнительные конечные точки, необходимые для компонентов Identity /Account Razor.
app.MapAdditionalIdentityEndpoints();

app.UsingAPIEndpoints();
// Миграции EF Core
app.AutoMigrate();

app.Run();
