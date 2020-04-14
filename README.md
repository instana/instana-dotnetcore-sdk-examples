# Instana SDK Examples for .NET Core
This repository provides examples on how to use the Instana Tracing SDK for .NET Core, starting with .NET Core 2.1.
Instana provides a nuget-package for the SDK, which you can add to your applications : https://www.nuget.org/packages/Instana.Tracing.Core.Sdk/

Once installed, you can use the APIs the SDK provides to create your own, codified traces for your applications.
The projects inside this repo display different scenarios in which the SDK can be helpful, in addition to automatic tracing.

# Capabilities
The SDK lets you create spans, which compose a trace, that will be ultimately available on your Instana unit. To create those spans and transport meaningful data, it exposes the following capabilities

* create spans for EXIT, ENTRY and INTERMEDIATE
* add tags to every span
* add data to every span
* add a result to a span
* add an exception to a span if applicable

# Setup
You only have to add the nuget-package to your project and add the code to create the spans. This usually follows the following pattern:

```C#
private void MyMethod()
{
    using(var span = CustomSpan.Create(...))
    {
        // your original method's code
    }
}
```

The APIs on `CustomSpan` expose several overloads, according to what you want to achieve.
You can read up on the documentation here : https://docs.instana.io/ecosystem/dot-net/tracing-sdk/#installing-the-sdk

Additionally you need running instana-agent on the machine that runs your application.