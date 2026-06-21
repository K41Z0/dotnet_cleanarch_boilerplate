using Application.DTOs;

namespace Application.UseCases.SearchMovies;

public interface ISearchMoviesUseCase
{
    Task<List<MovieDto>> ExecuteAsync(SearchMoviesQuery query, CancellationToken cancellationToken = default);
}