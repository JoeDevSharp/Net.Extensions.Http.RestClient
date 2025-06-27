using Net.Extensions.OAuth2.Abstracts;

namespace Net.Extensions.Http.RestClient
{
    public class AuthProviderHandler : DelegatingHandler
    {
        private readonly IAuthProvider _authProvider;

        public AuthProviderHandler(IAuthProvider authProvider)
            : base(new HttpClientHandler()) 
        {
            _authProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authUser = await _authProvider.LoginAsync();

            if (authUser is not null)
            {
                if (request.Headers.Contains("Authorization"))
                    request.Headers.Remove("Authorization");

                request.Headers.Add("Authorization", $"Bearer {authUser.Token}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
