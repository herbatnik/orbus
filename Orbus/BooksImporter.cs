namespace Orbus;

// Question: What is the main problem with the BooksImport class?
// it was
// - violating the single responsibility
// - using deprecated WebClient
// - 
public class BooksImporter
{
    private readonly IBookFeed _bookFeed;
    private readonly IBookStore _bookStore;

    public BooksImporter(
        IBookFeed bookFeed,
        IBookStore bookStore)
    {
        _bookFeed = bookFeed;
        _bookStore = bookStore;
    }

    public async Task Import(CancellationToken cancellationToken)
    {
        // download books xml from endpoint
        var books = await _bookFeed.FetchBooks(cancellationToken);

        // save books to the database
        await _bookStore.SaveBooks(books, cancellationToken);
    }
}