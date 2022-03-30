namespace Orbus;

public interface IBookFeed
{
    Task<IEnumerable<object>> FetchBooks(CancellationToken cancellationToken);
}