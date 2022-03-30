using System.Xml.Linq;

namespace Orbus;

public class BookFeed : IBookFeed
{
    // if this Url is meant to be variable - it could be injected in constructor (either as a string or via IConfiguration / IOptions<>)
    private const string Url = "https://www.w3schools.com/xml/books.xml";

    private readonly HttpClient _httpClient;

    public BookFeed(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Book>> FetchBooks(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(Url, cancellationToken);

        // parse books xml to a books collection
        var document = await XDocument.LoadAsync(
            await response.Content.ReadAsStreamAsync(cancellationToken),
            LoadOptions.None,
            cancellationToken);

        var books = document.Descendants("book")
            .Select(bookXml => new Book
            {
                // there was no requirement on nullability, depending on requirements this code should throw, filter or handle such use cases as per requirements
                Category = bookXml.Attribute("category")?.Value,
                Title = bookXml.Element("title")?.Value,
                Authors = bookXml.Elements("author").Select(e => e.Value),
                Price = decimal.TryParse(bookXml.Element("price")?.Value, out var parsedPrice) ? parsedPrice : null,
            });
        return books;
    }
}