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
                        new TeamMember { FirstName = "Roma", LastName = "Gupta", Tasks = { new TaskItem { Title = "Copilots, PowerPlatform and Copilots in PowerPlatform" } } },
                        new TeamMember { FirstName = "Matthew", LastName = "Leibowitz", Tasks = { new TaskItem { Title = "Watch time" }, new TaskItem { Title = "Finish up now" } } },
                        new TeamMember { FirstName = "Hennie", LastName = "Francis", Tasks = { new TaskItem { Title = "Book venue" } } },
                        new TeamMember { FirstName = "Allan", LastName = "Pead", Tasks = { new TaskItem { Title = "Talk about IoT" }, new TaskItem { Title = "Talk about AI" }, new TaskItem { Title = "Don't water the garden like a pleb" } } },
                        new TeamMember { FirstName = "Carike", LastName = "Botha", Tasks = { new TaskItem { Title = "Demo that PowerApp" } } },
                        new TeamMember { FirstName = "Dustyn", LastName = "Lightfoot", Tasks = { new TaskItem { Title = "Conect to the CPTMSDUG stream" } } },
                        new TeamMember { FirstName = "Louise", LastName = "van der Bijl", Tasks = { new TaskItem { Title = "Make sure Spike wears clothes" } } },
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
