using Application.DTOs;
using Domain.Common;
using Domain.Interfaces;

namespace Application.UseCases.SearchMovies;

public interface ISearchMoviesUseCase
{
    Task<Result<List<MovieDto>>> ExecuteAsync(MovieSearchFilter filter, CancellationToken cancellationToken = default);
}