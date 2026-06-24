using System.Net.Http.Json;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataSources;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class MovieRepository(OmdbDataSource omdbDataSource) : IMovieRepository
{
    public async Task<Result<Movie>> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var response = await omdbDataSource.Get(id, cancellationToken);

        switch (response)
        {
            case null:
                return Result<Movie>.Failure("No response from OMDb");
            case {Value.Search.Count: <= 0 }:
                return Result<Movie>.Failure("Movie not found");
            default:
            {
                var movies = response.Value?.Search?.Select(item => new Movie
                {
                    ImdbId = item.ImdbId,
                    Title = item.Title,
                    Year = item.Year,
                    Type = item.Type,
                    Poster = item.Poster
                }).ToList() ?? [];
        
                return Result<Movie>.Success(movies.First());
            }
        }
    }

    public async Task<Result<Page<Movie>>> SearchAsync(MovieFilter filter, CancellationToken cancellationToken = default)
    {
        var query = new List<string?>();
        if (!string.IsNullOrEmpty(filter.Text))
            query.Add($"s={Uri.EscapeDataString(filter.Text)}");

        if (!string.IsNullOrWhiteSpace(filter.Type))
            query.Add($"type={filter.Type}");

        if (!string.IsNullOrWhiteSpace(filter.Year))
            query.Add($"y={filter.Year}");

        if (filter.Page is > 1)
            query.Add($"page={filter.Page}");

        var url = "?" + string.Join("&", query);
        var response = await omdbDataSource.Get(url, cancellationToken);

        if (response.Value is null)
            return Result<Page<Movie>>.Failure("No response from OMDb API");

        if (response.Value?.Response == "False")
            return Result<Page<Movie>>.Failure(response.Error ?? "Unknown error from OMDb API");

        var movies = response.Value?.Search?.Select(item => new Movie
        {
            ImdbId = item.ImdbId,
            Title = item.Title,
            Year = item.Year,
            Type = item.Type,
            Poster = item.Poster
        }).ToList() ?? [];

        var totalResults = int.TryParse(response.Value?.TotalResults, out var total) ? total : 0;
        var pageNumber = filter.Page ?? 1;
        const int pageSize = 10;

        return Result<Page<Movie>>.Success(new Page<Movie>
        {
            Items = movies,
            Number = pageNumber,
            Size = pageSize,
            TotalResults = totalResults
        });
    }
}