using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace google_cloud_pubsub_consume_example
{
    public class GCPublisherService
    {
        CancellationTokenSource tokenSource;
        CancellationToken token;
        public GCPublisherService()
        {
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
        }

        public void StartPeriodicallyPublish(int timeIntervalInSeconds)
        {
            PublisherServiceApiClient publisher = PublisherServiceApiClient.Create();
            TopicName topicName = new TopicName("k8s-brewery", "sdk-example-test-topic");
            try
            {
                publisher.CreateTopic(topicName);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to create topic");
            }

            int j = 0;
            Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    // Publish a message to the topic.
                    PubsubMessage message = new PubsubMessage
                    {
                        Data = ByteString.CopyFromUtf8("Test message number " + j),   
                        Attributes =
                        {
                            { "description", "Simple text message number "+j }
                        }
                    };
                    publisher.Publish(topicName, new[] { message });
                    j++;
                    Thread.Sleep(timeIntervalInSeconds * 1000);
                }
            });
        }

        private void StopPeriodicallyPublish()
        {
            tokenSource.Cancel();
        }
    }
}
