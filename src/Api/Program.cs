using Application.UseCases.SearchMovies;
using Domain.Interfaces;
using Infrastructure.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services
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

app.MapGet("/movies/search", async (string query, ISearchMoviesUseCase useCase, CancellationToken ct) =>
{
    if (string.IsNullOrWhiteSpace(query))
        return Results.BadRequest("Query parameter is required");

    var result = await useCase.ExecuteAsync(query, ct);
    return Results.Ok(result);
})
.WithName("SearchMovies")
.WithOpenApi();

app.Run();