using Net.Extensions.Http.RestClient;               // Core RestClient class
using Net.Extensions.Http.RestClient.Extensions;    // Fluent extension methods like WithBaseUrl, AddHeader, etc.
using Net.Extensions.OAuth2.Providers;
using Policies;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleAppExemple
{
    internal class Program
    {
        // Entry point of the application, using async Task Main (C# 7.1+)
        static async Task Main(string[] args)
        {
            await Send(); // Run the example
        }

        /// <summary>
        /// Demonstrates how to send a GET request using Net.Extensions.Http.RestClient.
        /// </summary>
        private static async Task Send()
        {
            var provider = new GoogleProvider(
                "550344567807-kd6kvfomtl9kjr7j4ro1ba8dhgjf02ap.apps.googleusercontent.com",
                "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",
                "http://localhost:60000/"
            );

            // STEP 1: Create a new RestClient and configure it with the base URL
            var client = new RestClient()
                .WithAuthProvider(provider)
                .WithBaseUrl("https://reqres.in"); // Base address for all requests

            // STEP 2: Build a request targeting /api/users?page=1
            var request = new RestRequest("/api/users")
                .AddHeader("x-api-key", "reqres-free-v1") // Optional custom header (not required by reqres.in)
                .WithMethod(HttpMethod.Get)               // Set the HTTP method to GET
                .AddQueryParam("page", "1");              // Add query parameter ?page=1

            // STEP 3: Send the request and await the deserialized response of type UserListResponse
            var response = await client.SendAsync<UserListResponse>(request);

            // STEP 4: Check if the response is successful and contains valid data
            if (response.IsSuccess && response.Data != null)
            {
                // Display basic pagination info
                Console.WriteLine($"📄 Page {response.Data.Page} of {response.Data.TotalPages}");
                Console.WriteLine();

                // Iterate through each user in the response and print their details
                foreach (var user in response.Data.Data)
                {
                    Console.WriteLine($"👤 {user.FirstName} {user.LastName}");
                    Console.WriteLine($"📧 {user.Email}");
                    Console.WriteLine($"🖼️  Avatar: {user.Avatar}");
                    Console.WriteLine(new string('-', 40));
                }

                // Print support message and URL from the API response
                Console.WriteLine();
                Console.WriteLine($"💬 Support: {response.Data.Support.Text}");
                Console.WriteLine($"🔗 URL: {response.Data.Support.Url}");
            }
            else
            {
                // If the request failed or returned invalid data, print the error
                Console.WriteLine($"❌ Request failed: {response.ErrorMessage}");
            }

            // Prevent the console window from closing immediately
            Console.ReadLine();
        }
    }
}
