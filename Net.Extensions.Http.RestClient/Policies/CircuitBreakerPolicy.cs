namespace Net.Extensions.Http.RestClient.Policies
{
    public class CircuitBreakerPolicy
    {
        private readonly int _failureThreshold;
        private readonly TimeSpan _openDuration;

        private int _failureCount;
        private DateTime? _circuitOpenedAt;
        private readonly object _lock = new();

        public CircuitBreakerPolicy(int failureThreshold = 5, TimeSpan? openDuration = null)
        {
            if (failureThreshold <= 0) throw new ArgumentOutOfRangeException(nameof(failureThreshold));
            _failureThreshold = failureThreshold;
            _openDuration = openDuration ?? TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Ejecuta la acción respetando el estado del circuito.
        /// Lanza InvalidOperationException si el circuito está abierto.
        /// </summary>
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            lock (_lock)
            {
                if (_circuitOpenedAt.HasValue)
                {
                    var elapsed = DateTime.UtcNow - _circuitOpenedAt.Value;
                    if (elapsed < _openDuration)
                        throw new InvalidOperationException("Circuit breaker abierto, operación bloqueada.");
                    else
                        ResetCircuit();
                }
            }

            try
            {
                var result = await action();

                lock (_lock)
                {
                    ResetCircuit();
                }

                return result;
            }
            catch
            {
                lock (_lock)
                {
                    _failureCount++;
                    if (_failureCount >= _failureThreshold)
                    {
                        _circuitOpenedAt = DateTime.UtcNow;
                    }
                }
                throw;
            }
        }

        private void ResetCircuit()
        {
            _failureCount = 0;
            _circuitOpenedAt = null;
        }
    }
}
