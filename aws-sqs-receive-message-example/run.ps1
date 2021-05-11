$env:CORECLR_ENABLE_PROFILING=1
$env:CORECLR_PROFILER="{FA8F1DFF-0B62-4F84-887F-ECAC69A65DD3}"
$env:CORECLR_PROFILER_PATH="D:\repos\instana-dotnetcore-sdk-examples\aws-sqs-receive-message-example\bin\Debug\netcoreapp3.1\publish\instana_tracing/CoreRewriter_x64.dll"
$env:INSTANA_LOG_SPANS=1
$env:INSTANA_DEBUG_TRACER=1

dotnet AWSSQSExample.dll