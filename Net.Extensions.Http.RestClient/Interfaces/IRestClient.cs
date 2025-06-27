using Net.Extensions.Http.RestClient.Policies;

namespace Net.Extensions.Http.RestClient.Interface
{
    public interface IRestClient
    {
        /// <summary>
        /// URL base para las peticiones
        /// </summary>
        Uri BaseUrl { get; }

        /// <summary>
        /// Envía una petición REST y retorna la respuesta tipada.
        /// </summary>
        Task<RestResponse<T>> SendAsync<T>(RestRequest request);

        /// <summary>
        /// Configura el handler de autenticación (ej. AuthProviderHandler).
        /// Puede lanzar excepción si el cliente ya está inicializado.
        /// </summary>
        void SetAuthHandler(HttpMessageHandler authHandler);

        /// <summary>
        /// Establece o cambia la URL base para las peticiones.
        /// </summary>
        /// <param name="baseAddress">Nueva URL base</param>
        void SetBaseAddress(Uri baseAddress);

        /// <summary>
        /// Configura el timeout para las peticiones HTTP.
        /// </summary>
        /// <param name="timeout">Duración del timeout</param>
        void SetTimeout(TimeSpan timeout);

        /// <summary>
        /// Configura la política de reintentos para las peticiones.
        /// </summary>
        /// <param name="retryPolicy">Implementación de política de retry</param>
        void SetRetryPolicy(RetryPolicy retryPolicy);

        /// <summary>
        /// Configura la política de timeout para las peticiones.
        /// </summary>
        /// <param name="timeoutPolicy">Implementación de política de timeout</param>
        void SetTimeoutPolicy(TimeoutPolicy timeoutPolicy);
    }
}
