using Domain.Entities;

namespace Domain.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> SearchAsync(string query, CancellationToken cancellationToken = default);
}