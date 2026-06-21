using Application.DTOs;
using Domain.Common;

namespace Application.UseCases.SearchMovies;

public interface ISearchMoviesUseCase
{
    Task<Result<List<MovieDto>>> ExecuteAsync(SearchMoviesQuery query, CancellationToken cancellationToken = default);
}