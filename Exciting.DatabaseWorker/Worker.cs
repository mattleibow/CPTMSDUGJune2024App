using System.Diagnostics;
using Exciting.Database;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace Exciting.DatabaseWorker;

public class Worker(
    IServiceScopeFactory scopeFactory,
    IHostApplicationLifetime lifetime) : BackgroundService
{
    public const string  ActivitySourceName = "DatabaseWorker";

    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = ActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ExcitingDbContext>();

            var strategy = new SqlServerRetryingExecutionStrategy(
                context,
                maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: [ 0 ]);

            await strategy.ExecuteAsync(async () =>
            {
                await context.Database.EnsureCreatedAsync(stoppingToken);

                // using var transaction = await context.Database.BeginTransactionAsync(stoppingToken);
                // await context.Database.MigrateAsync(stoppingToken);
                // await transaction.CommitAsync(stoppingToken);
            });
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        // lifetime.StopApplication();
    }
}
