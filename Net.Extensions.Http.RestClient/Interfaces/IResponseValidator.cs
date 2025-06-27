namespace Net.Extensions.Http.RestClient.Interfaces
{
    public interface IResponseValidator
    {
        /// <summary>
        /// Valida los datos de la respuesta después de deserializar.
        /// Retorna true si la validación pasa, false si falla.
        /// </summary>
        /// <typeparam name="T">Tipo de datos deserializados</typeparam>
        /// <param name="responseData">Datos deserializados</param>
        /// <param name="errorMessage">Mensaje de error si la validación falla</param>
        bool Validate<T>(T responseData, out string? errorMessage);
    }
}
