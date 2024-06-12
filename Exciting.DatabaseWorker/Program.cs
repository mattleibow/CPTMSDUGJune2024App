using Exciting.Database;
using Exciting.DatabaseWorker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// TODO
builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

// TODO
builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.Services.AddDbContext<ExcitingDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ExcitingDb"));
});

var host = builder.Build();
host.Run();
