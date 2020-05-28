namespace InstanaSDKConsoleExample
{
    using Instana.Tracing.Sdk.Spans;
    using System;
    using System.Collections.Generic;

    internal class Pipeline
    {
        List<IStep> _pipelineSteps;

        public Pipeline()
        {
            _pipelineSteps = new List<IStep>();
        }

        public void AddStep(IStep pipelineStep)
        {
            _pipelineSteps.Add(pipelineStep);
        }

        public void ExecuteJob(string input, Dictionary<string, object> stepResults)
        {
            // let's create the root for our trace. A trace always starts with an entry
            // (something enters a system, which leads to a reaction)
            using (var traceRoot = CustomSpan.CreateEntryForNewTrace(this))
            {
                // since we want to collect errors happening during the process, we
                // wrap the call, so that any exception will be automatically collected
                // and added to our trace, at the call where it happened.
                traceRoot.WrapAction(() =>
                {
                    string mutatedInput = input;
                    Guid jobId = Guid.NewGuid();
                    // add some tags to our trace, so that we can identify it later
                    // based on our own data, not instana internals
                    traceRoot.SetTag(new string[] { "JobId" }, jobId.ToString());
                    traceRoot.SetTag(new string[] { "Pipeline-Length" }, _pipelineSteps.Count.ToString());
                    traceRoot.SetTag(new string[] { "Input" }, input);
                    
                    // execute each step in the pipeline.
                    // we're pretending to have an exit-call for every step
                    // (it _could_ be a remote-service, but in this example it isn't...)
                    foreach (IStep step in _pipelineSteps)
                    {
                        using (var exit = CustomSpan.CreateExit(this, null))
                        {
                            // each step could throw an exception, so wrap the call
                            // to extract the error-information and let it automatically be 
                            // added to our trace...
                            // after collecting the exception, bubble it up
                            exit.WrapAction(() =>
                            {
                                mutatedInput = step.Execute(mutatedInput, stepResults);
                            }, true);
                        }
                    }


                }, true);

            }
        }
    }
}
