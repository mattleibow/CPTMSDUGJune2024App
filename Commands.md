# Commands

## Docker Demo

```sh
# build docker image
docker build -t website-manual .

# run docker container
docker run -it --rm -p 8080:8080 website-manual

# start dev tunnel
devtunnel host --allow-anonymous --port-numbers 8080
```

## Aspire Demo

```sh
# start aspire app
dotnet run --project Exciting.AppHost --launch-profile https

# check manifest
dotnet run --project Exciting.AppHost --publisher manifest --output-path aspire-manifest.json
```

## Aspire + Dev Tunnel Demo

```sh
# delete any existing/old dev tunnel
devtunnel delete exciting-tunnel --force

# create dev tunnel
devtunnel create exciting-tunnel --allow-anonymous

# add the REST API port
devtunnel port create exciting-tunnel --port-number 7258 --protocol https
# add the Open Telemetry port (analytics and logs to the dashboard)
devtunnel port create exciting-tunnel --port-number 21031 --protocol https
# add the Aspire Dashboard
devtunnel port create exciting-tunnel --port-number 17048 --protocol https

# start the dev tunnel
devtunnel host exciting-tunnel
```

## Profiling Demo

```sh
# start the router
dotnet dsrouter android-emu

# either: configure the emulator for startup tracing
adb shell setprop debug.mono.profile '10.0.2.2:9000,suspend,connect'
# or: configure the emulator for ad-hoc tracing
adb shell setprop debug.mono.profile '10.0.2.2:9000,nosuspend,connect'

# build and start the maui app
dotnet build -t:Run -c Release -f net8.0-android -r android-arm64 -p:AndroidEnableProfiler=true Exciting.Mobile

# start collecting
dotnet trace collect --format speedscope -p ADD_PID_HERE
```

## Benchmark Demo

```sh
# run
dotnet run -c Release --project Exciting.Benchmarks
```

```
// * Summary *

BenchmarkDotNet v0.13.13-nightly.20240601.156, macOS Sonoma 14.5 (23F79) [Darwin 23.5.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]   : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD
  ShortRun : .NET 8.0.6 (8.0.624.26715), Arm64 RyuJIT AdvSIMD

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

| Method                  | Mean     | Error     | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------ |---------:|----------:|----------:|------:|---------:|---------:|---------:|----------:|------------:|
| CurrentImpl             | 6.442 ms | 0.0613 ms | 0.0034 ms |  1.00 | 906.2500 | 906.2500 | 359.3750 | 1690228 B |       1.000 |
| NoToArrayImpl           | 9.842 ms | 1.4174 ms | 0.0777 ms |  1.53 |        - |        - |        - |    2028 B |       0.001 |
| SingleLoopImpl          | 2.809 ms | 1.2141 ms | 0.0665 ms |  0.44 | 906.2500 | 906.2500 | 363.2813 | 1690067 B |       1.000 |
| SingleLoopNoToArrayImpl | 2.364 ms | 0.4983 ms | 0.0273 ms |  0.37 |        - |        - |        - |     507 B |       0.000 |
| BestImpl                | 1.458 ms | 0.6455 ms | 0.0354 ms |  0.23 |        - |        - |        - |     425 B |       0.000 |

// * Legends *
  Mean        : Arithmetic mean of all measurements
  Error       : Half of 99.9% confidence interval
  StdDev      : Standard deviation of all measurements
  Ratio       : Mean of the ratio distribution ([Current]/[Baseline])
  Gen0        : GC Generation 0 collects per 1000 operations
  Gen1        : GC Generation 1 collects per 1000 operations
  Gen2        : GC Generation 2 collects per 1000 operations
  Allocated   : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  Alloc Ratio : Allocated memory ratio distribution ([Current]/[Baseline])
  1 ms        : 1 Millisecond (0.001 sec)
```