
namespace custom_messaging_example
{
    using Instana.Tracing.Sdk.Spans;
    public class MessageClient
    {
        public void Connect(string topic)
        {
            MessageRouter.RegisterTopic(topic);
        }
        public void Send(Message msg)
        {
            using (var exitSpan = CustomSpan.CreateExit(this, msg.Headers.Add).AsMessageSendTo(msg.Topic))
            {
                MessageRouter.PostMessage(msg);
            }
        }
    }
}
