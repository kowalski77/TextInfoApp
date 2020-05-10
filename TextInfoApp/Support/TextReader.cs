using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace TextInfoApp.Support
{
    public static class TextReader
    {
        public static async Task<string> ReadWebApiAsync(string webApiName, CancellationToken cancellationToken)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"TextInfoApp.Resources.{webApiName}.txt";

            //Simulates a random delay call
            //await Task.Delay(3000, cancellationToken);

            await using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream ??
                                                throw new InvalidOperationException($"Wrong resource: {resourceName}"));

            return await reader.ReadToEndAsync();
        }

        private static int GetRandomWaitingTime()
        {
            var random = new Random();
            return random.Next(1000, 5000);
        }
    }
}