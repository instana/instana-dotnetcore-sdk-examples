# Using agent-configuration for instrumentation

The .NET Core Tracer offers the ability to instrument code on the fly, given the according configuration. While the feature-set is yet limited in comparison to the auto-instrumentation features, this allows for easy instrumentation of custom code or not yet auto-instrumented libraries.

## How the configuration-based approach works

In order for this mechanism to work, there are several components that need to be present and aware of the configuration. This means that a process needs to be restarted in order to be correctly instrumented by configuration.

1. The IL-rewriter needs to get the information to rewrite the methods accordingly
2. The managed tracer needs to get the information to correctly configure the instrumentation-strategies and yield the correct spans.

The configuration is done using the `configuration.yaml` file of the agent in `<agent-dir>/etc/instana/configurationyaml`

## Sample configuration

Here is an example of the configuration file, which will be explained further down:

```yaml
com.instana.plugin.netcore:
  sensor:
    enabled: true
  tracing:
    enabled: true
  instrumentation:
    sdk:
      targets:
        - match:
            type: class
            class: DataService.Services.DataService
            method: GetDataForTrip
            arguments: 1
          span:
            name: DataRetrieval
            type: INTERMEDIATE
            tags:
              - name: trip-id
                kind: argument
                index: 0

```

Now let's look at what this configuration does. Everything around the configuration-based sdk can be found under the node `sdk`.

The instrumentation supports multiple so-called `targets`. A target is a definition of _WHAT_ should be instrumented and _HOW_ it translates to a span.
A `target` consists of a `match` (the _WHAT_) and a `span` (the _HOW_) node.

### Defining the matcher

Looking at the `match` we see the following properties:

`type`: The type of instrumentation to be applied. For now only `class` is supported. This means that we want to instrument a specific method on a class. Later versions will also support the instrumentation of `interface` and `baseclass` which has different effects on how the rewriter locates the methods to be rewritten.

`class`: The fully qualified name of the class that should be instrumented.

`method`: The name of the method we want to instrument

`arguments`: The number of arguments of the method to be instrumented. This is used to seperate different overloads of methods with the same name.

So in this example we want to instrument the method `GetDataForTrip` which has `1` argument and is a member of the `class` `DataService.Services.DataService`.

### Defining the span

After we instructed the rewriter with the `match` about which method to rewrite, we now need to instruct the managed tracer what kind of span we want to generate for this method.
Looking at the `span`-node we have the following properties:

`name`: An arbitrary name which will be used as a default-label for the span (more on this later)

`type`: The kind of span we want to generate, can be any of `ENTRY`, `EXIT` or `INTERMEDIATE`

`tags`: A list of tags that should be added to the span. There are currently two different kinds of tags that can be added (see `tag.kind`)

`tag.name` : The name of the tag. Instana has some well-known tag-names, which will be used in a specific way (like `db.statement` as a label for database-exits).

`tag.kind`: Defines where the value for the tag is populated from. Can be one of `constant` or `argument`.

`tag.index`: Only valid when defining a tag of type `argument`. In this case the `index` is the zero based index of the argument passed to the instrumented method. The value of that argument will be put as a value onto the tag.

'tag.value`: The constant expression that should be added as a tag-value (can be any valid string)

