using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using ResultEndpoints.Data;

namespace ResultEndpoints.Endpoints.Todo;

public record DeleteTodoRequest
{
    [FromRoute]
    [Range(1, int.MaxValue)]
    public required int Id { get; set; }
}

public class DeleteTodo(TodoContext context)
    : EndpointBaseSync.WithRequest<DeleteTodoRequest>.WithResult<
        Results<NoContent, NotFound<ProblemDetails>, BadRequest<ProblemDetails>>
    >
{
    [EndpointSummary("Delete todo")]
    [EndpointDescription("Delete todo based on the request")]
    [Tags("Todo")]
    [EndpointName(nameof(DeleteTodo))]
    [HttpDelete("/todos/{id}")]
    public override Results<NoContent, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> Handle(
        DeleteTodoRequest request
    )
    {
        var todo = context.Todos.FirstOrDefault(t => t.Id == request.Id);

        if (todo == null)
        {
            return TypedResults.NotFound(TypedResults.Problem("Not Found").ProblemDetails);
        }

        if (todo.Completed)
        {
            return TypedResults.BadRequest(
                TypedResults.Problem("Unable to delete completed todo").ProblemDetails
            );
        }

        context.Todos.Remove(todo);
        context.SaveChanges();

        return TypedResults.NoContent();
    }
}
