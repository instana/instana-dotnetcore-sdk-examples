using Azure.Storage.Queues;
using Instana.Tracing.Sdk.Spans;
using System;
using System.Threading;

namespace azure_queues_example
{
    public static class Program
    {
        const string testConnectionString = "DefaultEndpointsProtocol=https;AccountName=queueinfratest;AccountKey=Hux6bQ1wH4EmDMLYQhveSKBChIX9xB+opyOZWoFvZfw4A4FsxTOVezFgZC2EUDKV7xqVrwH27/Q26yFnQptu6w==;EndpointSuffix=core.windows.net";
        const string testQueueName = "queue1";

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
                Message message = new Message("Simple text message");
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
