using Exciting.Database;
using Exciting.DatabaseWorker;

var builder = Host.CreateApplicationBuilder(args);

// TODO
builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

// TODO
builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddSqlServerDbContext<ExcitingDbContext>("excitingdb");

var host = builder.Build();
host.Run();
