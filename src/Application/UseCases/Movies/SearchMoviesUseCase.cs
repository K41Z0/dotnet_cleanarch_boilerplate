using Application.DTOs;
using Domain.Common;
using Domain.Interfaces;

namespace Application.UseCases.Movies;

public class SearchMoviesUseCase(IMovieRepository movieRepository) : ISearchMoviesUseCase
{
    public async Task<Result<PagedList<MovieDto>>> ExecuteAsync(MovieFilter filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var (movies, _, _) = await movieRepository.SearchAsync(
                filter.Text ?? string.Empty, 
                filter.Type, 
                filter.Year, 
                filter.Page, 
                cancellationToken);

            var result = movies.Select(m => new MovieDto
            {
                ImdbId = m.ImdbId,
                Title = m.Title,
                Year = m.Year,
                Type = m.Type,
                Poster = m.Poster
            }).ToList();

            return Result<PagedList<MovieDto>>.Success(new PagedList<MovieDto> {Items = result});
        }
        catch (Exception ex)
        {
            return Result<PagedList<MovieDto>>.Failure(ex.Message);
        }
    }
}