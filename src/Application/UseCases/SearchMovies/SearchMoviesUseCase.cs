using Application.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.SearchMovies;

public class SearchMoviesUseCase : ISearchMoviesUseCase
{
    private readonly IMovieRepository _movieRepository;

    public SearchMoviesUseCase(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<List<MovieDto>> ExecuteAsync(string query, CancellationToken cancellationToken = default)
    {
        var movies = await _movieRepository.SearchAsync(query, cancellationToken);

        return movies.Select(m => new MovieDto
        {
            ImdbId = m.ImdbId,
            Title = m.Title,
            Year = m.Year,
            Type = m.Type,
            Poster = m.Poster
        }).ToList();
    }
}