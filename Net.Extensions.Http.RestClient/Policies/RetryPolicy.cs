namespace Net.Extensions.Http.RestClient.Policies
{
    public class RetryPolicy
    {
        public int MaxRetries { get; }
        public TimeSpan Delay { get; }

        public RetryPolicy(int maxRetries = 3, TimeSpan? delay = null)
        {
            if (maxRetries < 0) throw new ArgumentOutOfRangeException(nameof(maxRetries));
            MaxRetries = maxRetries;
            Delay = delay ?? TimeSpan.FromSeconds(2);
        }

        /// <summary>
        /// Ejecuta la función asíncrona con reintentos en caso de fallo considerado transitorio.
        /// </summary>
        /// <param name="action">Función que ejecuta la operación</param>
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            int attempt = 0;

            while (true)
            {
                try
                {
                    return await action();
                }
                catch (HttpRequestException) when (attempt < MaxRetries)
                {
                    attempt++;
                    await Task.Delay(Delay);
                }
            }
        }
    }
}
