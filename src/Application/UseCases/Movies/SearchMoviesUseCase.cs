using Application.DTOs;
using Domain.Common;
using Domain.Interfaces;

namespace Application.UseCases.Movies;

public class SearchMoviesUseCase(IMovieRepository movieRepository) : ISearchMoviesUseCase
{
    public async Task<Result<Page<MovieDto>>> ExecuteAsync(MovieFilter filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var (movies, totalResults, _) = await movieRepository.SearchAsync(
                filter.Text ?? string.Empty,
                filter.Type,
                filter.Year,
                filter.Page,
                cancellationToken);

            var dtos = movies.Select(m => new MovieDto
            {
                ImdbId = m.ImdbId,
                Title = m.Title,
                Year = m.Year,
                Type = m.Type,
                Poster = m.Poster
            }).ToList();

            const int pageSize = 10;
            var pageNumber = filter.Page > 0 ? filter.Page : 1;

            var pagedItems = dtos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var page = new Page<MovieDto>
            {
                Items = pagedItems,
                Number = pageNumber,
                Size = pageSize,
                TotalResults = totalResults
            };

            return Result<Page<MovieDto>>.Success(page);
        }
        catch (Exception ex)
        {
            return Result<Page<MovieDto>>.Failure(ex.Message);
        }
    }
}