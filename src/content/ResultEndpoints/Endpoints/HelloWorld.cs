using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ResultEndpoints.Endpoints;

public class HelloWorld
    : EndpointBaseSync.WithRequest<string>.WithResult<
        Results<Ok<string>, BadRequest<ProblemDetails>>
    >
{
    [EndpointSummary("Says hello")]
    [EndpointDescription("Says hello based on the request")]
    [Tags("Hello")]
    [EndpointName(nameof(HelloWorld))]
    [HttpGet("/hello-world")]
    public override Results<Ok<string>, BadRequest<ProblemDetails>> Handle(
        [FromQuery(Name = "q")] [Length(3, 100)] string request
    )
    {
        if (request == "error")
        {
            throw new Exception("Something went wrong...");
        }
        else if (request == "badrequest")
        {
            return TypedResults.BadRequest(TypedResults.Problem("Something bad happened").ProblemDetails);
        }

        return TypedResults.Ok($"Hello, {request}");
    }
}
