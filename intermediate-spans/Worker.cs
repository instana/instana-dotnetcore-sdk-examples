using Instana.Tracing.Sdk.Spans;
using System.Text;
using System.Threading;

namespace intermediate_spans
{
    internal class Worker
    {
        public string DoSomeWork(string input)
        {
            StringBuilder builder = new StringBuilder(input);
            DoThis(builder);
            DoThat(builder);
            return builder.ToString();
        }

        public void DoThis(StringBuilder state)
        {
            // create one intermediate span for this subtask
            using (var thisSpan = CustomSpan.Create(this, SpanType.INTERMEDIATE))
            {
                state.Append("...This...");
                // and annotate the span with something for us to see
                thisSpan.SetTag("state", state.ToString());
                Thread.Sleep(2);
            }
        }

        private void DoThat(StringBuilder state)
        {
            // create one intermediate span for this subtask
            using (var thatSpan = CustomSpan.Create(this, SpanType.INTERMEDIATE))
            {
                state.Append("...That...");
                // and annotate the span with something for us to see
                thatSpan.SetTag("state", state.ToString());
                Thread.Sleep(5);
            }

        }
    }
}
