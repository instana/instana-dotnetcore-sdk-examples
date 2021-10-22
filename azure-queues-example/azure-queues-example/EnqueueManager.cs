using Azure.Storage.Queues;
using Instana.Tracing.Sdk.Spans;

namespace azure_queues_example
{
    public class EnqueueManager
    {
        public void Enqueue(Message message, QueueClient queue)
        {
            using (var exitSpan = CustomSpan.CreateExit(this, message.AddCorrelationData))
            {
                queue.SendMessage(message.Body);
            }
        }
    }

}
