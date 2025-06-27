namespace Contracts
{
    public interface IHttpRouteBuilder
    {
        /// <summary>
        /// Define el template de la ruta con placeholders, ejemplo: "api/users/{userId}/orders/{orderId}"
        /// </summary>
        /// <param name="template">Template de ruta</param>
        /// <returns>El builder para encadenar</returns>
        IHttpRouteBuilder WithTemplate(string template);

        /// <summary>
        /// Asigna un valor para un parámetro de la ruta
        /// </summary>
        /// <param name="name">Nombre del parámetro (sin llaves)</param>
        /// <param name="value">Valor a reemplazar</param>
        /// <returns>El builder para encadenar</returns>
        IHttpRouteBuilder AddParameter(string name, string value);

        /// <summary>
        /// Construye y retorna la ruta con parámetros reemplazados
        /// </summary>
        string Build();
    }
}
