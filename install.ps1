# if ($IsMacOS) {
#     brew install --cask devtunnel
# }

# dotnet build Exciting.Mobile -f net8.0-maccatalyst -r maccatalyst-arm64 -c Debug

DOTNET_DiagnosticPorts=/tmp/exciting-mobile-port,suspend \
    ./artifacts/bin/Exciting.Mobile/debug_net8.0-maccatalyst_maccatalyst-arm64/Exciting.Mobile.app/Contents/MacOS/Exciting.Mobile


dotnet trace collect --diagnostic-port /tmp/exciting-mobile-port --format speedscope


# /Users/matthew/Library/Android/sdk
# /Users/matthew/Library/Android/sdk/platform-tools/adb 


# /Users/matthew/Library/Android/sdk/platform-tools/adb shell setprop debug.mono.profile '10.0.2.2:9000,nosuspend,connect'
# dotnet dsrouter android-emu
# dotnet trace collect -p PID_HERE --format speedscope
# dotnet build -t:run -c Release -f net8.0-android Exciting.Mobile -p:AndroidEnableProfiler=true



# dotnet run --project Exciting.AppHost --launch-profile https

# devtunnel host --allow-anonymous --port-numbers 7258

# devtunnel host --port-numbers 7258 --port-numbers 19030 --allow-anonymous --protocol https
# devtunnel host --port-numbers 7258 --port-numbers 21031 --allow-anonymous --protocol https

# devtunnel delete exciting-tunnel --force
# devtunnel create exciting-tunnel --allow-anonymous && \
# devtunnel port create exciting-tunnel --port-number 7258 --protocol https && \
# devtunnel port create exciting-tunnel --port-number 21031 --protocol https && \
# devtunnel port create exciting-tunnel --port-number 17048 --protocol https && \
# devtunnel host exciting-tunnel


# docker pull mcr.microsoft.com/azure-sql-edge:latest
# docker run \
#     -e "ACCEPT_EULA=Y" \
#     -e "MSSQL_SA_PASSWORD=MyPass@word" \
#     -p 1433:1433 \
#     -d \
#     mcr.microsoft.com/azure-sql-edge:latest


# docker build -t team-api-manual .
# docker run -it --rm team-api-manual -p 8080:8080
