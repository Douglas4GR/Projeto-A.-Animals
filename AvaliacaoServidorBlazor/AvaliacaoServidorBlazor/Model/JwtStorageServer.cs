using HttpClientService;
using RH.ModelView;
using System.Net;

namespace AvaliacaoServidorBlazor.Model
{
    public class JwtStorageServer : IJwtStorage
    {
        private readonly IHttpContextAccessor httpContext;

        public JwtStorageServer(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }
        public async Task<string?> GetAuthenticationStateAsync()
        {
            return await Task.FromResult(httpContext.HttpContext!.Session.GetString("jwt"));
        }

        public async Task<bool> isLoggedAsync()
        {
            return !string.IsNullOrEmpty(await GetAuthenticationStateAsync());
        }

        public async Task LogOffAsync()
        {
            httpContext.HttpContext!.Session.Remove("jwt");
        }

        public async Task Updateauthentication(Token token)
        {
            httpContext.HttpContext!.Session.SetString("jwt", token.token);
        }
    }
}
