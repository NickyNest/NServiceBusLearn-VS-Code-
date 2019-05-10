using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Sales
{
    class Program
    {
        static async Task Main(string[] args) 
        {
            Console.Title = "Sales";

            var endpointConfiguration = new EndpointConfiguration("Sales");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(immediate => { immediate.NumberOfRetries(2); });
            recoverability.Delayed(delayed => 
            {
                delayed.NumberOfRetries(1);
                delayed.TimeIncrease(TimeSpan.FromSeconds(5));
            });

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
