# Instana SDK Examples for .NET Core
This repository provides examples on how to use the Instana Tracing SDK for .NET Core, starting with .NET Core 2.1.
The Instana .NET Core SDK is provided as a separate Nuget package, called [Instana.Tracing.Core.Sdk](https://www.nuget.org/packages/Instana.Tracing.Core.Sdk/), which you need to add to your applications in order to make of the Instana .NET Core SDK. 
For more documentation on how to install the Instana .NET Core SDK nuget package, refer to the [Installing the SDK](https://docs.instana.io/ecosystem/dot-net/tracing-sdk/#installing-the-sdk) section of the Instana .NET Core documentation.

Once the Instana .NET Core SDK nuget package is installed, the APIs the SDK enable you to create your own, codified traces for your applications.

# Pre-requisites

The Instana .NET Core SDK assumes that your application is monitored by an Instana host agent.
Without an Instana host agent monitoring your application, the SDK methods behave as no-op.
Refer to the Instana documentation for [installing an Instana hosgt agent](https://docs.instana.io/setup_and_manage/host_agent) and [enabling .NET Core monitoring with Instana](https://docs.instana.io/ecosystem/dot-net-core#tracing).

# Capabilities

The projects inside this repo display different scenarios in which the SDK can be helpful as additions to the automatic tracing that Instana already provides.
For more information on Instana's automatic tracing for .NET Core, refer to the [Instana .NET Core](https://docs.instana.io/ecosystem/dot-net-core) documentation.

In a nutshell, the .NET Core SDK lets you create spans and add additional tags on spans automatically created for you by Instana.
For more information on spans, traces and distributed tracing, refer to the [Concepts of tracing](https://docs.instana.io/tracing/concepts) page of the Instana documentation.

More in detail, the .NET Core SDK allows you to:

* create spans of type `EXIT`, `ENTRY` and `INTERMEDIATE`
* add tags to spans
* add results to spans
* add an exception to a span and mark it erroneous

# Using the SDK

Creating spans usually follows the following pattern:

```C#
private void MyMethod()
{
    using(var span = CustomSpan.Create(...))
    {
        // your original method's code
    }
}
```

The APIs on `CustomSpan` expose several method overloads that will make it easier for you to accomplish your goal.

More information on the methods exposed by the SDK is available in the [.NET Tracing SDK](https://docs.instana.io/ecosystem/dot-net/tracing-sdk) documentation, and it applicable for both .NET and .NET Core.
