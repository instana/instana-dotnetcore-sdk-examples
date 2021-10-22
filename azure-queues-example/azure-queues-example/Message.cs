using System.Text;

namespace azure_queues_example
{
    public class Message
    {
        public string Body { get; set; }

        public Message(string messageBody)
        {
            this.Body = messageBody;
        }

        public void AddCorrelationData(string instanaId, string instanaValue)
        {
            StringBuilder sb = new StringBuilder(this.Body);
            sb.Append("|").Append(instanaId).Append("=").Append(instanaValue);
            this.Body = sb.ToString();
        }
    }
}
