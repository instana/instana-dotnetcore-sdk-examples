# Example for consuming Google Cloud PubSub messages

The application in this folder is a simple Console-application written with the .NET Core Framework.

## Learnings

What you learn here is how you can make your custom Azure Queue spans for Dequeue calls. This makes it easier to look at and understand complex traces where you have correlation between Azure Queue Enqueue (SendMessage) and Azure Queue Dequeue (ReceiveMessage & DeleteMessage) calls.

## Anatomy

The project references the `Instana.Tracing.Core.Sdk` nuget package and uses the contained classes to create a customized tracing for the app.
The application simulates sending messages to Azure Queue and creates traces for those calls (this is automatically supported by `Instana.Tracing.Core`). Also application simulates consuming these messages where each processing of message should be wrapped with custom created span.
This scenario would apply to situations where you are using Azure Queue and where you need span for each received message.
After starting the app, a Queue Client is created which sends a message to the corresponding queue. The message is then extended with correlation data.
On receiving the messages from the queue, one span is created for every received message and this message is later deleted from the queue (simulating the dequeue process). 
Correlation Data is read from received message and passed to a new span.

## How to run

Just download the code, publish the app, set the env-vars and start it.
For convenience we added a script, which does all of that for you.

```powershell
PS > ./run.ps1
```

After building the code and publishing the app, the script will set the correct env-vars and run the application.

## Mechanisms shown

The example application creates a traces with two child-calls. It's main purpose is to show you how you can add traces for your consuming of Azure Queues where correlation is maintained.

## APIs used

The application uses the following APIs

* `CustomSpan.CreateEntry` => Starts a new trace without looking for any provided context for continuation
* `CustomSpan.CreateExit` => Used for a call leaving a subsystem (in the example leaving the pipeline to call the individual steps)
* `CustomSpan.WrapAction` => Used for automatic error-handling / -collection


