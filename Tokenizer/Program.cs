using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Tokenizer
{
    internal sealed class Program
    {
        private static async Task Main()
        {
            Console.Write("Client Id:");
            var clientId = Console.ReadLine();

            Console.Write("Client Secret:");
            var clientSecret = Console.ReadLine();
            
            Console.Write("Directory Id:");
            var directoryId = Console.ReadLine();
            
            Console.Write("Resource:");
            var resource = Console.ReadLine();

            var context = new AuthenticationContext($"https://login.windows.net/{directoryId}");
            var credential = new ClientCredential(clientId, clientSecret);

            var result = await context.AcquireTokenAsync(resource, credential).ConfigureAwait(false);

            Console.WriteLine("Token:");
            Console.WriteLine(result.AccessToken);

            Console.ReadKey(true);
        }
    }
}
