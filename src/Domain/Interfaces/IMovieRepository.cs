using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Interfaces;

public interface IMovieRepository
{
    Task<(List<Movie> Movies, int TotalResults)> SearchAsync(MovieSearchFilter filter, CancellationToken cancellationToken = default);
}