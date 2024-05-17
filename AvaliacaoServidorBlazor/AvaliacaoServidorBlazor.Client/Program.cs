using AvaliacaoServidorBlazor.Client.Model;
using Blazored.LocalStorage;
using BlazorStrap;
using HttpClientService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var Configuration = builder.Configuration;
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["Api:BaserUrl"] ?? "https://localhost:5002")
    });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IJwtStorage, JwtStorageCliente>();
builder.Services.AddScoped<AuthenticationStateProvider, AutenticacaoPersonalizada>();

builder.Services.AddScoped<ClientApiCicloAvaliativo>();
builder.Services.AddScoped<ClientApiLogin>();
builder.Services.AddScoped<ClientApiPainelAviso>();
builder.Services.AddScoped<ClientApiImportacaoAvaliacao>();
builder.Services.AddScoped<ClientApiAvaliacao>();


builder.Services.AddBlazorStrap();

await builder.Build().RunAsync();
