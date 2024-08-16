using Microsoft.AspNetCore.Http.HttpResults;

namespace ResultEndpoints.Endpoints;

public class HelloWorld : EndpointBaseSync.WithRequest<string>.WithResult<Ok<string>>
{
    [EndpointSummary("Says hello")]
    [EndpointDescription("Says hello based on the request")]
    [Tags("Hello")]
    [EndpointName(nameof(HelloWorld))]
    [HttpGet("/hello-world")]
    public override Ok<string> Handle([FromQuery(Name = "q")] string request)
    {
        if (request == "error")
        {
            throw new Exception("Something went wrong...");
        }

        return TypedResults.Ok($"Hello, {request}");
    }
}
