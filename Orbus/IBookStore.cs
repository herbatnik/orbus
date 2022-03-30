namespace Orbus;

public interface IBookStore
{
    Task SaveBooks(IEnumerable<Book> books, CancellationToken cancellationToken);
}