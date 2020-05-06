# Annotating auto-trace spans with custom tags

In this example we demonstrate how you can access a span that has been created automatically and annotate it with your own data of intereset.
Accessing such a span is only possible while it has not been closed already. This is typically the case for Controller-Methods in ASP.NET Core or for entries in messaging-systems like RabbitMQ.

## How to run this example

Just download the code, publish the app, set the env-vars and start it.
For convenience we added a script, which does all of that for you.

```powershell
PS > ./run.ps1
```

After building the code and publishing the app, the script will set the correct env-vars and run the application.

**NOTE**
In order to see what the application acutally does (i.e. see the traces) you should have an agent running, which is connected to your Instana tenant-unit.

To access the application and trigger traces being created, you can just open a browser and navigate to `http://localhost:5000/weatherforecast`

## What the app does

The points of intereset are in the [WeatherforecastController](./Controllers/WeatherForecastController.cs), it's `Get` method specifically.
When you access the method as shown with the link above, an autotrace will be created. However the `asp.net`  type call, that you will see on the Instana UI carries an additional tag called `SUMMARIES`.

## How this works

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

With a call to `CustomSpan.FromCurrentContext` you get access to the current span on the stack.
After getting the current span, you can just annotate it with a call to `SetTag` as you would normally do with any custom span that you created by means of the SDK.
