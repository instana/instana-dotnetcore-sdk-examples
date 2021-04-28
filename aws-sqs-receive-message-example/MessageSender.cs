using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSSQSExample
{
    public class MessageSender
    {


        public static async Task SendMessageAsync(string queuename, IAmazonSQS amazonSQS)
        {
            SendMessageRequest sendMessageRequest = new SendMessageRequest()
            {
                QueueUrl = "http://localhost:9324/queue/test",
                MessageBody = $"Test at {DateTime.UtcNow.ToLongDateString()}"
            };

            SendMessageResponse sendMessageResponse = await amazonSQS.SendMessageAsync(sendMessageRequest);

            Console.WriteLine("Message is sent");
        }
    }
}
