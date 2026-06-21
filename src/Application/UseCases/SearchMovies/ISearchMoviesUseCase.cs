using Application.DTOs;

namespace Application.UseCases.SearchMovies;

public interface ISearchMoviesUseCase
{
    Task<List<MovieDto>> ExecuteAsync(string query, CancellationToken cancellationToken = default);
}