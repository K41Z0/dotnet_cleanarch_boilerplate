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

    public async Task<List<MovieDto>> ExecuteAsync(SearchMoviesQuery query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.Query))
            return new List<MovieDto>();

        var movies = await _movieRepository.SearchAsync(query.Query, cancellationToken);

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