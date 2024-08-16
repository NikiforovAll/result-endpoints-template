using Microsoft.EntityFrameworkCore;
using ResultEndpoints.Data;

namespace ResultEndpoints;

public sealed class DataSeeder(IDbContextFactory<TodoContext> contextFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var context = contextFactory.CreateDbContext();

        context.Todos.AddRange(
            [
                new()
                {
                    Title = "Buy milk",
                    Order = 1,
                    Status = Status.Completed
                },
                new()
                {
                    Title = "Buy bread",
                    Order = 2,
                    Status = Status.NotStarted
                },
                new()
                {
                    Title = "Buy eggs",
                    Order = 3,
                    Status = Status.InProgress
                }
            ]
        );

        await context.SaveChangesAsync(stoppingToken);
    }
}
