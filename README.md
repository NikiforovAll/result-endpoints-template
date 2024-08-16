# Nall.ResultEndpoints.Template

[![Build](https://github.com/NikiforovAll/result-endpoints-template/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/NikiforovAll/result-endpoints-template/actions/workflows/build.yml)
[![CodeQL](https://github.com/NikiforovAll/result-endpoints-template/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/NikiforovAll/result-endpoints-template/actions/workflows/codeql-analysis.yml)
[![NuGet](https://img.shields.io/nuget/dt/Nall.ResultEndpoints.Template.svg)](https://nuget.org/packages/Nall.ResultEndpoints.Template)
[![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0.0-yellow.svg)](https://conventionalcommits.org)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/nikiforovall/result-endpoints-template/blob/main/LICENSE.md)

This is a template for creating a new minimal API project with [Ardalis.ApiEndpoints](https://github.com/ardalis/ApiEndpoints) that utilizes the TypedResults in Endpoint (aka replacement for MVC Controllers for Web APIs).

```csharp
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
```

## Installation

```bash
dotnet new install Nall.ResultEndpoints.Template
```

Verify installation:

```bash
dotnet new list result-endpoints-api

# These templates matched your input: 'result-endpoints-api'

# Template Name     Short Name             Language  Tags
# ----------------  ---------------------  --------  ----------------------------
# Result Endpoints   -resultendpoints-api  [C#]      Web/API/Ardalis.ApiEndpoints
```

## Usage

```bash
dotnet new result-endpoints-api --dry-run

# File actions would have been taken:
#   Create: ./.vscode/settings.json
#   Create: ./Data/DbContext.cs
#   Create: ./Endpoints/HelloWorld.cs
#   Create: ./Endpoints/Todo/CreateTodo.cs
#   Create: ./Endpoints/Todo/DeleteTodo.cs
#   Create: ./Endpoints/Todo/GetTodo.cs
#   Create: ./Endpoints/Todo/ListTodos.cs
#   Create: ./Endpoints/Todo/UpdateTodo.cs
#   Create: ./HttpResultsOperationFilter.cs
#   Create: ./Program.cs
#   Create: ./Properties/launchSettings.json
#   Create: ./Seeder.cs
#   Create: ./appsettings.Development.json
#   Create: ./appsettings.json
#   Create: ./content.csproj
```

## Main takeaways

* `TypedResults` directly contributes to OpenAPI. And the ancillary `Results` class describes discriminated unions for the response types.
* `TypedResults` can be used in MVC Controllers and Minimal APIs.
* `Ardalis.ApiEndpoints` is built on top of Controllers, therefore we can use it in conjunction with `TypedResults`.
* `Ardales.ApiEndpoints` has two main classes: `EndpointBaseSync` and `EndpointBaseAsync`, both inherits from `ControllerBase`. It means that automatic model validation is enabled by default. This is why we need to extend generated OpenAPI definition with additional responses produced by model validation. I.e.: in API scenarios, we can add `ProblemDetails` as the way to report validation errors.
* Another important aspect, is exception handling, although, it is not recommended to throw exceptions from client code, sometimes it is inevitable. In this case, exceptions are handled by global error handler. Exceptions are transformed into `ProblemDetails` and returned to the client.
* We can use `EndpointSummary`, `EndpointDescription`, `EndpointName` to extend generated OpenAPI definition in library agnostic way.

## References

- <https://github.com/ardalis/ApiEndpoints>
- <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#problem-details>
- <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-8.0>
- <https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.typedresults?view=aspnetcore-8.0>
- <https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-8.0#validation-failure-error-response>
