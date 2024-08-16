using Microsoft.EntityFrameworkCore;

namespace ResultEndpoints.Data;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public required DbSet<TodoItem> Todos { get; set; }
}

public record class TodoItem
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public Status Status { get; set; }

    public bool Completed => Status == Status.Completed;

    public int Order { get; set; }
}

public enum Status
{
    NotStarted,
    InProgress,
    Completed
}

public record TodoViewModel
{
    private TodoViewModel() { }

    public int Id { get; private set; }
    public string? Title { get; private set; }

    public Status Status { get; private set; }

    public bool Completed { get;  private set; }

    public static TodoViewModel From(TodoItem todo) =>
        new()
        {
            Id = todo.Id,
            Title = todo.Title,
            Status = todo.Status,
            Completed = todo.Completed
        };
}
