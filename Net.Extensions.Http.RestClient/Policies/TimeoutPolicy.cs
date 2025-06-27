namespace Net.Extensions.Http.RestClient.Policies
{
    public class TimeoutPolicy
    {
        public TimeSpan Timeout { get; }

        public TimeoutPolicy(TimeSpan? timeout = null)
        {
            Timeout = timeout ?? TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Ejecuta una tarea con timeout configurable.
        /// Lanza TimeoutException si se supera el tiempo.
        /// </summary>
        /// <param name="action">Función asíncrona a ejecutar</param>
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            using var cts = new CancellationTokenSource(Timeout);

            var task = action();
            var delayTask = Task.Delay(Timeout, cts.Token);

            var completedTask = await Task.WhenAny(task, delayTask);

            if (completedTask != task)
                throw new TimeoutException($"La operación excedió el timeout de {Timeout.TotalSeconds} segundos.");

            cts.Cancel(); // Cancelar delay si la tarea terminó antes

            return await task; // Resultado o excepción de la tarea original
        }
    }
}
