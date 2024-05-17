using AvaliacaoServidorBlazor.Client.Model;
using AvaliacaoServidorBlazor.Client.Pages;
using AvaliacaoServidorBlazor.Components;
using AvaliacaoServidorBlazor.Model;
using Blazored.LocalStorage;
using BlazorStrap;
using HttpClientService;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
var Configuration = builder.Configuration;
builder.Services.AddCascadingAuthenticationState();
/*builder.Services.AddAuthentication();
builder.Services.AddAuthorization()*/;
//builder.Services.AddAuthorizationCore();
//builder.Services.AddDistributedMemoryCache();
builder.Services.AddBlazorStrap();
/*builder.Services.AddSession(o =>
{
    o.IOTimeout = TimeSpan.FromMinutes(5);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});*/

//builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddHttpContextAccessor();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IJwtStorage, JwtStorageCliente>();
builder.Services.AddScoped<AuthenticationStateProvider, AutenticacaoPersonalizada>();

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["Api:BaserUrl"] ?? "https://localhost:5002")
    });
//builder.Services.AddHttpClient();

builder.Services.AddScoped<ClientApiCicloAvaliativo>();
builder.Services.AddScoped<ClientApiLogin>();
builder.Services.AddScoped<ClientApiPainelAviso>();
builder.Services.AddScoped<ClientApiServidor>();
builder.Services.AddScoped<ClientApiImportacaoAvaliacao>();
builder.Services.AddScoped<ClientApiAvaliacao>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
//app.UseSession();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Logout).Assembly);

app.Run();
