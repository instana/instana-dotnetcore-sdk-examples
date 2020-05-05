Write-Host Setting environment
$env:CORECLR_ENABLE_PROFILING=1
$env:CORECLR_PROFILER="{FA8F1DFF-0B62-4F84-887F-ECAC69A65DD3}"
$env:CORECLR_PROFILER_PATH=$env:CORECLR_PROFILER_PATH=((Get-Location).Path+"\instana_tracing\CoreRewriter_x64.dll")
$env:INSTANA_LOG_SPANS="true"
$env:INSTANA_DEBUG_TRACER=1
$env:DOTNET_STARTUP_HOOKS=$env:CORECLR_PROFILER_PATH=((Get-Location).Path+"\Instana.Tracing.Core.dll")
Write-Host Starting application
dotnet annotate-autotrace-spans.dll