using Instana.Tracing.Api;
using Instana.Tracing.Sdk.Spans;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace custom_messaging_example
{
    public class MessageRouter
    {
        private static List<string> _topics = new List<string>();
        private static Dictionary<string, MessageConsumer> _topicToConsumer = new Dictionary<string, MessageConsumer>();
        private static bool _listening = false;
        private static Task _listenerTask;

        private static BlockingCollection<Message> _messages = new BlockingCollection<Message>();
        public static void RegisterTopic(string topicName)
        {
            if (!_topicToConsumer.ContainsKey(topicName))
            {
                _topicToConsumer.Add(topicName, new MessageConsumer() { Topic = topicName });
                Listen();
            }
        }

        public static void PostMessage(Message msg)
        {
            _messages.Add(msg);
        }
        private static void Listen()
        {
            if(_listening)
            {
                return;
            }
            _listening = true;
            foreach (string topic in _topics)
            {
                MessageConsumer consumer = new MessageConsumer() { Topic = topic };
            }
            
            _listenerTask = Task.Factory.StartNew(() =>
            {
                foreach (Message msg in _messages.GetConsumingEnumerable())
                {
                    using (var entrySpan = CustomSpan.CreateEntry(null, () => ExtractCorrelation(msg)).AsMessageReceiveFrom(msg.Topic))
                    {
                        if (_topicToConsumer.ContainsKey(msg.Topic))
                        {
                            _topicToConsumer[msg.Topic].HandleMessage(msg);
                        }
                    }
                }
            });
        }

        private static DistributedTraceInformation ExtractCorrelation(Message msg)
        {
            if (!msg.Headers.ContainsKey(TracingConstants.ExternalTraceIdHeader) ||
               !msg.Headers.ContainsKey(TracingConstants.ExternalParentSpanIdHeader))
            {
                return null;
            }

            DistributedTraceInformation info = new DistributedTraceInformation();
            info.ParentSpanId = SpanIdUtil.FromExternalSpanId(msg.Headers[TracingConstants.ExternalParentSpanIdHeader]);
            info.TraceId = SpanIdUtil.FromExternalSpanId(msg.Headers[TracingConstants.ExternalTraceIdHeader]);
            return info;
        }
    }
}
