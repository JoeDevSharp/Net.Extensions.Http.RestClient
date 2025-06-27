# Net.Extensions.Http.RestClient

Cliente HTTP REST ligero, modular y fuertemente tipado para .NET, diseñado para simplificar llamadas a APIs REST con control total sobre autenticación, validación y políticas de resiliencia (retry, timeout, circuit breaker).

---

## Características

- **API sencilla y fluida** basada en modelos `RestClient`, `RestRequest`, `RestResponse`.
- **Soporte nativo de autenticación** mediante tokens con `IAuthProvider` y `AuthProviderHandler`.
- **Políticas configurables** de retry, timeout y circuit breaker para robustez.
- **Validación integrada**: valida requests y respuestas con interfaces `IRequestValidator` y `IResponseValidator`.
- **Construcción segura de rutas** con parámetros a través de `IHttpRouteBuilder` y `HttpRoute`.
- Serialización y deserialización optimizada con `System.Text.Json`.
- Modular, extensible y sin dependencias externas pesadas.
- Compatible con .NET 6+.

---

## Instalación

Actualmente no está publicado en NuGet. Para usarlo:

- Incluye el proyecto `Net.Extensions.Http.RestClient` en tu solución.
- Referencia el proyecto desde tu aplicación.

---

## Uso básico

```csharp
using Net.Extensions.Http.RestClient.Core;
using System.Net.Http;

var httpClient = new HttpClient { BaseAddress = new Uri("https://api.example.com/") };
var restClient = new RestClient(httpClient);

var request = new RestRequest("users/123", HttpMethod.Get);
var response = await restClient.SendAsync<User>(request);

if (response.IsSuccess)
{
    Console.WriteLine($"Usuario: {response.Data.Name}");
}
else
{
    Console.WriteLine($"Error: {response.ErrorMessage}");
}
````

---

## Autenticación

Implementa la interfaz `IAuthProvider` para proveer tokens:

```csharp
public class MyAuthProvider : IAuthProvider
{
    public Task<string?> GetTokenAsync()
    {
        // Devuelve token JWT o similar
        return Task.FromResult("token-aqui");
    }
}
```

Luego añade el handler en el pipeline `HttpClient`:

```csharp
var authHandler = new AuthProviderHandler(new MyAuthProvider());
var httpClient = new HttpClient(authHandler)
{
    BaseAddress = new Uri("https://api.example.com/")
};
var restClient = new RestClient(httpClient);
```

---

## Políticas de resiliencia

Configura y aplica políticas para retry, timeout y circuit breaker para mayor robustez:

```csharp
var retryPolicy = new RetryPolicy(maxRetries: 3, delay: TimeSpan.FromSeconds(2));
var timeoutPolicy = new TimeoutPolicy(TimeSpan.FromSeconds(10));
var circuitBreaker = new CircuitBreakerPolicy(failureThreshold: 5, openDuration: TimeSpan.FromSeconds(30));

var response = await retryPolicy.ExecuteAsync(() =>
    timeoutPolicy.ExecuteAsync(ct =>
        circuitBreaker.ExecuteAsync(() =>
            restClient.SendAsync<User>(request)
        )
    )
);
```

---

## Validación

Puedes validar requests antes de enviarlos e inspeccionar respuestas con validadores personalizados:

```csharp
public class MyRequestValidator : IRequestValidator
{
    public bool Validate(RestRequest request, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(request.Resource))
        {
            errorMessage = "El recurso es obligatorio";
            return false;
        }
        errorMessage = null;
        return true;
    }
}

public class MyResponseValidator : IResponseValidator
{
    public bool Validate<T>(T responseData, out string? errorMessage)
    {
        if (responseData == null)
        {
            errorMessage = "Respuesta vacía";
            return false;
        }
        errorMessage = null;
        return true;
    }
}
```

---

## Construcción de rutas con parámetros

```csharp
var route = new HttpRoute()
    .WithTemplate("users/{userId}/orders/{orderId}")
    .AddParameter("userId", "123")
    .AddParameter("orderId", "456")
    .Build(); // "users/123/orders/456"
```

Útil para construir URLs dinámicas seguras.

---

## Contribuir

* Reporta issues y bugs.
* Envía pull requests con mejoras o nuevas funcionalidades.
* Sigue la convención y mantén el código claro y minimalista.

---

## Licencia

MIT License.

---

¿Quieres que te prepare ejemplos listos para copiar y pegar o integración con DI?
