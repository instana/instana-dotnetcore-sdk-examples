# Annotating intermediate spans with custom tags

Here is an easy way to annotate the custom intermediate spans you might create to trace internal methods of your services.

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

The app creates an entry-span (as a message being received) and then calls a worker-class, which will create intermediate spans for the things it does.
Intermediate spans come in handy when you want to have insights into the inner workings of your own libraries. 

## How this works

All that has to be done is to use the 'CustomSpan.Create' API, which will allow you to create an intermediate span.
Once the span has been created, you can annotate it just like any other span by using the 'SetTag'  API.
