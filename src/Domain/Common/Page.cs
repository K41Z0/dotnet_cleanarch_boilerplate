namespace Domain.Common;

/// <summary>
/// Represents a page of results.
/// Human-friendly pagination container.
/// </summary>
public class Page<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int Number { get; init; } = 1;
    public int Size { get; init; } = 10;
    public int TotalResults { get; init; }

    public int TotalPages => TotalResults == 0 ? 0 : (int)Math.Ceiling(TotalResults / (double)Size);
    public bool HasPreviousPage => Number > 1;
    public bool HasNextPage => Number < TotalPages;
}