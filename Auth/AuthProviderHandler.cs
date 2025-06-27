using Auth.Interfaces;

namespace Auth
{
    public class AuthProviderHandler : DelegatingHandler
    {
        private readonly IAuthProvider _authProvider;

        public AuthProviderHandler(IAuthProvider authProvider)
        {
            _authProvider = authProvider ?? throw new System.ArgumentNullException(nameof(authProvider));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _authProvider.GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                if (request.Headers.Contains("Authorization"))
                    request.Headers.Remove("Authorization");

                request.Headers.Add("Authorization", $"Bearer {token}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
