using Domain.Common;

namespace Domain.Interfaces;

public class MovieFilter : SearchFilter
{
    public string? Type { get; set; }
    public string? Year { get; set; }
    public int Page { get; set; } = 1;
}