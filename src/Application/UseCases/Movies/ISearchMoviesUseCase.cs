using Application.DTOs;
using Domain.Common;
using Domain.Interfaces;

namespace Application.UseCases.Movies;

public interface ISearchMoviesUseCase
{
    Task<Result<List<MovieDto>>> ExecuteAsync(MovieFilter filter, CancellationToken cancellationToken = default);
}