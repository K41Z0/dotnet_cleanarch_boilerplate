using Application.UseCases.SearchMovies;
using Domain.Interfaces;
using Infrastructure.Clients;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/movies/search", async (string q, ISearchMoviesUseCase useCase, CancellationToken ct) =>
{
    var result = await useCase.ExecuteAsync(new SearchMoviesQuery(q), ct);
    return Results.Ok(result);
})
.WithName("SearchMovies")
.WithOpenApi();

app.Run();