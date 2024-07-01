# Commands

## Docker SQL Demo

```sh
# pull the DB image
docker pull mcr.microsoft.com/azure-sql-edge:latest

# start the DB container
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyPass@word" -p 1433:1433 -d mcr.microsoft.com/azure-sql-edge:latest
```

```sql
USE master

DROP DATABASE IF EXISTS CPTMSDUG;
GO

CREATE DATABASE CPTMSDUG;
GO

USE CPTMSDUG;

CREATE TABLE dbo.Member (ID INT, FullName NVARCHAR(50));
INSERT INTO dbo.Member VALUES (1, N'Allan Pead');
INSERT INTO dbo.Member VALUES (2, N'John Wick');
INSERT INTO dbo.Member VALUES (3, N'John McClane');
GO

SELECT * FROM dbo.Member;
GO
```

```sh
# start the DB container with persisted data
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyPass@word" -p 1433:1433 -d -v ${PWD}/sqldata:/var/opt/mssql mcr.microsoft.com/azure-sql-edge:latest
```

```sql
USE CPTMSDUG;

SELECT * FROM dbo.Member;
GO
```

## Dockerfile Demo

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

```sh
# build the maui app
dotnet build Exciting.Mobile -f net8.0-maccatalyst -r maccatalyst-arm64 -c Debug

# run the app
DOTNET_DiagnosticPorts=~/exciting-trace,suspend ./artifacts/bin/Exciting.Mobile/debug_net8.0-maccatalyst_maccatalyst-arm64/Exciting.Mobile.app/Contents/MacOS/Exciting.Mobile

# start collecting
dotnet trace collect --diagnostic-port ~/exciting-trace --format speedscope
```

## Benchmark Demo

```sh
# run
dotnet run -c Release --project Exciting.Benchmarks
```
