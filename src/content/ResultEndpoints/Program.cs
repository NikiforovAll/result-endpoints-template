using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ResultEndpoints;
using ResultEndpoints.Data;

var builder = WebApplication.CreateBuilder(args);

var enumConverter = new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower);

builder.Services.AddDbContextFactory<TodoContext>(options =>
{
    options.UseInMemoryDatabase("Todos");
});

builder.Services.AddHostedService<DataSeeder>();

builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.OperationFilter<HttpResultsOperationFilter>();
});

builder
    .Services.AddControllers(options =>
    {
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), 500));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), 400));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(enumConverter);
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});

builder.Services.ConfigureHttpJsonOptions(opt =>
{
    // this one is used during the serialization of the response TypedResults, ref: https://github.com/dotnet/aspnetcore/issues/45872
    opt.SerializerOptions.Converters.Add(enumConverter);
});

var app = builder.Build();

app.UseStaticFiles();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.InjectStylesheet("/swagger-ui/theme-muted.css");
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

app.Run();
