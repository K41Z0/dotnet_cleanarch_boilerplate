using Application.UseCases.SearchMovies;
using Domain.Interfaces;
using Infrastructure.Clients;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.ListenLocalhost(5162);
    }
    else
    {
        options.ListenLocalhost(5162);
        options.ListenLocalhost(7209, listenOptions => listenOptions.UseHttps());
    }
});

builder.Services.AddHttpClient<IMovieRepository, OMDbClient>(client =>
{
    client.BaseAddress = new Uri("https://www.omdbapi.com/");
});

builder.Services.AddScoped<ISearchMoviesUseCase, SearchMoviesUseCase>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/movies/search", async (
    [AsParameters] MovieSearchFilter filter,
    ISearchMoviesUseCase useCase,
    CancellationToken ct) =>
{
    if (string.IsNullOrWhiteSpace(filter.Query))
        return Results.BadRequest("Query parameter is required");

    var result = await useCase.ExecuteAsync(filter, ct);

    if (!result.IsSuccess)
        return Results.BadRequest(new { error = result.Error });

    return Results.Ok(result.Value);
})
.WithName("SearchMovies")
.WithOpenApi();

app.Run();