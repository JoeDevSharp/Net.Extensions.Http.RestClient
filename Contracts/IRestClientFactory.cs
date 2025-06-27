namespace Contracts
{
    public interface IRestClientFactory
    {
        /// <summary>
        /// Crea una instancia de cliente REST para la interfaz TClient.
        /// </summary>
        /// <typeparam name="TClient">Tipo de cliente, normalmente interfaz</typeparam>
        /// <returns>Instancia del cliente</returns>
        TClient CreateClient<TClient>() where TClient : class;
    }
}
