using System;
using System.Threading.Tasks;
using Dyrix;

namespace SampleConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            const string resource = "https://g666.crm4.dynamics.com";
            const string directoryId = "71b07708-1af0-453c-9098-24ac683a143e";
            const string clientId = "6f670244-2d51-40e1-837d-567dca629e66";
            const string clientSecret = "e/O1vKNELlWSEXFClo8LaAiwzvrabrM07hEjfN0Cz9Q=";
            
            using (var dynamics = await Dynamics.CreateAsync(resource, directoryId, clientId, clientSecret))
            {
                for (var i = 0; i < 10; i++)
                {
                    Console.WriteLine(await dynamics.GetStringAsync("WhoAmI()"));
                }
            }
        }
    }
}