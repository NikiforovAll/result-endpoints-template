using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using ResultEndpoints.Data;

namespace ResultEndpoints.Endpoints.Todo;

public record UpdateTodoRequest
{
    [FromRoute]
    [Range(1, int.MaxValue)]
    public required int Id { get; set; }

    [FromBody]
    public required UpdateTodoDto Dto { get; set; }

    public record UpdateTodoDto
    {
        public Status? Status { get; set; }

        public int? Order { get; set; }
    }
}

public class UpdateTodo(TodoContext context)
    : EndpointBaseSync.WithRequest<UpdateTodoRequest>.WithResult<
        Results<NoContent, NotFound<ProblemDetails>, BadRequest<ProblemDetails>>
    >
{
    [EndpointSummary("Update todo")]
    [EndpointDescription("Update todo based on the request")]
    [Tags("Todo")]
    [EndpointName(nameof(UpdateTodo))]
    [HttpPut("/todos/{id}")]
    public override Results<NoContent, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> Handle(
        [FromRoute] UpdateTodoRequest request
    )
    {
        var todo = context.Todos.FirstOrDefault(t => t.Id == request.Id);

        if (todo == null)
        {
            return TypedResults.NotFound(TypedResults.Problem("Not Found").ProblemDetails);
        }

        if (request.Dto.Status.HasValue)
        {
            todo.Status = request.Dto.Status.Value;
        }

        if (request.Dto.Order.HasValue)
        {
            todo.Order = request.Dto.Order.Value;
        }

        context.SaveChanges();

        return TypedResults.NoContent();
    }
}
