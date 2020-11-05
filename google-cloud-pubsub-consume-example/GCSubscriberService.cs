using Google.Cloud.PubSub.V1;
using Instana.Tracing.Api;
using Instana.Tracing.Sdk.Spans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace google_cloud_pubsub_consume_example
{
    class GCSubscriberService
    {
        public void StreamingPull()
        {
            // First create a topic.
            PublisherServiceApiClient publisherService = PublisherServiceApiClient.CreateAsync().Result;
            TopicName topicName = new TopicName("k8s-brewery", "sdk-example-test-topic");
            try
            {
                publisherService.CreateTopic(topicName);
            }
            catch
            {

            }

            // Subscribe to the topic.
            SubscriberServiceApiClient subscriberService = SubscriberServiceApiClient.CreateAsync().Result;
            SubscriptionName subscriptionName = new SubscriptionName("k8s-brewery", "sdk-example-test-subscription");
            try
            {
                subscriberService.CreateSubscription(subscriptionName, topicName, pushConfig: null, ackDeadlineSeconds: 60);
            }
            catch
            {

            }

            // Pull messages from the subscription using SubscriberClient.
            SubscriberClient subscriber = SubscriberClient.CreateAsync(subscriptionName).Result;
            List<PubsubMessage> receivedMessages = new List<PubsubMessage>();
            // Start the subscriber listening for messages.
            subscriber.StartAsync((msg, cancellationToken) =>
            {
                using (var span = CustomSpan.Create()
                        .AsGCPubSubReceive(subscriptionName.SubscriptionId, subscriptionName.ProjectId)
                        .AsChildOf(() => GetDisInfo(msg)))
                {
                    span.WrapAction(() =>
                    {
                        receivedMessages.Add(msg);
                        Console.WriteLine($"[Test] Received message {msg.MessageId} published at {msg.PublishTime.ToDateTime()}");
                        Console.WriteLine($"[Test] Text: '{msg.Data.ToStringUtf8()}'");
                    }, true);
                    return Task.FromResult(SubscriberClient.Reply.Ack);
                }
            });
        }

        public static DistributedTraceInformation GetDisInfo(PubsubMessage msg)
        {
            DistributedTraceInformation disInfo = new DistributedTraceInformation();

            string traceId = string.Empty;
            string parentId = string.Empty;

            if (msg.Attributes.TryGetValue(TracingConstants.ExternalTraceIdHeader, out traceId))
            {
                disInfo.TraceId = TraceIdUtil.GetLongFromHex(traceId);
            }

            if (msg.Attributes.TryGetValue(TracingConstants.ExternalParentSpanIdHeader, out parentId))
            {
                disInfo.ParentSpanId = TraceIdUtil.GetLongFromHex(parentId);
            }
            
            return disInfo;
        }
    }
}
