using Domain.Common;

namespace Domain.Interfaces;

public class MovieFilter : Filter
{
    public string? Type { get; set; }
    public string? Year { get; set; }
}