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
            var result = await movieRepository.SearchAsync(filter, cancellationToken);

            if (!result.IsSuccess || result.Value == null)
            {
                return Result<Page<MovieDto>>.Failure(result.Error ?? "Failed to retrieve movies");
            }

            var dtos = result.Value.Items.Select(m => new MovieDto
            {
                ImdbId = m.ImdbId,
                Title = m.Title,
                Year = m.Year,
                Type = m.Type,
                Poster = m.Poster
            }).ToList();

            var page = new Page<MovieDto>
            {
                Items = dtos,
                Number = result.Value.Number,
                Size = result.Value.Size,
                TotalResults = result.Value.TotalResults
            };

            return Result<Page<MovieDto>>.Success(page);
        }
        catch (Exception ex)
        {
            return Result<Page<MovieDto>>.Failure(ex.Message);
        }
    }
}