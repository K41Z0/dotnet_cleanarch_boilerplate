namespace Domain.Interfaces;

/// <summary>
/// Typed filter for searching movies using OMDb API (supports query parameters binding).
/// </summary>
public class MovieSearchFilter
{
    public string Query { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? Year { get; set; }
    public int Page { get; set; } = 1;
}