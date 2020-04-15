using System;

namespace custom_messaging_example
{
    public  class MessageConsumer
    {
        public string Topic { get; set; }
        public void HandleMessage(Message msg)
        {
            Console.WriteLine("[CONSUMER]\t[" + this.Topic + "]\t Handled message:" + msg.Body);
        }
    }
}