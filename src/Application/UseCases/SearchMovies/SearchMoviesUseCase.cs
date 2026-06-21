using Application.DTOs;
using Domain.Common;
using Domain.Interfaces;

namespace Application.UseCases.SearchMovies;

public class SearchMoviesUseCase : ISearchMoviesUseCase
{
    private readonly IMovieRepository _movieRepository;

    public SearchMoviesUseCase(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<Result<List<MovieDto>>> ExecuteAsync(SearchMoviesQuery query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.Query))
            return Result<List<MovieDto>>.Failure("Search query cannot be empty");

        try
        {
            var movies = await _movieRepository.SearchAsync(query.Query, cancellationToken);

            var dtos = movies.Select(m => new MovieDto
            {
                ImdbId = m.ImdbId,
                Title = m.Title,
                Year = m.Year,
                Type = m.Type,
                Poster = m.Poster
            }).ToList();

            return Result<List<MovieDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<MovieDto>>.Failure($"Failed to search movies: {ex.Message}");
        }
    }
}