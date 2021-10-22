using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Instana.Tracing.Api;
using Instana.Tracing.Sdk.Spans;
using System;

namespace azure_queues_example
{
    public class DequeueManager
    {
        public void Dequeue(int count, QueueClient queue)
        {
            foreach (QueueMessage queueMessage in queue.ReceiveMessages(count).Value)
            {
                using(var entrySpan = CustomSpan.CreateEntry(null, () => ExtractCorrelationData(queueMessage.Body.ToString())))
                {
                    HandleMessage(queueMessage, queue);
                }
            }
        }

        private static DistributedTraceInformation ExtractCorrelationData(string messageBody)
        {
            string[] messageParts = messageBody.Split('|');
            string[] correlationParts1 = messageParts[1].Split('=');
            string[] correlationParts2 = messageParts[2].Split('=');

            Console.WriteLine($"{correlationParts1[0]}: {correlationParts1[1]}");
            Console.WriteLine($"{correlationParts2[0]}: {correlationParts2[1]}");

            DistributedTraceInformation info = new DistributedTraceInformation();
            info.ParentSpanId = SpanIdUtil.FromExternalSpanId(correlationParts1[1]);
            info.TraceId = SpanIdUtil.FromExternalSpanId(correlationParts2[1]);

            return info;
        }

        private string ExtractMessageText(BinaryData messageBody)
        {
            string[] messageParts = messageBody.ToString().Split('|');
            return messageParts[0];
        }

        private void HandleMessage(QueueMessage queueMessage, QueueClient queue)
        {
            Console.WriteLine($"Message: {ExtractMessageText(queueMessage.Body)}");
            queue.DeleteMessage(queueMessage.MessageId, queueMessage.PopReceipt);
        }
    }
}
