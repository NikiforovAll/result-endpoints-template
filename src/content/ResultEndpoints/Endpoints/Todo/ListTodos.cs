using Microsoft.AspNetCore.Http.HttpResults;
using ResultEndpoints.Data;

namespace ResultEndpoints.Endpoints.Todo;

public record ListTodosRequest
{
    [FromQuery(Name = "completed")]
    public bool? Completed { get; set; }
}

public class ListTodos(TodoContext context)
    : EndpointBaseSync.WithRequest<ListTodosRequest>.WithResult<Ok<IEnumerable<TodoViewModel>>>
{
    [EndpointSummary("List todos")]
    [EndpointDescription("List todos based on the request")]
    [Tags("Todo")]
    [EndpointName(nameof(ListTodos))]
    [HttpGet("/todos")]
    public override Ok<IEnumerable<TodoViewModel>> Handle(ListTodosRequest request)
    {
        var query = context.Todos.AsQueryable();

        if (request.Completed.HasValue)
        {
            query = query.Where(t => t.Completed == request.Completed.Value);
        }

        var items = query.OrderBy(t => t.Order).ToList();

        return TypedResults.Ok(items.Select(TodoViewModel.From));
    }
}