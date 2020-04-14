namespace InstanaSDKConsoleExample
{
    using Instana.Tracing.Sdk.Spans;
    using System;
    using System.Collections.Generic;

    internal class ValidationStep : IStep
    {
        public string Execute(string input, Dictionary<string, object> protocol)
        {
            // for every step we create an entry (remember that we create an exit for every call 
            // from the pipeline to an IStep-implementation). We correlate based on the current context
            // since we never really leave the boundaries of the app. In remote scenarios though, this would
            // look differently.
            using (var span = CustomSpan.CreateEntry(this, CorrelationUtil.GetCorrelationFromCurrentContext))
            {
                // wrap the call and bubble exceptions up to gather error-information
                span.WrapAction(() =>
                {
                    span.SetTag(new string[] { "Input" }, input);
                    if (string.IsNullOrEmpty(input))
                    {
                        protocol["Validation-Result"] = "Invalid";
                        throw new ArgumentException("The input for this job was invalid!");
                    }
                    protocol["Validation-Result"] =  "Valid";
                }, true);
                return input;
            }
        }
    }
}
