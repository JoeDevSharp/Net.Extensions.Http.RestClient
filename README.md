# Net.Extensions.Http.RestClient

**Type-safe, fluent and extensible HTTP client for .NET.**

Designed to simplify consuming REST APIs by combining powerful abstractions, flexible authentication integration, and a developer-friendly API.

---

## 🚀 Features

- ✅ Strongly typed request/response model
- 🔁 Built-in support for retry and timeout policies
- 🔐 Seamless integration with any authentication provider via `IAuthProvider`
- 🌐 Fully customizable request construction with fluent API
- ♻️ Reusable and composable
- 🔧 No reflection, no proxies, no black boxes
- 🎯 Ideal for Console Apps, WinForms, WPF, Blazor, MAUI, and Services

---

## 📦 Installation

```bash
Install-Package JoeDevSharp.Net.Extensions.Http.RestClient
```

---

## 🧩 Key Concepts

### `RestClient`

Core class to perform HTTP requests using fluent configuration and typed responses.

### `RestRequest`

Fluent builder for defining endpoint, method, headers, query parameters, and body.

### `RestResponse<T>`

Standard result wrapper with success flag, HTTP status code, typed data, and error message.

### `IAuthProvider`

Optional interface to inject authentication logic from `Net.Extensions.OAuth2` or custom providers.

---

## 🛠️ Example: Basic GET Request with OAuth2 Provider

```csharp
using Net.Extensions.Http.RestClient;
using Net.Extensions.Http.RestClient.Extensions;
using Net.Extensions.OAuth2.Providers;

var provider = new GoogleProvider(
    clientId: "your-client-id",
    clientSecret: "your-client-secret",
    redirectUri: "http://localhost:60000/"
);

var client = new RestClient()
    .WithAuthProvider(provider)
    .WithBaseUrl("https://reqres.in");

var request = new RestRequest("/api/users")
    .WithMethod(HttpMethod.Get)
    .AddQueryParam("page", "1");

var response = await client.SendAsync<UserListResponse>(request);

if (response.IsSuccess && response.Data != null)
{
    foreach (var user in response.Data.Data)
        Console.WriteLine($"{user.FirstName} {user.LastName} - {user.Email}");
}
```

---

## 🔍 Fluent Configuration Options

```csharp
client
    .WithBaseUrl("https://api.example.com")
    .WithAuthProvider(myAuthProvider)
    .WithRetryPolicy(maxRetries: 3, delayBetweenRetries: TimeSpan.FromSeconds(2))
    .WithTimeout(TimeSpan.FromSeconds(10))
    .WithTimeoutPolicy(TimeSpan.FromSeconds(5));
```

---

## 📄 Models

### `UserListResponse`

```csharp
public class UserListResponse
{
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public List<User> Data { get; set; }
    public Support Support { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Avatar { get; set; }
}

public class Support
{
    public string Url { get; set; }
    public string Text { get; set; }
}
```

---

## ✅ When to Use

- You want a **lightweight**, **strongly-typed** HTTP client.
- You need clean integration with an OAuth2 or custom authentication provider.
- You want **fluent APIs** without sacrificing control.
- You are building modern .NET apps where maintainability and clarity matter.

---

## 📃 License

MIT License — Free to use and modify in personal and commercial projects.

---

## 🧰 Coming Soon

- Support for request/response interceptors
- Built-in logging helpers
- Mock/testing framework

---

## ✉️ Feedback & Contributions

Feel free to submit issues or PRs. For collaboration, contact [JoeDevSharp](https://github.com/JoeDevSharp).
