# Annotating auto-trace spans with custom tags

The application in this folder is a simple web-api application, written with the .NET Core Framework. The application can be run from the console.

## Learnings

In this example we demonstrate how you can access a span that has been created automatically and annotate it with your own data of interest by means of tags.
These tags allow you to find the traces and also group services carrying specific tags in their traces in a logical application perspective.

## Anatomy
As part of the example, the application creates traces which are based on auto-tracing. Since auto-tracing is platform dependent at some points, you might need to switch
the referenced nuget package `Instana.Tracing.Core.Rewriter.Windows` with the appropriate package for your platform.
Those are:

* Instana.Tracing.Core.Linux (for every Linux flavor except Alpine)
* Instana.Tracing.Core.Alpine (for Alpine Linux)

## How to run 

Just download the code, publish the app, set the env-vars and start it.
For convenience we added a script, which does all of that for you.

```powershell
PS > ./run.ps1
```

After building the code and publishing the app, the script will set the correct env-vars and run the application.

**NOTE**

In order to see what the application acutally does (i.e. see the traces) you should have an agent running, which is connected to your Instana tenant-unit.

To access the application and trigger traces being created, you can just open a browser and navigate to `http://localhost:5000/weatherforecast`

## Mechanisms shown

On every request to the controller, a trace will automatically be created, based on the automatic tracing for .NET Core.
The points of intereset are in the [WeatherforecastController](./Controllers/WeatherForecastController.cs), it's `Get` method specifically.
When you access the method as shown with the link above, an autotrace will be created. However the `asp.net`  type call, that you will see on the Instana UI carries an additional tag called `SUMMARIES`.

All you need in order to add your custom data to an existing span are two APIs, as shown in the excerpt below:

```C#
using (CustomSpan autospan = CustomSpan.FromCurrentContext())
{
    var rng = new Random();
    var range = Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
    });
    autospan.SetTag("SUMMARIES", string.Join(",", range.Select((fc) => fc.Summary).ToArray()));
    return range.ToArray();
}
```

With a call to `CustomSpan.FromCurrentContext` you get access to the current span on the stack - which is an automatically created span in this case (due to the ASP.NET instrumentation).
After getting the current span, you can just annotate it with a call to `SetTag` as you would normally do with any custom span that you created by means of the SDK.

## APIs used

The application uses the following APIs

* `CustomSpan.FromCurrentContext` => Takes a span from the current call-context if present and returns it
* `CustomSpan.SetTag` => Used for custom annotations to the span


