using Domain.Common;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IMovieRepository : IRepository<Movie, MovieFilter>
{
}
