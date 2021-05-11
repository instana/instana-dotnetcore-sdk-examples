# Example for consuming Google Cloud PubSub messages

The application in this folder is a simple Console-application written with the .NET Core Framework.

## Learnings

What you learn here is how you can make your custom Google Cloud PubSub spans for Consume calls. This makes it easier to look at and understand complex traces where you have correlation between Google Cloud Publish calls and Google Cloud Consume calls.

## Anatomy

The project references the `Instana.Tracing.Core.Sdk` nuget package and uses the contained classes to create a customized tracing for the app.
The application simulates publishing messages on Google PubSub and creates traces for those calls (this is automatically supported by `Instana.Tracing.Core`). Also application simulates consuming these messages where each processing of message should be wrapped with custom created span.
This scenario would apply to situations where you are using Google Cloud PubSub and where you need span for each received message.
After starting the app, two publishers will be created start sending messages. One publisher sends message periodically every two seconds. For that publisher exists appropriate subscriber what consume these messages in StrimingPull mode. The second publisher sends one message per one application run. The corresponding subscriber should consume one message but just in one Pull.
SDK spans for consume calls should be created for both subscribers and correlation with appropriate publish calls should be created.

## How to run

Just download the code, publish the app, set the env-vars and start it.
For convenience we added a script, which does all of that for you.

```powershell
PS > ./run.ps1
```

After building the code and publishing the app, the script will set the correct env-vars and run the application.

## Mechanisms shown

The example application creates a traces with two child-calls. It's main purpose is to show you how you can add traces for your consuming of Google Cloud PubSub where correlation is maintained.

## APIs used

The application uses the following APIs

* `CustomSpan.Create` => Create new span
* `CustomSpan.WrapAction` => Used for automatic error-handling
* `CustomSpan.AsGCPubSubReceive` => Used for mimic incoming calls from Google Pub Sub
* `CustomSpan.AsChildOf` => Used for mark created span as child of already existing trace which is finished with Publish onto Google Pub Sub

