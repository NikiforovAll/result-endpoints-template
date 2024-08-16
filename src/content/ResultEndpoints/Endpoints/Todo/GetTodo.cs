using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using ResultEndpoints.Data;

namespace ResultEndpoints.Endpoints.Todo;

public record GetTodoRequest
{
    [FromRoute]
    public int Id { get; set; }
}

public class GetTodo(TodoContext context)
    : EndpointBaseSync.WithRequest<GetTodoRequest>.WithResult<
        Results<Ok<TodoViewModel>, NotFound<ProblemDetails>>
    >
{
    [EndpointSummary("Get todo")]
    [EndpointDescription("Get todo based on the request")]
    [Tags("Todo")]
    [EndpointName(nameof(GetTodo))]
    [HttpGet("/todos/{id}", Name = nameof(GetTodo))]
    public override Results<Ok<TodoViewModel>, NotFound<ProblemDetails>> Handle(
        [FromRoute] GetTodoRequest request
    )
    {
        var todo = context.Todos.FirstOrDefault(t => t.Id == request.Id);

        if (todo == null)
        {
            return TypedResults.NotFound(TypedResults.Problem("Not Found").ProblemDetails);
        }

        return TypedResults.Ok(TodoViewModel.From(todo));
    }
}
