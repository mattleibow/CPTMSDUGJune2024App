# if ($IsMacOS) {
#     brew install --cask devtunnel
# }

# DOTNET_DiagnosticPorts=/tmp/exciting-mobile-port,suspend \
#     ./artifacts/bin/Exciting.Mobile/debug_net8.0-maccatalyst_maccatalyst-arm64/Exciting.Mobile.app/Contents/MacOS/Exciting.Mobile

# dotnet trace collect --diagnostic-port /tmp/exciting-mobile-port --format speedscope

# /Users/matthew/Library/Android/sdk
# /Users/matthew/Library/Android/sdk/platform-tools/adb 

# /Users/matthew/Library/Android/sdk/platform-tools/adb shell setprop debug.mono.profile '10.0.2.2:9000,nosuspend,connect'
# dotnet dsrouter android-emu
# dotnet trace collect -p PID_HERE --format speedscope
# dotnet build -t:run -c Release -f net8.0-android Exciting.Mobile -p:AndroidEnableProfiler=true

# dotnet run --project Exciting.AppHost --launch-profile http

# devtunnel host --port-numbers 7258 --allow-anonymous
# devtunnel host --port-numbers 7258 --port-numbers 19030 --allow-anonymous --protocol https
# devtunnel host --port-numbers 7258 --port-numbers 21031 --allow-anonymous --protocol https

# devtunnel delete exciting-tunnel --force
# devtunnel create exciting-tunnel --allow-anonymous && \
# devtunnel port create exciting-tunnel --port-number 7258 --protocol https && \
# devtunnel port create exciting-tunnel --port-number 21031 --protocol https && \
# devtunnel port create exciting-tunnel --port-number 17048 --protocol https && \
# devtunnel host exciting-tunnel