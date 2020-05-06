$env:CORECLR_ENABLE_PROFILING=""
$env:CORECLR_PROFILER=""
Write-Host Building application
dotnet publish -c Release -o out
Set-Location ./out
Write-Host Setting environment
$env:CORECLR_ENABLE_PROFILING=1
$env:CORECLR_PROFILER="{FA8F1DFF-0B62-4F84-887F-ECAC69A65DD3}"
$env:CORECLR_PROFILER_PATH=$env:CORECLR_PROFILER_PATH=((Get-Location).Path+"\instana_tracing\CoreRewriter_x64.dll")
Write-Host Starting application
dotnet intermediate-spans.dll