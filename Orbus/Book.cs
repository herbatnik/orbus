namespace Orbus;

public class Book
{
    public string? Category { get; init; }

    public string? Title { get; init; }

    public IEnumerable<string> Authors { get; init; } = Enumerable.Empty<string>();

    public decimal? Price { get; init; }
}
