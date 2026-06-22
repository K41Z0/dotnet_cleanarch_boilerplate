namespace Application.Filters;

public class MovieSearchFilter
{
    public string Query { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? Year { get; set; }
    public int Page { get; set; } = 1;
}