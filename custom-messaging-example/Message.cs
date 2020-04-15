using System;
using System.Collections.Generic;
using System.Text;

namespace custom_messaging_example
{
    public class Message
    {
        public Dictionary<string, string> Headers { get; private set; }
        public string Topic { get; set; }
        public string Body { get; set; }
        public Message()
        {
            this.Headers = new Dictionary<string, string>();
        }

        public void AddHeader(string name, string value)
        {
            this.Headers.Add(name, value);
        }
    }
}
