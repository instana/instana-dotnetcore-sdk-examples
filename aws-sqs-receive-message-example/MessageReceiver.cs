using Amazon.SQS;
using Amazon.SQS.Model;
using Instana.Tracing.Api;
using Instana.Tracing.Sdk.Spans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSSQSExample
{
    class MessageReceiver
    {
        public static async Task ReceiveMessageAsync(IAmazonSQS amazonSQS)
        {
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest("http://localhost:9324/queue/test");
            receiveMessageRequest.MaxNumberOfMessages = 10;

            using (var span = CustomSpan.Create()
                       .AsAWSSQSMessageReceive(receiveMessageRequest.QueueUrl))
            {

                Message correlationMessage = null;
                ReceiveMessageResponse r = await amazonSQS.ReceiveMessageAsync(receiveMessageRequest);
                List<DeleteMessageBatchRequestEntry> deleteReqEntries = new List<DeleteMessageBatchRequestEntry>(r.Messages.Count);
                correlationMessage = r.Messages.FirstOrDefault();

                span.WrapAction(() =>
                {
                    foreach (var message in r.Messages)
                    {
                        Console.WriteLine("Messageid: " + message.MessageId);
                        Console.WriteLine("Message body:" + message.Body);
                        Console.WriteLine("Recepit: " + message.ReceiptHandle);
                        Console.WriteLine("MD5Body: " + message.MD5OfBody);
                        Console.WriteLine();

                        deleteReqEntries.Add(new DeleteMessageBatchRequestEntry(message.MessageId, message.ReceiptHandle));
                    }


                }, true);

                span.AsChildOf(() => GetDisInfo(correlationMessage));

                DeleteMessageBatchRequest deleteMessageBatchRequest = new DeleteMessageBatchRequest("http://localhost:9324/queue/test", deleteReqEntries);
                var response = await amazonSQS.DeleteMessageBatchAsync(deleteMessageBatchRequest);
            }
        }

        public static DistributedTraceInformation GetDisInfo(Message msg)
        {
            DistributedTraceInformation disInfo = new DistributedTraceInformation();

            if (msg == null) return disInfo;

            if (msg.MessageAttributes.TryGetValue(TracingConstants.ExternalTraceIdHeader, out MessageAttributeValue traceIdAttributeValue))
            {
                disInfo.TraceId = TraceIdUtil.GetLongFromHex(traceIdAttributeValue.StringValue);
            }

            if (msg.MessageAttributes.TryGetValue(TracingConstants.ExternalParentSpanIdHeader, out MessageAttributeValue parentIdAttributeValue))
            {
                disInfo.ParentSpanId = TraceIdUtil.GetLongFromHex(parentIdAttributeValue.StringValue);
            }

            return disInfo;
        }
    }
}
