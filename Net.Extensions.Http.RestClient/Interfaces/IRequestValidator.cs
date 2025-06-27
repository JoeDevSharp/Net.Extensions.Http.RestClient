namespace Net.Extensions.Http.RestClient.Interface
{
    public interface IRequestValidator
    {
        /// <summary>
        /// Valida la petición antes de enviarla.
        /// Retorna true si es válida, false en caso contrario.
        /// </summary>
        /// <param name="request">La petición a validar</param>
        /// <param name="errorMessage">Mensaje de error si la validación falla</param>
        bool Validate(RestRequest request, out string? errorMessage);
    }
}
