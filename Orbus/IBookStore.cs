namespace Orbus;

public interface IBookStore
{
    Task SaveBooks(IEnumerable<object> books, CancellationToken cancellationToken);
}