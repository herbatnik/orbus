namespace Orbus;

public interface IBookFeed
{
    Task<IEnumerable<Book>> FetchBooks(CancellationToken cancellationToken);
}