using System.Net.Http.Json;
using Domain.Common;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DataSources;

public class OmdbDataSource(HttpClient httpClient, IConfiguration configuration)
{
    private List<string> BaseQuery 
        => [$"apikey={_apiKey}"];
    
    private readonly string _apiKey 
        = configuration["OMDb:ApiKey"] 
          ?? throw new ArgumentNullException($"OMDb:ApiKey is missing for {nameof(OmdbDataSource)}");

    public async Task<Result<SearchResponse>> Get(string query, CancellationToken cancellationToken = default)
    {
        var baseQuery = BaseQuery;
        baseQuery.Add(Uri.EscapeDataString(query));
            
        var url = "?" + string.Join("&", query);
        var response = await httpClient.GetFromJsonAsync<SearchResponse>(url, cancellationToken);
        
        if (response is null)
            return Result<SearchResponse>.Failure("ERROR");
        
        return  Result<SearchResponse>.Success(response);
    }
}

public class SearchResponse
{
    public string? Response { get; init; }
    public string? Error { get; init; }
    public string? TotalResults { get; init; }
    public List<MovieItem>? Search { get; init; }
}

public abstract class MovieItem
{
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Poster { get; set; } = string.Empty;
}