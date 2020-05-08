# Example for calling a custom database / database that is not supported by automatic tracing

The application in this folder is a simple Console-application written with the .NET Core Framework.

## Learnings

What you learn here is how you can make your custom spans appear as typed spans (like Database, Http, Messaging, RPC, etc.). This makes it easier to look at and understand complex traces and also allows for a better understanding of which type of calls contributes most to your transactions' duration.

## Anatomy

The project references the `Instana.Tracing.Core.Sdk` nuget package and uses the contained classes to create a fully customized tracing for the app.
The application simulates calls into a database-system and creates traces for those calls (exit-calls towards the database).
This scenario would apply to situations where you are using a database-system which Instana's automatic tracing does not support.
After starting the app, you can write a database-statement to the console and hit enter. The application will then create a trace showing the start
of the trace (after hitting return), displayed as an http-entry (but it could be anything else), followed by the calls to the database-client (class DatabaseClient).

## How to run

Just download the code, publish the app, set the env-vars and start it.
For convenience we added a script, which does all of that for you.

```powershell
PS > ./run.ps1
```

After building the code and publishing the app, the script will set the correct env-vars and run the application.

## Mechanisms shown

The example application creates a trace with two child-calls. It's main purpose is to show you how you can add traces for non-supported databases by 
using the SDK's convenience-methods for mimicking specific types of calls, such as Database-, Http-, Rpc-, Messaging-Calls.

## APIs used

The application uses the following APIs

* `CustomSpan.CreateEntry` => Starts a new trace without looking for any provided context for continuation
* `CustomSpan.CreateExit` => Used for a call leaving a subsystem (in the example leaving the pipeline to call the individual steps)
* 'CustomSpan.AsHttpTo' => Used to mimic an incoming http-call
* 'CustomExitSpan.AsDatabaseCallTo' => Used to mimic an outgoing database-call
* `CustomSpan.WrapAction` => Used for automatic error-handling / -collection
* `CustomSpan.SetTag` => Used for custom annotations to the span to facilitate retrieval

