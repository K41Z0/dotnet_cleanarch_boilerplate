using Domain.Entities;

namespace Domain.Interfaces;

public interface IMovieRepository
{
    Task<(List<Movie> Movies, int TotalResults)> SearchAsync(MovieFilter filter, CancellationToken cancellationToken = default);
}