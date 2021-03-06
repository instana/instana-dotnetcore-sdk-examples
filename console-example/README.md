# Console Example for custom traces

The application in this folder is a simple Console-application written with the .NET Core Framework.
This example relies exclusively on custom tracing: no automatic tracing will be used.

## What will you learn

This example shows how you can leverage the capabilities of the SDK to instrument your own libraries and get valuable information. 
Using tags that you can add lets you easily integrate this kind of traces with Application Perspectives in Instana, when you will be able to use these tags and their values to [create Application Perspective](https://docs.instana.io/application_monitoring/application-perspectives#create-an-application-perspective), as we all to refine queries in [Unbounded Analytics](https://docs.instana.io/tracing/analytics/).

## What does this example contain

The project references the `Instana.Tracing.Core.Sdk` nuget package and uses the contained classes to create a fully customized tracing for the app.
Internally the application creates a pipeline (a list of steps to be taken).
The user can start a job by entering any text to the console when prompted.
The application will then execute the job by passing it into the pipeline.
The trace will start with the pipeline being called and have calls for every step within the pipeline.
Errors - if any - will be added to the current steps and can be seen in the traces when sent to Instana.

## How to run this example

Download the source-code and compile the application.
You do not really need to add any environment-variables for this example since we're not relying on automatic tracing.
For convenience, we added the usual `run.ps1` script that does the build, publish and run for you.
You can simple reuse the script when you incorporate changes to the application for your own experiments.

## Show-cased mechanics

The example application creates a trace with several child-calls.
Although there is no "real" distribution (as in "calling a remote service") it uses pairs of exit- and entry-spans to display the pattern to be applied in distributed scenarios, without going into the specifics of custom call-correlation across system boundaries.

## APIs used

The application uses the following APIs

* `CustomSpan.CreateEntryForNewTrace` => Starts a new trace without looking for any provided context for continuation
* `CustomSpan.CreateExit` => Used for a call leaving a subsystem (in the example leaving the pipeline to call the individual steps)
* `CustomSpan.CreateEntry` => Used inside the individual steps (directly following the `CreateExit`- call going ot from the pipeline)
* `CustomSpan.WrapAction` => Used for automatic error-handling / -collection
* `CustomSpan.SetTag` => Used for custom annotations to the span to facilitate retrieval
