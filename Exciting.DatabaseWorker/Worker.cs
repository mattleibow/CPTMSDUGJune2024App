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
                // Ensure the database is created
                await context.Database.EnsureCreatedAsync(stoppingToken);

                // Migrate the database to the latest version
                {
                    // using var transaction = await context.Database.BeginTransactionAsync(stoppingToken);
                    // await context.Database.MigrateAsync(stoppingToken);
                    // await transaction.CommitAsync(stoppingToken);
                }

                // Seed the database with some data
                if (!context.Members.Any())
                {
                    var members = new[]
                    {
                        new TeamMember { FirstName = "Allan", LastName = "Pead" },
                        new TeamMember { FirstName = "Carike", LastName = "Botha" },
                        new TeamMember { FirstName = "Dustyn", LastName = "Lightfoot" },
                        new TeamMember { FirstName = "Hennie", LastName = "Francis" },
                        new TeamMember { FirstName = "Louise", LastName = "van der Bijl" },
                        new TeamMember { FirstName = "Matthew", LastName = "Leibowitz" },
                        new TeamMember { FirstName = "Roma", LastName = "Gupta" },
                    };

                    using var transaction = await context.Database.BeginTransactionAsync(stoppingToken);

                    await context.Members.AddRangeAsync(members, stoppingToken);
                    await context.SaveChangesAsync(stoppingToken);

                    await transaction.CommitAsync(stoppingToken);
                }
            });
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        // WORAROUND: VSCode terminates everything if this runs
        // lifetime.StopApplication();
    }
}
