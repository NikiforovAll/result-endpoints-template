using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using ResultEndpoints.Data;

namespace ResultEndpoints.Endpoints.Todo;

public record CreateTodoRequest
{
    [Required]
    [Length(1, 100)]
    public required string Title { get; set; }

    [Range(1, int.MaxValue)]
    public int Order { get; set; }

    public static TodoItem From(CreateTodoRequest todo) =>
        new() { Title = todo.Title, Order = todo.Order };
}

public class CreateTodo(TodoContext context)
    : EndpointBaseAsync.WithRequest<CreateTodoRequest>.WithResult<CreatedAtRoute<TodoViewModel>>
{
    [EndpointSummary("Create todo")]
    [EndpointDescription("Create todo based on the request")]
    [Tags("Todo")]
    [EndpointName(nameof(CreateTodo))]
    [HttpPost("/todos")]
    public override async Task<CreatedAtRoute<TodoViewModel>> HandleAsync(
        [FromBody] CreateTodoRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var item = CreateTodoRequest.From(request);
        context.Todos.Add(item);

        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.CreatedAtRoute(
            TodoViewModel.From(item),
            nameof(GetTodo),
            new RouteValueDictionary(new { id = item.Id })
        );
    }
}
