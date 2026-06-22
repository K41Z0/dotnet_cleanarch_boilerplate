using Application.DTOs;
using Domain.Common;
using Domain.Interfaces;

namespace Application.UseCases.Movies;

public class PagedList<T>
{
    public IReadOnlyList<T> Items { get; set; } = [];
    public int Number { get; set; } = 1;
    public int Size { get; set; } = 5;
}

public interface ISearchMoviesUseCase
{
    Task<Result<PagedList<MovieDto>>> ExecuteAsync(MovieFilter filter, CancellationToken cancellationToken = default);
}