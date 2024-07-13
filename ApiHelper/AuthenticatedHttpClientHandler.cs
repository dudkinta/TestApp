using System.Net;
using System.Net.Http.Headers;

namespace ApiHelper
{
    internal class AuthenticatedHttpClientHandler : DelegatingHandler
    {
        private readonly TokenProvider _tokenProvider;

        public AuthenticatedHttpClientHandler(TokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenProvider.GetTokenAsync());

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenProvider.GetTokenAsync());
                response = await base.SendAsync(request, cancellationToken);
            }

            return response;
        }
    }
}
