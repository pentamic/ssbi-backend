using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Pentamic.SSBI.Services
{
    public class IdentityManagerProxyHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uri = new UriBuilder(request.RequestUri);
            if (uri.Path == "/api/users/")
            {
                var forwardUri = new UriBuilder(System.Configuration.ConfigurationManager.AppSettings["OidcProviderUrl"])
                {
                    Path = uri.Path
                };
                request.RequestUri = forwardUri.Uri;
                var memoryCache = MemoryCache.Default;
                TokenResponse tokenResponse;
                if (!memoryCache.Contains("identity-manager-api-token"))
                {
                    var tokenClient = new TokenClient(
                    System.Configuration.ConfigurationManager.AppSettings["OidcProviderUrl"] + "/connect/token"
                    , "identity-manager", "secret");
                    tokenResponse = await tokenClient.RequestClientCredentialsAsync("identity-manager-api");
                    var expiration = DateTimeOffset.UtcNow.AddMinutes(5);
                    memoryCache.Add("identity-manager-api-token", tokenResponse, expiration);
                }
                else
                {
                    tokenResponse = memoryCache.Get("identity-manager-api-token") as TokenResponse;
                }
                var client = new HttpClient();
                client.SetBearerToken(tokenResponse.AccessToken);
                if (request.Method == HttpMethod.Get)
                {
                    request.Content = null;
                }
                var response = await client.SendAsync(request, cancellationToken);
                return response;
            }
            else
            {
                var response = await base.SendAsync(request, cancellationToken);
                return response;
            }
        }
    }
}