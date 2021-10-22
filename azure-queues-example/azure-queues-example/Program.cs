using Azure.Storage.Queues;
using Instana.Tracing.Sdk.Spans;
using System;
using System.Threading;

namespace azure_queues_example
{
    public static class Program
    {
        const string testConnectionString = "{connection_string}";
        const string testQueueName = "{queue_name}";

        static void Main(string[] args)
        {
            Console.WriteLine("Azure Queue Enqueue-Dequeue System v1.0");
            Console.WriteLine("---------------------------------------");

            EnqueueManager enqueueManager = new EnqueueManager();
            DequeueManager dequeueManager = new DequeueManager();
            QueueClient queue = new QueueClient(testConnectionString, testQueueName);
            queue.Create();

            while (true)
            {
                Message message = new Message("Simple text message || with | separator | =included=");
                using (var rootSpan = CustomSpan.CreateEntryForNewTrace(null))
                {
                    enqueueManager.Enqueue(message, queue);
                }

                Thread.Sleep(500);

                Console.WriteLine("Reading messages: ");
                dequeueManager.Dequeue(10, queue);
            }
        }
    }

}
