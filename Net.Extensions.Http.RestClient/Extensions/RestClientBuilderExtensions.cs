using Auth;
using Auth.Interfaces;
using Policies;

namespace Net.Extensions.Http.RestClient
{
    public static class RestClientBuilderExtensions
    {
        /// <summary>
        /// Sets the base URL for the RestClient.
        /// </summary>
        public static RestClient WithBaseUrl(this RestClient client, string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL cannot be null or empty.", nameof(baseUrl));

            client.SetBaseAddress(new Uri(baseUrl));
            return client;
        }

        /// <summary>
        /// Adds an authentication provider to automatically add Authorization headers.
        /// </summary>
        public static RestClient WithAuthProvider(this RestClient client, IAuthProvider authProvider)
        {
            if (authProvider == null)
                throw new ArgumentNullException(nameof(authProvider));

            client.SetAuthHandler(new AuthProviderHandler(authProvider));
            return client;
        }

        /// <summary>
        /// Configures the HttpClient timeout.
        /// </summary>
        public static RestClient WithTimeout(this RestClient client, TimeSpan timeout)
        {
            if (timeout <= TimeSpan.Zero)
                throw new ArgumentException("Timeout must be greater than zero.", nameof(timeout));

            client.SetTimeout(timeout);
            return client;
        }

        /// <summary>
        /// Adds a retry policy to the client.
        /// </summary>
        public static RestClient WithRetryPolicy(this RestClient client, int maxRetries, TimeSpan delayBetweenRetries)
        {
            var retryPolicy = new RetryPolicy(maxRetries, delayBetweenRetries);
            client.SetRetryPolicy(retryPolicy);
            return client;
        }

        /// <summary>
        /// Adds a timeout policy with a maximum allowed duration per request.
        /// </summary>
        public static RestClient WithTimeoutPolicy(this RestClient client, TimeSpan maxDuration)
        {
            var timeoutPolicy = new TimeoutPolicy(maxDuration);
            client.SetTimeoutPolicy(timeoutPolicy);
            return client;
        }
    }
}
