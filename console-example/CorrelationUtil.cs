namespace InstanaSDKConsoleExample
{
    using Instana.Tracing.Api;
    using Instana.Tracing.Sdk.Spans;

    internal class CorrelationUtil
    {
        public static DistributedTraceInformation GetCorrelationFromCurrentContext()
        {
            // in our example we never leave the scope of our test-application, so the distribution
            // shown here is artificial. In real-world scenarios, you would put the 
            var context = CustomSpan.GetCurrentContext();

            if (context != null)
            {
                var dti = new DistributedTraceInformation();
                dti.ParentSpanId = context.GetSpanId();
                dti.TraceId = context.GetTraceId();
                return dti;
            }

            return null;
        }
    }
}
