using System.Net.Http.Json;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Clients;

public class OMDbClient : IMovieRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OMDbClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OMDb:ApiKey"] ?? throw new ArgumentNullException("OMDb:ApiKey is missing");
    }

    public async Task<(List<Movie> Movies, int TotalResults, string? Error)> SearchAsync(
        string query,
        string? type = null,
        string? year = null,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<string>
        {
            $"apikey={_apiKey}",
            $"s={Uri.EscapeDataString(query)}",
            "type=movie"
        };

        if (!string.IsNullOrWhiteSpace(type))
            queryParams.Add($"type={type}");

        if (!string.IsNullOrWhiteSpace(year))
            queryParams.Add($"y={year}");

        if (page > 1)
            queryParams.Add($"page={page}");

        var url = "?" + string.Join("&", queryParams);

        var response = await _httpClient.GetFromJsonAsync<OMDbSearchResponse>(url, cancellationToken);

        if (response == null)
            return (new List<Movie>(), 0, "No response from OMDb API");

        if (response.Response == "False")
            return (new List<Movie>(), 0, response.Error ?? "Unknown error from OMDb API");

        var movies = response.Search?.Select(item => new Movie
        {
            ImdbId = item.imdbID,
            Title = item.Title,
            Year = item.Year,
            Type = item.Type,
            Poster = item.Poster
        }).ToList() ?? new List<Movie>();

        int totalResults = int.TryParse(response.TotalResults, out var total) ? total : 0;

        return (movies, totalResults, null);
    }

    private class OMDbSearchResponse
    {
        public string Response { get; set; } = "False";
        public string? Error { get; set; }
        public string? TotalResults { get; set; }
        public List<OMDbMovieItem>? Search { get; set; }
    }

    private class OMDbMovieItem
    {
        public string imdbID { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
    }
}