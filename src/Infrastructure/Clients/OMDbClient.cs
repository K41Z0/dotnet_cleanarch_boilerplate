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

    public async Task<List<Movie>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        var url = $"?apikey={_apiKey}&s={Uri.EscapeDataString(query)}&type=movie";

        var response = await _httpClient.GetFromJsonAsync<OMDbSearchResponse>(url, cancellationToken);

        if (response?.Search == null)
            return new List<Movie>();

        return response.Search.Select(item => new Movie
        {
            ImdbId = item.imdbID,
            Title = item.Title,
            Year = item.Year,
            Type = item.Type,
            Poster = item.Poster
        }).ToList();
    }

    private class OMDbSearchResponse
    {
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