using Blazored.LocalStorage;
using HttpClientService;
using Microsoft.AspNetCore.Components.Authorization;
using RH.ModelView;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Json;

namespace AvaliacaoServidorBlazor.Client.Model
{
    public class JwtStorageCliente : IJwtStorage
    {
        private readonly ILocalStorageService local;
        private const string LocalKey = "auth-jwt";

        public JwtStorageCliente(ILocalStorageService local)
        {
            this.local = local;
        }

        public async Task Updateauthentication(Token token)
        {
            await local.SetItemAsStringAsync(LocalKey, JsonSerializer.Serialize(token));
        }

        public async Task<string?> GetAuthenticationStateAsync()
        {
            var acesso = await local.GetItemAsStringAsync(LocalKey);
            if(string.IsNullOrEmpty(acesso))
                return "";
            Token token = JsonSerializer.Deserialize<Token>(acesso!)!;
            return token.token;
        }

        public async Task<bool> isLoggedAsync()
        {
            var acesso = await local.GetItemAsStringAsync(LocalKey);
            if (string.IsNullOrEmpty(acesso))
                return false;
            Token token = JsonSerializer.Deserialize<Token>(acesso!)!;
            var atual = DateTime.Now;
            if(atual > token.dataExpira)
                return false;
            return true;
        }

        public async Task LogOffAsync()
        {
            await local.RemoveItemAsync(LocalKey);
        }
    }
}
