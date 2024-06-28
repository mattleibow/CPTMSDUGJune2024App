using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.Hosting;

// Adds common .NET Aspire services: service discovery, resilience, health checks, and OpenTelemetry.
// This code is the client equivalent of the ServiceDefaults project. See https://aka.ms/dotnet/aspire/service-defaults
public static class AppDefaultsExtensions
{
    public static MauiAppBuilder AddAppDefaults(this MauiAppBuilder builder)
    {
        builder.ConfigureAppOpenTelemetry();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            http.AddServiceDiscovery();
        });

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Transient<IMauiInitializeService, OpenTelemetryInitializer>(_ => new OpenTelemetryInitializer()));

        return builder;
    }

    public static IConfigurationBuilder AddAspireApp(this IConfigurationBuilder builder, Dictionary<string, string> settings, string? devTunnelId = null)
    {
        var copy = new Dictionary<string, string>(settings);

        if (!string.IsNullOrWhiteSpace(devTunnelId))
        {
            foreach (var setting in copy)
            {
                if (setting.Value.Contains("localhost"))
                {
                    // source format is `http[s]://localhost:[port]`
                    // tunnel format is `http[s]://exciting-tunnel-[port].devtunnels.ms`
                    var newVal = Regex.Replace(
                        setting.Value,
                        @"://localhost\:(\d+)(.*)",
                        $"://{devTunnelId}-$1.devtunnels.ms$2");

                    copy[setting.Key] = newVal;
                }
            }
        }

        builder.AddInMemoryCollection(copy);

        return builder;
    }

    class OpenTelemetryInitializer : IMauiInitializeService
    {
        public void Initialize(IServiceProvider services)
        {
            services.GetService<MeterProvider>();
            services.GetService<TracerProvider>();
            // TODO: Uncomment when LoggerProvider is public, with OpenTelemetry.Api version 1.9.0
            //services.GetService<LoggerProvider>();
        }
    }

    public static MauiAppBuilder ConfigureAppOpenTelemetry(this MauiAppBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddRuntimeInstrumentation()
                    .AddAppMeters();
            })
            .WithTracing(tracing =>
            {
                if (builder.Configuration.IsDevelopment())
                {
                    // We want to view all traces in development
                    tracing.SetSampler(new AlwaysOnSampler());
                }

                tracing
                    // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                    //.AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static MauiAppBuilder AddOpenTelemetryExporters(this MauiAppBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.SetOpenTelemetryEnvironmentVariables();

            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.Exporter package)
        // builder.Services.AddOpenTelemetry()
        //    .UseAzureMonitor();

        return builder;
    }

    private static void SetOpenTelemetryEnvironmentVariables(this MauiAppBuilder builder)
    {
        var settings = builder.Configuration.AsEnumerable();
        foreach (var setting in settings)
        {
            if (setting.Key.StartsWith("OTEL_"))
            {
                Environment.SetEnvironmentVariable(setting.Key, setting.Value);
            }
        }
    }

    private static MeterProviderBuilder AddAppMeters(this MeterProviderBuilder meterProviderBuilder) =>
        meterProviderBuilder.AddMeter(
            "System.Net.Http");
}
