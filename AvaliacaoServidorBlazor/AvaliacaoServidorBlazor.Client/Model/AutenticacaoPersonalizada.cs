using HttpClientService;
using Microsoft.AspNetCore.Components.Authorization;
using RH.ModelView;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AvaliacaoServidorBlazor.Client.Model
{
    public class AutenticacaoPersonalizada : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonimo = new ClaimsPrincipal();
        private readonly IJwtStorage jwtStorage;

        public AutenticacaoPersonalizada(IJwtStorage jwtStorage)
        {
            this.jwtStorage = jwtStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (!await jwtStorage.isLoggedAsync())
                    return await Task.FromResult(new AuthenticationState(anonimo));
                var userClaims = DecriptarToken(await jwtStorage.GetAuthenticationStateAsync());
                if (userClaims is null || string.IsNullOrEmpty(userClaims.Name))
                    return await Task.FromResult(new AuthenticationState(anonimo));
                var claimsPrincipal = setClaimPrincipal(userClaims);
                if (claimsPrincipal is null)
                    return await Task.FromResult(new AuthenticationState(anonimo));
                else
                    return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch { 
                return await Task.FromResult(new AuthenticationState(anonimo));
            }
        }

        public async Task UpdateAutenticationStateAsync(Token jwtToke)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if (!string.IsNullOrEmpty(jwtToke.token))
            {
                await jwtStorage.Updateauthentication(jwtToke);
                var getUserClaims = DecriptarToken(jwtToke.token);
                if (getUserClaims is null || string.IsNullOrEmpty(getUserClaims.Name))
                    return;
                claimsPrincipal = setClaimPrincipal(getUserClaims);
                if (claimsPrincipal is null)
                    return;
            }
            else
                await jwtStorage.LogOffAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        private CustomUserClaims DecriptarToken(string? jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
                return new CustomUserClaims();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var name = token.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Name)!.Value;
            return new CustomUserClaims(name);
        }

        private ClaimsPrincipal setClaimPrincipal(CustomUserClaims userClaims)
        {
            if (userClaims.Name is null)
                return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new (ClaimTypes.Name, userClaims.Name)
                ], "JwtAuth"));
        }
    }
}
