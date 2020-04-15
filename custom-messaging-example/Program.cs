using Instana.Tracing.Sdk.Spans;
using System;
using System.Threading;

namespace custom_messaging_example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Custom Messaging System v1.0");
            Console.WriteLine("----------------------------");
            MessageClient client = new MessageClient();
            client.Connect("awesome-topic");
            while(true)
            {
                Console.Write("Enter message > ");
                string message = Console.ReadLine();
                using (var rootSpan = CustomSpan.CreateEntryForNewTrace(null))
                {
                    client.Send(new Message() { Topic = "awesome-topic", Body = message });
                }

                Thread.Sleep(500);
            }
        }
    }
}
