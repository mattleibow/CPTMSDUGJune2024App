using Exciting.Database;
using Exciting.DatabaseWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddSqlServerDbContext<ExcitingDbContext>("excitingdb");

var host = builder.Build();
host.Run();
