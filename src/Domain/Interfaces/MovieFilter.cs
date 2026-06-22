namespace Domain.Interfaces;

public class MovieFilter
{
    public string? Text { get; set; }
    public string? Type { get; set; }
    public string? Year { get; set; }
    public int? Page { get; set; }
}