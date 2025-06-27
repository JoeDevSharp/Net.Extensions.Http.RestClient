using Contracts;
using System.Text.RegularExpressions;

namespace Net.Extensions.Http.RestClient
{
    public class HttpRoute : IHttpRouteBuilder
    {
        private string _template = string.Empty;
        private readonly Dictionary<string, string> _parameters = new();

        private static readonly Regex _parameterRegex = new(@"\{(\w+)\}", RegexOptions.Compiled);

        public IHttpRouteBuilder WithTemplate(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentException("Template no puede ser vacío.", nameof(template));

            _template = template;
            _parameters.Clear();
            return this;
        }

        public IHttpRouteBuilder AddParameter(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nombre del parámetro no puede ser vacío.", nameof(name));

            _parameters[name] = value ?? throw new ArgumentNullException(nameof(value));
            return this;
        }

        public string Build()
        {
            if (string.IsNullOrEmpty(_template))
                throw new InvalidOperationException("Debe definir un template antes de construir la ruta.");

            string result = _template;

            var matches = _parameterRegex.Matches(_template);
            foreach (Match match in matches)
            {
                string paramName = match.Groups[1].Value;
                if (!_parameters.TryGetValue(paramName, out var paramValue))
                    throw new InvalidOperationException($"No se proporcionó valor para el parámetro '{paramName}'.");

                result = result.Replace(match.Value, Uri.EscapeDataString(paramValue));
            }

            return result;
        }
    }
}
