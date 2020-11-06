using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Instana.Tracing.Sdk.Spans;
using System;
using System.Linq;

namespace google_cloud_pubsub_consume_example
{
    class Program
    {
        static void Main(string[] args)
        {
            GCPublisherService gcPublisherService = new GCPublisherService();
            GCSubscriberService gcSubscriberService = new GCSubscriberService();
            gcPublisherService.StartPeriodicallyPublish(2000);
            gcSubscriberService.StreamingPull();

            PublisherServiceApiClient publisher = PublisherServiceApiClient.Create();
            TopicName topicName = new TopicName("k8s-brewery", "sdk-example-test-topic-2");
            try
            {
                publisher.CreateTopic(topicName);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to create topic");
            }

            // Publish a message to the topic.
            PubsubMessage message = new PubsubMessage
            {
                Data = ByteString.CopyFromUtf8("Message "),
                Attributes =
                {
                    { "Description", "Simple text message " }
                }
            };
            publisher.Publish(topicName, new[] { message });

            SubscriberServiceApiClient subscriber = SubscriberServiceApiClient.Create();
            SubscriptionName subscriptionName = new SubscriptionName("k8s-brewery", "sdk-example-test-subscription-2");
            try
            {
                subscriber.CreateSubscription(subscriptionName, topicName, pushConfig: null, ackDeadlineSeconds: 60);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to create subscription");
            }

            PullResponse response = subscriber.Pull(subscriptionName, returnImmediately: true, maxMessages: 100);
            foreach (ReceivedMessage received in response.ReceivedMessages)
            {
                using (var span = CustomSpan.Create()
                    .AsGCPubSubReceive(subscriptionName.SubscriptionId, subscriptionName.ProjectId)
                    .AsChildOf(() => GCSubscriberService.GetDisInfo(received.Message)))
                {
                    span.WrapAction(() =>
                    {
                        PubsubMessage msg = received.Message;
                        Console.WriteLine($"Received message {msg.MessageId} published at {msg.PublishTime.ToDateTime()}");
                        Console.WriteLine($"Text: '{msg.Data.ToStringUtf8()}'");
                        Console.WriteLine($"Attributes: '{string.Join(",", msg.Attributes.Select(x => $"{x.Key}-{x.Value}"))}'");

                    }, true);
                }
            }
            if (response.ReceivedMessages.Count > 0) subscriber.Acknowledge(subscriptionName, response.ReceivedMessages.Select(m => m.AckId));

            Console.WriteLine("Press any key to close ...");
            Console.ReadKey();
        }
    }
}
