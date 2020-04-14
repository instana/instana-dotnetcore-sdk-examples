
namespace InstanaSDKConsoleExample
{
    using Instana.Tracing.Sdk.Spans;
    using System.Collections.Generic;

    internal class WordcountStep : IStep
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
                    string[] words = input.Split(new char[] { ',', ' ', '!', '.', '?' });
                    int icount = 0;
                    foreach (string word in words)
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            icount++;
                        }
                    }
                    protocol["Word-Count"] = icount;
                }, true);
            }
            return input;
        }
    }
}
