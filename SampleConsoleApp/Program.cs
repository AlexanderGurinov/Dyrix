using System;
using System.Threading.Tasks;
using Dyrix;
using Microsoft.Extensions.Configuration;

namespace SampleConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            RunAsync(configuration).Wait();
        }

        private static async Task RunAsync(IConfiguration configuration)
        {
            /*
            const string resource = "https://g666.crm4.dynamics.com";
            const string directoryId = "71b07708-1af0-453c-9098-24ac683a143e";
            const string clientId = "6f670244-2d51-40e1-837d-567dca629e66";
            const string clientSecret = "e/O1vKNELlWSEXFClo8LaAiwzvrabrM07hEjfN0Cz9Q=";

            or from secrets

            {
              "ConnectionStrings": {
                "DynamicsConnectionString": "Resource = https://g666.crm4.dynamics.com; DirectoryId = 71b07708-1af0-453c-9098-24ac683a143e; ClientId = 6f670244-2d51-40e1-837d-567dca629e66; ClientSecret = e/O1vKNELlWSEXFClo8LaAiwzvrabrM07hEjfN0Cz9Q="
              }
            }

            */
            using (var dynamics = await Dynamics.CreateAsync(configuration.GetConnectionString("DynamicsConnectionString")))
            {
                for (var i = 0; i < 10; i++)
                {
                    Console.WriteLine(await dynamics.GetJsonAsync("WhoAmI()"));
                }

                Console.WriteLine(await dynamics.GetJsonAsync("accounts?$select=name&$top=3"));
            }
        }
    }
}