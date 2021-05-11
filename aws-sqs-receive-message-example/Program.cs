using Amazon.SQS;
using System;
using System.Threading.Tasks;

namespace AWSSQSExample
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            IAmazonSQS amazonSQS = InitializeSQSClient();

            Console.WriteLine("Test is started");

            await MessageSender.SendMessageAsync("test", amazonSQS);
            await MessageReceiver.ReceiveMessageAsync(amazonSQS);

            Console.WriteLine("Test is complited");
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static IAmazonSQS InitializeSQSClient()
        {
            var sqsConfig = new AmazonSQSConfig();

            sqsConfig.ServiceURL = "http://localhost:9324";
            return new AmazonSQSClient(sqsConfig);
        }
    }
}
