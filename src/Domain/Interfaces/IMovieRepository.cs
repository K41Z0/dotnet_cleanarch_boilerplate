using Domain.Entities;

namespace Domain.Interfaces;

public interface IMovieRepository
{
    Task<(List<Movie> Movies, int TotalResults, string? Error)> SearchAsync(
        string query,
        string? type = null,
        string? year = null,
        int page = 1,
        CancellationToken cancellationToken = default);
}